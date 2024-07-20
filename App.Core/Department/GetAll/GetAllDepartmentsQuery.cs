using App.Infrastructure.Common;
using MediatR;

namespace App.Application.Departments.GetAll
{
    public class GetAllDepartmentsQuery : IRequest<Result<List<DepartmentDto>>>
    {
    }
}
