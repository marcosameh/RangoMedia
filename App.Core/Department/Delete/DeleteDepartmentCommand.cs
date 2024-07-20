using App.Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Departments.Delete
{
    public class DeleteDepartmentCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
