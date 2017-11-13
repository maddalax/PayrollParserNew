using System;
using System.Text;
using PayrollParser.Util;

namespace PayrollParser.Models
{
    public class Transaction
    {
        public long Date { get; set; }
        public decimal Total { get; set; }
        public string Vendor { get; set; }
        public decimal Number { get; set; }
        public string Category { get; set; }
        public string Meta { get; set; }

        public string ToMoneyFormat(bool disableCarrot)
        {
            var builder = new StringBuilder();
            if(!disableCarrot)
                builder.Append("^").Append("\n");
            builder.Append("D").Append(ParseDate()).Append("\n");
            builder.Append("T").Append("-").Append(Total).Append("\n");
            builder.Append("N").Append(Number).Append("\n");
            builder.Append("P").Append(Vendor).Append("\n");
            builder.Append("L").Append(Category).Append("\n");
            return builder.ToString();
        }

        private string ParseDate() => DateUtil.SimpleDate(Date);
        
    }
}