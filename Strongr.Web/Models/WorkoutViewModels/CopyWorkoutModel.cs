using System;
using System.ComponentModel.DataAnnotations;

namespace Strongr.Web.Models.WorkoutViewModels
{
  public class CopyWorkoutModel
  {
    [Required]
    public DateTimeOffset? CurrentDateTime { get; set; }

    public string TimeZoneName { get; set; }

    public Guid? WorkoutId { get; set; }
  }
}
