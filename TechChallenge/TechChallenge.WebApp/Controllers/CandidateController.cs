using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Repositories;
using TechChallenge.Core.Calendar.Services;
using TechChallenge.WebApp.Models;
using TechChallenge.WebApp.Validators;
using TechChallenge.WebApp.ViewModels;

namespace TechChallenge.WebApp.Controllers
{
    [ApiController]
    [Route("api/candidates")]
    public class CandidateController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IUserRepository _userRepository;

        public CandidateController(IAvailabilityService availabilityService, IUserRepository userRepository)
        {
            _availabilityService = availabilityService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Candidate")]
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

        [HttpPost("/{id}/filter")]
        public async Task<IActionResult> FilterPeriods(string id, FilterPeriodsModel filter)
        {
            var userIdsCandidate = new List<string>() {id};
            var availabilitiesCandidate = await _availabilityService.GetAvailabilitiesByUserId(userIdsCandidate, filter.StartDate, filter.EndDate);
            var availabilitiesInterviewers = await _availabilityService.GetAvailabilitiesByUserId(filter.Interviewers, filter.StartDate, filter.EndDate);

            var hourAvailabilities = _availabilityService.GetHoursAvailabilities(availabilitiesCandidate, availabilitiesInterviewers);
            var matches = _availabilityService.GetMatchesFromAvailabilities(hourAvailabilities);

            var viewModel = matches.Select(x => new HourAvailabilityViewModel(x));
            
            return Ok(viewModel);
        }
    }
}