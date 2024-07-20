using App.Domain.Models;
using App.Infrastructure.Common;
using App.Infrastructure.Helpers;
using MediatR;

namespace App.Application.Departments.Update
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result<int>>
    {
        private readonly RangoMediaContext _context;

        public UpdateDepartmentCommandHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken);

                if (department == null)
                {
                    return Result<int>.Fail("Department not found.");
                }

                department.Name = request.Name;
                department.ParentDepartmentId = request.ParentDepartmentId;

                if (request.Logo != null)
                {
                    var logoPath = Path.Combine("wwwroot", "photos", "logo");
                    var fileName = await PhotoUploader.SaveFileAsync(request.Logo, "wwwroot", "photos/logo");

                    department.Logo = fileName;
                }

                _context.Departments.Update(department);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<int>.Ok(department.Id, "Department updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex, "Failed to update department.");
            }
        }
    }
}
