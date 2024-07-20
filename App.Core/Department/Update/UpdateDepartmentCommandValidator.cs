using App.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Departments.Update
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        private readonly RangoMediaContext _context;

        public UpdateDepartmentCommandValidator(RangoMediaContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync((command, name, cancellationToken) => BeUniqueName(command.Id, name, cancellationToken))
                .WithMessage("A department with the same name already exists.");

            RuleFor(x => x.Logo)
                .Must(BeAValidImageFile).WithMessage("Logo must be a valid image file.")
                .When(x => x.Logo != null);
        }

        private bool BeAValidImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        private async Task<bool> BeUniqueName(int id, string name, CancellationToken cancellationToken)
        {
            return await _context.Departments
                .Where(d => d.Id != id)
                .AllAsync(d => d.Name != name, cancellationToken);
        }
    }
}
