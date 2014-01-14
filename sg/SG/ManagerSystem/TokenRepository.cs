using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace ManagerSystem
{
    public class TokenRepository
    {
        static public Token getToken()
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

        static managersystemEntities ms_ent = new managersystemEntities();

        static private Token Copy(Token tmp)
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

        static public Token Find(int id)
        {
            return ms_ent.TokenSet.Find(id);
        }

        static public Token Find(string token)
        {
            var obj = ms_ent.TokenSet.First(o => o.token == token);

            if (obj == null)
                return null;

            return Copy(obj);
        }

        static public Token Sanitize(Token d)
        {
            return Copy(d);
        }

        static public List<Token> FindAll()
        {
            List<Token> l = new List<Token>();

            var lq_l = from d in ms_ent.TokenSet select d;
            foreach (var singleToken in lq_l)
            {
                Token t = Copy(singleToken);

                l.Add(t);
            }
            return l;
        }

        static public string RegenerateToken(Token t)
        {
            if (t == null)
                return "";

            t.is_valid = false;
            t.updated = DateTime.Now;
            InsertOrUpdate(t);

            Token new_token = TokenRepository.getToken();
            new_token.Desguace = t.Desguace;
            new_token.Taller = t.Taller;
            InsertOrUpdate(new_token);
            
            Save();

            return new_token.token;
        }

        static public void InsertOrUpdate(Token token)
        {
            if (token.Id == default(int))
            {
                // New entity
                ms_ent.TokenSet.Add(token);
            }
            else
            {
                // Existing entity
                Token t = Find(token.Id);
                t.is_valid = token.is_valid;
                t.updated = token.updated;
            }
        }

        static public void Delete(int id)
        {
            var token = ms_ent.TokenSet.Find(id);
            ms_ent.TokenSet.Remove(token);
        }

        static public void Save()
        {
            ms_ent.SaveChanges();
        }

        static public void Dispose()
        {
            ms_ent.Dispose();
        }
    }
}