using EFCore_1.Models;

namespace EFCore_1.DTOs
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
