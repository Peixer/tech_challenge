using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Services;
using TechChallenge.WebApp.Validators;

namespace TechChallenge.WebApp.Controllers
{
    [ApiController]
    [Authorize(Roles = "Interviewer")]
    [Route("api/interviewers")]
    public class InterviewerController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public InterviewerController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Availability availability)
        {
            var validator = new AvailabilityValidator();
            var validRes = await validator.ValidateAsync(availability);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }
            if (!_availabilityService.IsValidSlotTime(availability))
            {
                return new BadRequestObjectResult("Start time or end time is incorrect");
            }

            var username = User?.FindFirst(ClaimTypes.Name)?.Value;

            _availabilityService.InsertAvailability(availability, username);
            return new ObjectResult(availability) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _availabilityService.Find());
        }
    }
}