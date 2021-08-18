using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Services;
using TechChallenge.Core.Calendar.Util.Auth;
using TechChallenge.WebApp.Validators;

namespace TechChallenge.WebApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

            var insertWithSuccess = await _userService.Insert(user);

            return !insertWithSuccess ? 
                new BadRequestObjectResult("Existing username, please enter another username") : 
                new ObjectResult(user) {StatusCode = StatusCodes.Status201Created};
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.Find());
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

            User userLogged = await _userService.FindUser(user.Username, user.Password);

            if (userLogged == null)
            {
                return new BadRequestObjectResult("Username or password is incorrect");
            }

            var token = TokenService.GenerateToken(userLogged);

            return new ObjectResult(new {user = userLogged, token}) {StatusCode = StatusCodes.Status201Created};
        }
    }
}