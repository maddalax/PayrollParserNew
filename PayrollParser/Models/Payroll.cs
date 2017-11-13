using System.Collections.Generic;

namespace PayrollParser.Models
{
    public class Payroll
    {
        public IList<Employee> Employees { get; set; }
        public decimal TotalNetPay { get; set; }
    }
}