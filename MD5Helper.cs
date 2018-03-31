using System;
using System.Security.Cryptography;
using System.Text;

namespace NodeEditor.Utils
{
    public static class MD5Helper
    {
        public static string GetMd5Hash(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var hash = InnerGetMd5Hash(md5Hash, source);

                return hash;
            }
        }

        public static int GetMd5HashInt(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var hash = InnerGetMd5HashInt(md5Hash, source);

                return hash;
            }
        }

        public static bool VerifyMd5Hash(string source, string hash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return InnerVerifyMd5Hash(md5Hash, source, hash);
            }
        }

        private static string InnerGetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static int InnerGetMd5HashInt(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BitConverter.ToInt32(data, 0);
        }

        private static bool InnerVerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = InnerGetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}