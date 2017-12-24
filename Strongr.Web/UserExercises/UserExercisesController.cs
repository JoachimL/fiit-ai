using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using Strongr.Web.Models.WorkoutViewModels;
using StrongR.ReadStack.TableStorage;
using StrongR.ReadStack.Workouts.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.UserExercises
{
    [Route("api/users/{userId}/exercises/{exerciseId}/last")]
    public class UserExercisesController : Controller
    {
        private readonly LastExerciseActivityTableHandler _tableHandler;

        public UserExercisesController(LastExerciseActivityTableHandler tableHandler)
        {
            _tableHandler = tableHandler;
        }

        public async Task<Activity> GetLastActivityForExercise(string userId, string exerciseId)
        {
            var a = await _tableHandler.Retrieve(userId, exerciseId);
            if (a == null)
                return new Activity();
            else
                return new Activity
                {
                    Sets = a.Sets.Select(s => new Set { Repetitions = s.Repetitions, Weight = s.Weight }).ToArray()
                };
        }
    }
}
