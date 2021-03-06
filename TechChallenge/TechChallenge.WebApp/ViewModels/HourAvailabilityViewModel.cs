using System;
using TechChallenge.Core.Calendar.DTO;

namespace TechChallenge.WebApp.ViewModels
{
    public class HourAvailabilityViewModel
    {
        public HourAvailabilityViewModel(HourAvailability hourAvailability)
        {
            From = hourAvailability.Hour;
            To = DateTime.ParseExact(hourAvailability.Hour, "htt", null).AddHours(1).ToString("htt").ToLower();
            DayOfWeek = hourAvailability.DayOfWeek.ToString();
        }

        public string From { get; set; }

        public string To { get; set; }

        public string DayOfWeek { get; set; }
    }
}