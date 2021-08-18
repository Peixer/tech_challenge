using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.DTO;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Services
{
    public interface IAvailabilityService
    {
        void InsertAvailability(Availability availability, string username);
        bool IsValidSlotTime(Availability availability);
        Task<List<Availability>> GetAvailabilitiesByUserId(List<string> userIds, DateTime startDate, DateTime endDate);
        List<string> SplitRangeHours(Availability availability);
        List<HourAvailability> GetHoursAvailabilities(List<Availability> availabilitiesCandidate,
            List<Availability> availabilitiesInterviewers);

        List<HourAvailability> GetMatchesFromAvailabilities(List<HourAvailability> hourAvailabilities);
        Task<List<Availability>> Find();   
    }
}