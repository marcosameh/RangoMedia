using App.Domain.Models;
using App.Infrastructure.Common;
using App.Infrastructure.Helpers;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace App.Application.Departments.Create
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<int>>
    {
        private readonly RangoMediaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateDepartmentCommandHandler(RangoMediaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<int>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string logoFileName = null;
                if (request.Logo != null)
                {
                    logoFileName = await PhotoUploader.SaveFileAsync(request.Logo, _webHostEnvironment.WebRootPath, "photos/logo", 100, 100);
                }

                var department = new Department
                {
                    Name = request.Name,
                    Logo = logoFileName,
                    ParentDepartmentId = request.ParentDepartmentId
                };

                _context.Departments.Add(department);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<int>.Ok(department.Id, "Department created successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex, "Failed to create department.");
            }
        }
    }
}
