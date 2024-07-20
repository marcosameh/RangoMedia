using App.Domain.Models;
using App.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Departments.Get
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
    {
        private readonly RangoMediaContext _context;

        public GetDepartmentByIdQueryHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _context.Departments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

                if (department == null)
                {
                    return Result<DepartmentDto>.Fail("Department not found.");
                }

                var departmentDto = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Logo = department.Logo,
                    ParentDepartmentId = department.ParentDepartmentId
                };

                return Result<DepartmentDto>.Ok(departmentDto);
            }
            catch (Exception ex)
            {
                return Result<DepartmentDto>.Fail(ex, "Failed to get department.");
            }
        }
    }
}
