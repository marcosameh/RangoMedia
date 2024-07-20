using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Application.Departments.Update
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

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

    }
}
