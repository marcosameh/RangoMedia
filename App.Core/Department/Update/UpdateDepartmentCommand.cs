using App.Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace App.Application.Departments.Update
{
    public class UpdateDepartmentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Logo { get; set; }
        public string? LogoPath { get; set; }
        public int? ParentDepartmentId { get; set; }
    }
}
