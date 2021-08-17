using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Repositories;
using TechChallenge.Core.Calendar.Util.Auth;
using TechChallenge.WebApp.Validators;

namespace TechChallenge.WebApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var validator = new UserValidator();
            var validRes = await validator.ValidateAsync(user);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }
            
            var userWithSameUsername = await _userRepository.FindUserByUsername(user.Username);

            if (userWithSameUsername != null)
            {
                return new BadRequestObjectResult("Existing username, please enter another username");
            }

            await _userRepository.Insert(user);
            return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userRepository.Find());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var validator = new UserLoginValidator();
            var validRes = await validator.ValidateAsync(user);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }

            User userLogged = await _userRepository.FindUser(user.Username, user.Password);

            if (userLogged == null)
            {
                return new BadRequestObjectResult("Username or password is incorrect");
            }

            var token = TokenService.GenerateToken(userLogged);

            return new ObjectResult(new {user = userLogged, token}) { StatusCode = StatusCodes.Status201Created };
        }
    }
}