using System.ComponentModel.DataAnnotations;

namespace EFCore_1.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime JoinedDate { get; set; }
        public Salary Salary { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
