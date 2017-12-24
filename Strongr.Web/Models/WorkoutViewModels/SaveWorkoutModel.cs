using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Models.WorkoutViewModels
{
    public class SaveWorkoutModel
    {
        [Required]
        public Guid? WorkoutId { get; set; }

        [Required]
        public DateTimeOffset? StartDateTime { get; set; }

        [Required]
        public int? Version { get; set; }
    }
}
