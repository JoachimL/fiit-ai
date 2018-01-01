using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Models.Workouts
{
    public class Workout
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartDateAndTime { get; set; }
        public string DisplayStartDateAndTime { get; set; }
    }
}
