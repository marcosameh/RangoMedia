using App.Domain.Models;
using App.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Departments.Delete
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result>
    {
        private readonly RangoMediaContext _context;

        public DeleteDepartmentCommandHandler(RangoMediaContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                                           .Include(d => d.InverseParentDepartment)
                                           .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (department == null)
            {
                return Result.Fail("Department not found.");
            }

            if (department.InverseParentDepartment.Any())
            {
                return Result.Fail("Department cannot be deleted because it has sub-departments.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync(cancellationToken);
            //TODO:remove Photos from Physical Path 
            return Result.Ok("Department deleted successfully.");
        }
    }
}
