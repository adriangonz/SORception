using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace ManagerSystem
{
    public class RToken
    {
        public managersystemEntities db_context;

        public RToken()
        {
            db_context = new managersystemEntities();
        }

        public RToken(managersystemEntities context)
        {
            db_context = context;
        }

        public Token getToken()
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            string token_string = output.ToString();

            Token t = new Token();
            t.created = DateTime.Now;
            t.updated = DateTime.Now;
            t.is_valid = true;
            t.token = token_string;

            return t;
        }


        private Token Copy(Token tmp)
        {
            Token d = new Token();
            d.Id = tmp.Id;
            d.updated = tmp.updated;
            d.created = tmp.created;
            d.token = tmp.token;
            d.is_valid = tmp.is_valid;
            d.Desguace = tmp.Desguace;
            d.Taller = tmp.Taller;
            return d;
        }

        public Token Find(int id)
        {
            return db_context.TokenSet.Find(id);
        }

        public Token Find(string token)
        {
            var obj = db_context.TokenSet.First(o => o.token == token);

            if (obj == null)
                return null;

            return Copy(obj);
        }

        public Token Sanitize(Token d)
        {
            return Copy(d);
        }

        public List<Token> FindAll()
        {
            List<Token> l = new List<Token>();

            var lq_l = from d in db_context.TokenSet select d;
            foreach (var singleToken in lq_l)
            {
                Token t = Copy(singleToken);

                l.Add(t);
            }
            return l;
        }

        public string RegenerateToken(Token t)
        {
            if (t == null)
                return "";

            t.is_valid = false;
            t.updated = DateTime.Now;
            InsertOrUpdate(t);

            Token new_token = getToken();
            new_token.Desguace = t.Desguace;
            new_token.Taller = t.Taller;
            InsertOrUpdate(new_token);
            
            Save();

            return new_token.token;
        }

        public void InsertOrUpdate(Token token)
        {
            if (token.Id == default(int))
            {
                // New entity
                db_context.TokenSet.Add(token);
            }
            else
            {
                // Existing entity
                Token t = Find(token.Id);
                t.is_valid = token.is_valid;
                t.updated = token.updated;
            }
        }

        public void Delete(int id)
        {
            var token = db_context.TokenSet.Find(id);
            db_context.TokenSet.Remove(token);
        }

        public void Save()
        {
            db_context.SaveChanges();
        }

        public void Dispose()
        {
            db_context.Dispose();
        }
    }
}