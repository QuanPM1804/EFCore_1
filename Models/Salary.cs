namespace EFCore_1.Models
{
    public class Salary
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal SalaryAmount { get; set; }
    }
}
