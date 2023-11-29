using System.Security.Cryptography;
using System.Text;

namespace Task1
{
    internal class Program
    {
        static void Main()
        {
            const string text = "somethingwithMagic123";
            var salt = Encoding.UTF8.GetBytes(text);
            var test1 = new OldImplementation().GeneratePasswordHashUsingSalt(text, salt);
            var test2 = new UpdatedImplementation().GeneratePasswordHashUsingSalt(text, salt);

            Console.ReadLine();
        }
    }

    internal class OldImplementation
    {
        // pervious
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }
    }
    internal class UpdatedImplementation
    {
        //updated
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, 10000);
            byte[] hashBytes = new byte[36];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(pbkdf2.GetBytes(20), 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public string GeneratePasswordHashUsingSaltWithSpan(string passwordText, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, 10000);
            Span<byte> hashBytes = stackalloc byte[36];
            salt.AsSpan().CopyTo(hashBytes);
            pbkdf2.GetBytes(20).AsSpan().CopyTo(hashBytes.Slice(16));

            return Convert.ToBase64String(hashBytes.ToArray());
        }
    }
}
