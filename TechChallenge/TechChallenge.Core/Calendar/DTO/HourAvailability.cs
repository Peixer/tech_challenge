using System;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.DTO
{
    public class HourAvailability
    {
        public User User { get; set; }
        public string Hour { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public string KeyGroup => Hour + DayOfWeek;
    }
}