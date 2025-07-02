using MediCareCMS.Models;

namespace MediCareCMS.Service
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}
