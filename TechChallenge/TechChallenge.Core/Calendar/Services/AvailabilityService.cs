using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.DTO;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Repositories;

namespace TechChallenge.Core.Calendar.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IUserRepository _userRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository, IUserRepository userRepository)
        {
            _availabilityRepository = availabilityRepository;
            _userRepository = userRepository;
        }

        public async void InsertAvailability(Availability availability, string username)
        {
            availability.User = await _userRepository.FindUserByUsername(username);
            await _availabilityRepository.Insert(availability);
        }

        public bool IsValidSlotTime(Availability availability)
        {
            var errorParserStartTime = DateTime.TryParseExact(availability.StartTime, "htt", null,
                DateTimeStyles.AssumeLocal, out var startDate);
            var errorParserEndTime = DateTime.TryParseExact(availability.EndTime, "htt", null,
                DateTimeStyles.AssumeLocal, out var endDate);

            if (!errorParserStartTime || !errorParserEndTime)
                return false;

            return endDate.Hour >= startDate.Hour;
        }

        public async Task<List<Availability>> GetAvailabilitiesByUserId(List<string> userIds, DateTime startDate, DateTime endDate)
        {
            var availabilities = await _availabilityRepository.Find(userIds);

            return availabilities.Where(x =>
                    (x.StartDate < endDate && x.StartDate >= startDate) ||
                    (x.EndDate > startDate && x.EndDate <= endDate) || 
                    (x.StartDate < startDate && x.EndDate > endDate))
                .ToList();
        }

        public List<string> SplitRangeHours(Availability availability)
        {
            var hoursToReturn = new List<string>();

            var startDate = DateTime.ParseExact(availability.StartTime, "htt", null);
            var endDate = DateTime.ParseExact(availability.EndTime, "htt", null);

            while (startDate.Hour != endDate.Hour)
            {
                hoursToReturn.Add(startDate.ToString("htt").ToLower());
                startDate = startDate.AddHours(1);
            }

            return hoursToReturn;
        }

        public List<HourAvailability> GetHoursAvailabilities(List<Availability> availabilitiesCandidate,
            List<Availability> availabilitiesInterviewers)
        {
            var hoursAvailabilities = new List<HourAvailability>();
            AddingHoursAvailabilities(availabilitiesCandidate, hoursAvailabilities);
            AddingHoursAvailabilities(availabilitiesInterviewers, hoursAvailabilities);

            return hoursAvailabilities;
        }

        private void AddingHoursAvailabilities(List<Availability> availabilities,
            List<HourAvailability> hoursAvailabilities)
        {
            availabilities.ForEach(availability =>
            {
                var hours = SplitRangeHours(availability);

                hoursAvailabilities.AddRange(hours.Select(hour => new HourAvailability() {Hour = hour, User = availability.User, DayOfWeek = availability.DayOfWeek}));
            });
        }

        public List<HourAvailability> GetMatchesFromAvailabilities(List<HourAvailability> hoursAvailabilities)
        {
            var groupBy = hoursAvailabilities.GroupBy(x => x.KeyGroup).ToList();

            var matches = groupBy.Where(x =>
                    x.Count() > 1 && x.Any(y => y.User.Role == UserRole.Candidate) &&
                    x.Any(y => y.User.Role == UserRole.Interviewer))
                .Select(group => group.First())
                .ToList();

            return matches;
        }

        public Task<List<Availability>> Find()
        {
            return this._availabilityRepository.Find();
        }
    }
}