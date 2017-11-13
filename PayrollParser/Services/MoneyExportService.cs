using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PayrollParser.Models;

namespace PayrollParser.Services
{
    public class MoneyExportService
    {
        public string ExportFile(MoneyExportRequest request)
        {
            var transactions = ParseToTransactions(request);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = path + $"/ExportedPayroll-{request.DateFormatted}.qif";
            var builder = new StringBuilder();
            builder.Append("!Type:Bank").AppendLine();
            for (var i = 0; i < transactions.Count; i++)
            {
                var t = transactions[i];
                builder.Append(t.ToMoneyFormat(i == 0));
            }
            File.WriteAllText(file, builder.ToString());
            return file;
        }

        private List<Transaction> ParseToTransactions(MoneyExportRequest request)
        {
            return request.Employees.Select(w => new Transaction
            {
                Category = "Cost of Goods:Labor",
                Vendor = "Payroll",
                Date = request.Date,
                Number = w.CheckNumber,
                Total = w.NetPay
            }).ToList();
        }
    }
}