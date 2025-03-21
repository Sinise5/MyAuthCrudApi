using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAuthCrudApi.Models;
using MyAuthCrudApi.Repositories;
using MyAuthCrudApi.Services;
using MyAuthCrudApi.Data;

namespace MyAuthCrudApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly AuthService _authService;
    private readonly AppDbContext _context;

    public AuthController(IUserRepository userRepo, AuthService authService, AppDbContext context)
    {
        _userRepo = userRepo;
        _authService = authService;
        _context = context; // ✅ Sekarang _context tidak error
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Validasi model
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Cek apakah username sudah terdaftar
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Username is already taken." });
        }

        // Validasi password tidak boleh kosong
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest(new { message = "Password is required." });
        }

        // Panggil repository untuk mendaftarkan user
        var newUser = await _userRepo.RegisterAsync(user, user.Password);
        
        // Berikan respons sukses
        return Ok(new { message = "User registered successfully", user = newUser });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        Console.WriteLine($"[DEBUG] Mencari user dengan {user.Password} username: {user.Username}");    

        var authenticatedUser = await _userRepo.AuthenticateAsync(user.Username, user.Password);
         if (authenticatedUser == null) 
        {
            Console.WriteLine("[ERROR] User tidak ditemukan atau password salah.");
            return Unauthorized(new { message = "Invalid username or password" });
        }

        Console.WriteLine("[DEBUG] User ditemukan. Membuat token...");
    
        var token = _authService.GenerateToken(authenticatedUser);
    
        Console.WriteLine($"[DEBUG] Token berhasil dibuat: {token}");
        return Ok(new { token });
    }

    [HttpPost("jwt")]
    public IActionResult Jwt()
    {
        byte[] keyBytes = new byte[32]; // 256-bit key
        new Random().NextBytes(keyBytes);
        string base64Key = Convert.ToBase64String(keyBytes);

        Console.WriteLine($"JWT Key (Base64 Encoded): {base64Key}");
        return Ok(new { base64Key });
    }

    

}
