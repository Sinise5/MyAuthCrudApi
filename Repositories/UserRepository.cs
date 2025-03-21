using Microsoft.EntityFrameworkCore;
using MyAuthCrudApi.Data;
using MyAuthCrudApi.Models;
using BCrypt.Net;

namespace MyAuthCrudApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        return (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) ? user : null;
    }

    public async Task<User> RegisterAsync(User user, string password)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
