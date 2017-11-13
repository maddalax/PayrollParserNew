using System.Collections.Generic;
using PayrollParser.Util;

namespace PayrollParser.Models
{
    public class MoneyExportRequest
    {
        public long Date { get; set; }
        
        public string DateFormatted => 
            DateUtil.GetDateTime(Date).ToShortDateString().Replace("/", "-");
        
        public IList<Employee> Employees { get; set; }

        public MoneyExportRequest()
        {
            Employees = new List<Employee>();
        }
    }
}