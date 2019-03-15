using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Internal
{
    internal struct PasswordString
    {
        public PasswordString(string rawPassword) : this() 
            => RawPassword = rawPassword.NotNull(nameof(rawPassword));

        public string RawPassword { get; }
        public string AsSha256()
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(RawPassword)));
            }
        }

        public static implicit operator PasswordString(string raw)
        {
            raw.NotNull(nameof(raw));

            return new PasswordString(raw);
        }
    }
}
