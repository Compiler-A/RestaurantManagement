using BusinessLayerRestaurant.Interfaces;
using System.Runtime.CompilerServices;



namespace BusinessLayerRestaurant.Classes
{
    public class clsSH256 : ISHA256
    {
        public string SH256Hashing(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

    public class clsBCrypt : IBCrypt
    {
        public string BCryptHashing(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

    public class  clsHashingService : IHashingService
    {
        ISHA256 _sha256;
        IBCrypt _bcrypt;

        public clsHashingService(ISHA256 SHA, IBCrypt BCrypt)
        {
            _sha256 = SHA;
            _bcrypt = BCrypt;
        }

        public string SH256Hashing(string password)
        {
            return _sha256.SH256Hashing(password);
        }

        public string BCryptHashing(string password)
        {
            return _bcrypt.BCryptHashing(password);
        }

        public bool ValidationBCrypt(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public bool ValidationSHA256(string password, string hashedPassword)
        {
            return SH256Hashing(password) == hashedPassword;
        }
    }
}