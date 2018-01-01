using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Strongr.Web.Models;
using Strongr.Web.Workouts;
using StrongR.ReadStack.Workouts.WorkoutsForUser;

namespace Strongr.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private WorkoutsOrchestrator _workoutsOrchestrator;

        public HomeController(IMediator mediator,
            UserManager<ApplicationUser> userManager,
            WorkoutsOrchestrator workoutsOrchestrator
            )
        {
            _mediator = mediator;
            _userManager = userManager;
            _workoutsOrchestrator = workoutsOrchestrator;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var response = await _workoutsOrchestrator.GetWorkoutsForUser(User);
            return View(response.Workouts);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
