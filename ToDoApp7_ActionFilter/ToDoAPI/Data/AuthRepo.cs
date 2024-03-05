using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoAPI.Models;

namespace ToDoAPI.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ToDoContext _context;
        private readonly IConfiguration _config;
        public AuthRepo(ToDoContext context, IConfiguration config) 
        {
            _context = context;
            _config = config;
        }
        public async Task<int> Register(User user, string password)
        {
            if (await UserExists(user.UserName))
                return 0;
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
        public async Task<string> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                                u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
                return null;    // User not found
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;    // Password doesn't match
            }
            return CreateToken(user);
        }
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string password, 
                                        out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var appSettingsToken = _config.GetSection("AppSettings:Token").Value;
            if (appSettingsToken == null)
            {
                throw new Exception("Appsettings token is null!");
            }
            SymmetricSecurityKey symSecKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials signCredentials = 
                new SigningCredentials(symSecKey, SecurityAlgorithms.HmacSha512);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signCredentials
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken secToken = tokenHandler.CreateToken(tokenDesc);

            return tokenHandler.WriteToken(secToken); // Serializes token to a string
        }
    }
}
