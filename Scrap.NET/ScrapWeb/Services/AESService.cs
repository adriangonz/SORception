using ScrapWeb.DataAccess;
using ScrapWeb.Entities;
using ScrapWeb.Exceptions;
using ScrapWeb.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Net;
using System.Web;

namespace ScrapWeb.Services
{
    public class AESService
    {
        private GenericRepository<AESPairEntity> AESPairRepository;
        private ScrapContext scrapContext;



        public AESService(ScrapContext context = null)
        {
            if (context == null)
                scrapContext = new ScrapContext();
            else
                scrapContext = context;
            AESPairRepository = new GenericRepository<AESPairEntity>(scrapContext);
        }


        public AESPairEntity getMyPair()
        {
            AESPairEntity mypair = AESPairRepository.Get(t => t.type == AESPairEntity.AESPairType.MINE).Last();
            if (mypair == null)
                throw new ServiceException("My AESPair no exist", HttpStatusCode.NotFound);
            return mypair;
        }

        public AESPairEntity getSGPair()
        {
            AESPairEntity mypair = AESPairRepository.Get(t => t.type == AESPairEntity.AESPairType.SG).Last();
            if (mypair == null)
                throw new ServiceException("My AESPair no exist", HttpStatusCode.NotFound);
            return mypair;
        }

        public bool generateMyPair()
        {
            AESPairEntity aes_pair = generateAppAESPair();
            aes_pair.type = AESPairEntity.AESPairType.MINE;
            AESPairRepository.Insert(aes_pair);
            scrapContext.SaveChanges();
            return true;
        }

        public bool saveSGPair(byte[] key, byte[] iv)
        {
            AESPairEntity aes_pair = createAESPair(key,iv);
            aes_pair.type = AESPairEntity.AESPairType.SG;
            AESPairRepository.Insert(aes_pair);
            scrapContext.SaveChanges();
            return true;
        }

        public AESPairEntity createAESPair(byte[] key, byte[] iv)
        {
            AESPairEntity pair = new AESPairEntity()
            {
                key = key,
                iv = iv
            };
            AESPairRepository.Insert(pair);
            return pair;
        }

        public AESPairEntity generateAppAESPair()
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            return this.createAESPair(myRijndael.Key, myRijndael.IV);
        }

        public string encryptMessage_with_MyPair(string message)
        {
            AESPairEntity aes_pair = getMyPair();
            byte[] encrypted_message = encrypt_function(message, aes_pair.key, aes_pair.iv);
            return getString(encrypted_message);
        }

        public string decryptMessage_with_MyPair(string message)
        {
            AESPairEntity aes_pair = getMyPair();
            byte[] byte_message = getBytes(message);
            return decrypt_function(byte_message, aes_pair.key, aes_pair.iv);
        }

        public string decryptMessage_with_SGPair(string message)
        {
            AESPairEntity aes_pair = getSGPair();
            byte[] byte_message = getBytes(message);
            return decrypt_function(byte_message, aes_pair.key, aes_pair.iv);
        }

        public string encryptMessage(string message, AESPairEntity aes_pair)
        {
            byte[] encrypted_message = encrypt_function(message, aes_pair.key, aes_pair.iv);
            return getString(encrypted_message);
        }

        public string decryptMessage(string message, AESPairEntity aes_pair)
        {
            byte[] byte_message = getBytes(message);
            return decrypt_function(byte_message, aes_pair.key, aes_pair.iv);
        }

        static byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private static byte[] encrypt_function(string Plain_Text, byte[] Key, byte[] IV)
        {

            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;

            //I crypto transform is used to perform the actual decryption vs encryption, hash function are also a version of crypto transform.
            ICryptoTransform Encryptor = null;
            //Crypto streams allow for encryption in memory.
            CryptoStream Crypto_Stream = null;

            System.Text.UTF8Encoding Byte_Transform = new System.Text.UTF8Encoding();

            //Just grabbing the bytes since most crypto functions need bytes.
            byte[] PlainBytes = Byte_Transform.GetBytes(Plain_Text);

            try
            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;

                MemStream = new MemoryStream();

                //Calling the method create encryptor method Needs both the Key and IV these have to be from the original Rijndael call
                //If these are changed nothing will work right.
                Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);

                //The big parameter here is the cryptomode.write, you are writing the data to memory to perform the transformation
                Crypto_Stream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);

                //The method write takes three params the data to be written (in bytes) the offset value (int) and the length of the stream (int)
                Crypto_Stream.Write(PlainBytes, 0, PlainBytes.Length);

            }
            finally
            {
                //if the crypto blocks are not clear lets make sure the data is gone
                if (Crypto != null)
                    Crypto.Clear();
                //Close because of my need to close things when done.
                Crypto_Stream.Close();
            }
            //Return the memory byte array
            return MemStream.ToArray();
        }


        private static string decrypt_function(byte[] Cipher_Text, byte[] Key, byte[] IV)
        {
            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Decryptor = null;
            CryptoStream Crypto_Stream = null;
            StreamReader Stream_Read = null;
            string Plain_Text;

            try
            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;

                MemStream = new MemoryStream(Cipher_Text);

                //Create Decryptor make sure if you are decrypting that this is here and you did not copy paste encryptor.
                Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);

                //This is different from the encryption look at the mode make sure you are reading from the stream.
                Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);

                //I used the stream reader here because the ReadToEnd method is easy and because it return a string, also easy.
                Stream_Read = new StreamReader(Crypto_Stream);
                Plain_Text = Stream_Read.ReadToEnd();
            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();

                MemStream.Flush();
                MemStream.Close();

            }
            return Plain_Text;
        }

    }
}