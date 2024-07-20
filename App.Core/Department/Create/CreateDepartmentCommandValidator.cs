using App.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace App.Application.Departments.Create
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        private readonly RangoMediaContext _context;

        public CreateDepartmentCommandValidator(RangoMediaContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MustAsync(BeAUniqueDepartmentName).WithMessage("Department name already exists.");

            RuleFor(x => x.Logo)
                .NotEmpty().WithMessage("Department logo is required.")
                 .Must(BeAValidImageFile).WithMessage("Logo must be a valid image file.");

            RuleFor(x => x.ParentDepartmentId)
                .MustAsync(BeAValidParentDepartment).WithMessage("Invalid parent department.");
        }

        private async Task<bool> BeAValidParentDepartment(int? parentDepartmentId, CancellationToken cancellationToken)
        {
            if (parentDepartmentId == null)
            {
                return true;
            }

            var parentExists = await _context.Departments.AnyAsync(d => d.Id == parentDepartmentId, cancellationToken);
            return parentExists;
        }

        private async Task<bool> BeAUniqueDepartmentName(string departmentName, CancellationToken cancellationToken)
        {
            var nameExists = await _context.Departments.AnyAsync(d => d.Name == departmentName, cancellationToken);
            return !nameExists;
        }

        private bool BeAValidImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}
