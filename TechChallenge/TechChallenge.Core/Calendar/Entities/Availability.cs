using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallenge.Core.Calendar.Entities
{
    public class Availability
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string StartTime { get; set; }        
        public string EndTime { get; set; } 
        public DateTime StartDate { get; set; }        
        public DateTime EndDate { get; set; } 
        public DayOfWeek DayOfWeek { get; set; }
        public User User { get; set; }
    }
}