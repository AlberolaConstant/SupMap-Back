namespace AuthService.Models;
using System;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "User"; // "User" ou "Admin"
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
