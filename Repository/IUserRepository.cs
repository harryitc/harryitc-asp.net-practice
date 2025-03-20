using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerShop.Repository
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
    }

}
