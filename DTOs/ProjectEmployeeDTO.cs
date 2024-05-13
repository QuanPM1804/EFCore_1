namespace EFCore_1.DTOs
{
    public class ProjectEmployeeDTO
    {
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public bool Enabled { get; set; }
    }
}
