using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface ISHA256
    {
        string SH256Hashing(string password);
    }

    public interface IBCrypt
    {
        string BCryptHashing(string password);
    }

    public interface IHashingService : ISHA256, IBCrypt
    {
        bool ValidationBCrypt(string password, string hashedPassword);
        bool ValidationSHA256(string password, string hashedPassword);
    }

}
