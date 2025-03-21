using MyAuthCrudApi.Models;

namespace MyAuthCrudApi.Repositories;

public interface IUserRepository
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User> RegisterAsync(User user, string password);
}
