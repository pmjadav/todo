using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Data;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            User user = new User
            {
                UserName = userRegisterDTO.UserName
            };
            var userId = await _authRepo.Register(user, userRegisterDTO.Password);
            if (userId == 0)
            {
                return BadRequest($"Cannot register {userRegisterDTO.UserName}");
            }
            return Ok($"User {userRegisterDTO.UserName} rergistered successfully!");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var token = await _authRepo.Login(userLoginDTO.UserName, userLoginDTO.Password);
            if (token == null)
            {
                return BadRequest("Incorrect username or password!");
            }
            return Ok(token);
        }
    }
}
