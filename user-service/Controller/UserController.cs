using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using BCrypt.Net;
using System.Threading.Tasks;

namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        public UserController(UserDbContext context) => _context = context;

        // Récupérer tous les utilisateurs
        [HttpGet]
        public async Task<IActionResult> GetUsers() =>
            Ok(await _context.Users.ToListAsync());

        // Récupérer un utilisateur par son ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // Créer un nouvel utilisateur
        [HttpPost("create")]
        public async Task<IActionResult> Create(User user)
        {
            // Hash du mot de passe
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Mettre à jour un utilisateur existant
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, User updated)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Mise à jour de UserName
            user.UserName = updated.UserName;
            user.Email = updated.Email;

            // Mise à jour du mot de passe si fourni
            if (!string.IsNullOrWhiteSpace(updated.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(updated.Password);

            // Mise à jour du rôle
            user.Role = updated.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Supprimer un utilisateur
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
