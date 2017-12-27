using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Models.WorkoutViewModels
{
    public class DeleteViewModel
    {
        public Guid WorkoutId { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
    }
}
