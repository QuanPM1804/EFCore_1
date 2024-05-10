using System.ComponentModel.DataAnnotations;

namespace EFCore_1.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
