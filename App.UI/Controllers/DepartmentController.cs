using App.Application.Departments.Create;
using App.Application.Departments.Get;
using App.Application.Departments.GetAll;
using App.Application.Departments.Update;
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

        private async Task<SelectList> GetDepartmentsSelectList(int? excludeId = null)
        {
            var departmentsResult = await _mediator.Send(new GetAllDepartmentsQuery());
            if (excludeId.HasValue)
            {
                var currentDepartment = departmentsResult.Value.Find(x => x.Id == excludeId);
                departmentsResult.Value.Remove(currentDepartment);
            }
            return new SelectList(departmentsResult.Value, "Id", "Name");
        }

        private void AddModelErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        // GET: DepartmentController
        public async Task<ActionResult> Index()
        {
            var departments = await _mediator.Send(new GetAllDepartmentsQuery());
            return View(departments.Value);
        }

        // GET: DepartmentController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Departments = await GetDepartmentsSelectList();
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDepartmentCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await GetDepartmentsSelectList();
                return View(command);
            }

            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(result.Errors);
            ViewBag.Departments = await GetDepartmentsSelectList();
            return View(command);
        }

        // GET: DepartmentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var departmentResult = await _mediator.Send(new GetDepartmentByIdQuery(id));

            if (!departmentResult.Success)
            {
                return NotFound();
            }

            ViewBag.Departments = await GetDepartmentsSelectList(id);

            var updateDepartmentCommand = new UpdateDepartmentCommand
            {
                Id = departmentResult.Value.Id,
                Name = departmentResult.Value.Name,
                LogoPath = departmentResult.Value.LogoPath,
                ParentDepartmentId = departmentResult.Value.ParentDepartmentId
            };

            return View(updateDepartmentCommand);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateDepartmentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await GetDepartmentsSelectList(id);
                return View(command);
            }

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            AddModelErrors(result.Errors);
            ViewBag.Departments = await GetDepartmentsSelectList(id);
            return View(command);
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
