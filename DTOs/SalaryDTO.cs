namespace EFCore_1.DTOs
{
    public class SalaryDTO
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal SalaryAmount { get; set; }
    }
}
