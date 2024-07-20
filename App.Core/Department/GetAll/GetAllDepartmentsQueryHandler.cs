using App.Domain.Models;
using App.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Departments.GetAll
{
    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, Result<List<DepartmentDto>>>
    {
        private readonly RangoMediaContext _context;

        public GetAllDepartmentsQueryHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _context.Departments
                    .Include(d => d.ParentDepartment)
                    .Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Logo = d.Logo,
                        ParentDepartmentId = d.ParentDepartmentId,
                        ParentDepartmentName = d.ParentDepartment.Name
                    }).AsNoTracking().ToListAsync(cancellationToken);

                return Result<List<DepartmentDto>>.Ok(departments);
            }
            catch (Exception ex)
            {
                return Result<List<DepartmentDto>>.Fail(ex, "Failed to retrieve departments.");
            }
        }
    }
}
