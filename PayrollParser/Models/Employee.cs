namespace PayrollParser.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public long CheckNumber { get; set; }
    }
}