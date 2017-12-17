using System;
using System.ComponentModel.DataAnnotations;

namespace Strongr.Web.Models.WorkoutViewModels
{
    public class CreateViewModel
    {
        [Required]
        public DateTimeOffset? StartDateTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TimeZoneName { get; set; } = "Europe/Oslo";
    }
}
