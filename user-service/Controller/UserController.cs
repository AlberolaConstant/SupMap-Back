using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Linq;


namespace UserService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        public UserController(UserDbContext context) => _context = context;
        //fonction qui verifie si l'utilisateur est authentifié et existe
        private async Task<User?> GetAuthenticatedUserAsync()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return null;
            var user = await _context.Users.FindAsync(userId);
            return user;
        }

        // R�cup�rer tous les utilisateurs
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {  
        var user = await GetAuthenticatedUserAsync();
        if (user == null)
            return Unauthorized("Utilisateur non authentifié ou supprimé.");
        var users = await _context.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                CreationDate = user.CreationDate
            })
            .ToListAsync();
        return Ok(users);
        }

        // R�cup�rer le nom et role d'un utilisateur par son ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null)
                return Unauthorized("Utilisateur non authentifié ou supprimé.");
            
            var userSearch = await _context.Users.FindAsync(id);
            var UserNameDto = new UserNameDto
            {
                Id = userSearch.Id,
                UserName = userSearch.UserName,
            };

        return Ok(UserNameDto);
        }
        
        [HttpGet("me")]
        public async Task<IActionResult> GetUserMe()
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null)
                return Unauthorized("Utilisateur non authentifié ou supprimé.");

            var UserDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                CreationDate = user.CreationDate
            };
            return Ok(UserDto);
        }

        // Cr�er un nouvel utilisateur
        [HttpPost("create")]
        public async Task<IActionResult> Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Mettre � jour un utilisateur existant
        [HttpPut("update/me")]
        public async Task<IActionResult> Update(UserUpdateDto updated)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null)
                return Unauthorized("Utilisateur non authentifié ou supprimé.");

            // Mise � jour de UserName
            if (!string.IsNullOrWhiteSpace(updated.UserName))
                user.UserName = updated.UserName;
            if (!string.IsNullOrWhiteSpace(updated.Email))
                user.Email = updated.Email;
            // Mise � jour du mot de passe si fourni
            if (!string.IsNullOrWhiteSpace(updated.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(updated.Password);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Supprimer un utilisateur
        [HttpDelete("delete/me")]
        public async Task<IActionResult> Delete()
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null)
                return Unauthorized("Utilisateur non authentifié ou supprimé.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
