using App.Application.Departments.Create;
using App.Application.Departments.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.UI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var departments = await _mediator.Send(new GetAllDepartmentsQuery());
            return View(departments.Value);
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartmentController/Create
        public async Task<ActionResult> Create()
        {
            var departments = await _mediator.Send(new GetAllDepartmentsQuery());
            ViewBag.Departments = new SelectList(departments.Value, "Id", "Name");
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDepartmentCommand command)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _mediator.Send(new GetAllDepartmentsQuery());
                ViewBag.Departments = new SelectList(departments.Value, "Id", "Name");
                return View(command);
            }

            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
            var allDepartments = await _mediator.Send(new GetAllDepartmentsQuery());
            ViewBag.Departments = new SelectList(allDepartments.Value, "Id", "Name");
            return View(command);
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
