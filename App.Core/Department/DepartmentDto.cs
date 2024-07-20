namespace App.Application.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string LogoPath => "/photos/logo/" + Logo;
        public int? ParentDepartmentId { get; set; }
        public string ParentDepartmentName { get; set; }
    }
}
