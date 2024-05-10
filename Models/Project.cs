using System.ComponentModel.DataAnnotations;

namespace EFCore_1.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    }
}
