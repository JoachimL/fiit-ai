using Microsoft.AspNetCore.Mvc;
using StrongR.ReadStack.TableStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Exercises
{
    public class ExercisesController : Controller
    {
        private readonly ExerciseRepository exerciseRepository;

        public ExercisesController(ExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        [HttpGet]
        [Route("api/exercises")]
        public async Task<IEnumerable<ExerciseApiModel>> GetExercises()
        {
            var exercises = await exerciseRepository.GetAllExercisesAsync();
            return exercises.Select(e => new ExerciseApiModel
            {
                Id = e.Id,
                Name = e.Name,
                Image = e.Image_1
            });
        }
    }
}
