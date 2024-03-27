using CRUDinNETCORE.Models;

namespace CRUDinNETCORE.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int Id);
        Task<User?> AddandUpdateUser(User userObj);
    }
}
