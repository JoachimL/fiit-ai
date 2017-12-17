using Bodybuildr.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Strongr.Web.Models;
using Strongr.Web.Models.WorkoutViewModels;
using StrongR.ReadStack.WorkoutDetail;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Strongr.Web.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public WorkoutController(
            UserManager<ApplicationUser> userManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var localTime = LocalDateTime.FromDateTime(model.StartDateTime.Value.DateTime);
            var zonedTime = localTime.InZoneStrictly(DateTimeZoneProviders.Tzdb[model.TimeZoneName]);
            
            var workoutId = Guid.NewGuid();
            await _mediator.Send(
                new StartWorkout(
                    workoutId,
                    await GetUserId(),
                    zonedTime.ToDateTimeOffset()));
            return RedirectToAction("Detail", new { id = workoutId });
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            return userId;
        }

        public async Task<IActionResult> Detail([Required]Guid? workoutId)
        {
            var workout = await _mediator.Send(
                new WorkoutDetailRequest(workoutId.Value, await GetUserId()));
            return View(workout);
        }
    }
}
