using App.Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace App.Application.Departments.Create
{
    public class CreateDepartmentCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public IFormFile Logo { get; set; }

        [DisplayName("Parent Department")]
        public int? ParentDepartmentId { get; set; }
    }
}
