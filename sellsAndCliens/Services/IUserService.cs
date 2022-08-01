using SellingManagementSystem.Models;

namespace SellingManagementSystem.Services
{
    public interface IUserService
    {
        string Auth(User credentials);
    }
}
