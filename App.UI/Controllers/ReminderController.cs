using App.Application.Reminders.Create;
using App.Application.Reminders.GetAll;
using App.UI.BackgroundJobs;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Controllers
{
    public class ReminderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ReminderController(IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            _mediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        // GET: Reminder
        public async Task<IActionResult> Index()
        {
            var reminders = await _mediator.Send(new GetRemindersQuery());
            return View(reminders.Value);
        }

        // GET: Reminder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reminder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReminderCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    _backgroundJobClient.Schedule<ReminderJob>(job => job.SendReminderEmail(command), command.ReminderDateTime);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
            }
            return View(command);
        }
    }
}
