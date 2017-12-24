using System;
using System.ComponentModel.DataAnnotations;

namespace Strongr.Web.Models.WorkoutViewModels
{
  public class CopyWorkoutModel
  {
    [Required]
    public DateTimeOffset? CurrentDateTime { get; set; }

    [Required]
    public string TimeZoneName { get; set; }

    [Required]
    public Guid? WorkoutId { get; set; }
  }
}
