using MediCareCMS.Models;

namespace MediCareCMS.Repository
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);
    }
}
