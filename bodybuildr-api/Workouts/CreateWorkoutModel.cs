using System;
using System.ComponentModel.DataAnnotations;

namespace BodyBuildr.Api.Controllers
{
    public class CreateWorkoutModel
    {
        [Required]
        public DateTimeOffset? StartDateTime { get; set; }
    }
}