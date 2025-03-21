using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyAuthCrudApi.Models;

[Index(nameof(Username), IsUnique = true)] // Unik di database
public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public required string Password { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}

