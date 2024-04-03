using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Helpers
{
   public static class PasswordHelper
    {
        public static string GetSaltedPasswordHash(string password)
        {
            string Key = "123@EngIbrahim";
            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
            byte[] salt = Encoding.UTF8.GetBytes(Key);
            byte[] saltedPassword = new byte[pwdBytes.Length + salt.Length];

            Buffer.BlockCopy(pwdBytes, 0, saltedPassword, 0, pwdBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, pwdBytes.Length, salt.Length);
            SHA1 sha = SHA1.Create();
            return Convert.ToBase64String(sha.ComputeHash(saltedPassword));
        }
    }
}
