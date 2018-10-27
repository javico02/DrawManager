using System;
using System.Security.Cryptography;
using System.Text;

namespace DrawManager.Api.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HMACSHA512 _hasherFunc;

        public PasswordHasher()
        {
            // TODO: Change the key to use for encoding
            _hasherFunc = new HMACSHA512(Encoding.UTF8.GetBytes("realworld"));
        }

        public byte[] Hash(string password, byte[] salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password);

            var allBytes = new byte[bytes.Length + salt.Length];
            Buffer.BlockCopy(bytes, 0, allBytes, 0, bytes.Length);
            Buffer.BlockCopy(salt, 0, allBytes, bytes.Length, salt.Length);

            return _hasherFunc.ComputeHash(allBytes);
        }
    }
}
