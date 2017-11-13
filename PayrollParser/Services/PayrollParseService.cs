using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using PayrollParser.Models;
using PayrollParser.Util;

namespace PayrollParser.Services
{
    public class PayrollParseService
    {
        public IList<Employee> ParseToEmployees(Stream stream)
        {
            var text = GetText(stream);
            return ExtractEmployees(text);
        }

        private string GetText(Stream stream)
        {
            var builder = new StringBuilder();
            var reader = new PdfReader(stream);
            for (var i = 1; i <= reader.NumberOfPages; i++)
                builder.Append(PdfTextExtractor.GetTextFromPage(reader, i,
                    new SimpleTextExtractionStrategy()));
            return builder.ToString();
        }

        private List<Employee> ExtractEmployees(string text)
        {
            var result = Regex.Split(text, @"\r?\n|\r");
            var lines = result.Select(w => w.Trim())
                .Where(w => Regex.IsMatch(w.Substring(0, 4), @"^\d"))
                .Where(w => w.Contains("$"))
                .Select(w => new string(w.Where(s => !char.IsLetter(s)).ToArray()))
                .Select(w => w.Replace(",", "").Replace("'", ""))
                .Select(w => w.Replace("$", ""))
                .Select(w => w.Replace("-", ""))
                .Select(w => Regex.Replace(w, @"\s+", " "))
                .Select(w =>
                {
                    var indexes = StringUtil.FindIndexes(w, ".");
                    foreach (var index in indexes)
                    {
                        var charAfter = w[index + 1];
                        var charBefore = w[index - 1];
                        if (!char.IsNumber(charAfter) || !char.IsNumber(charBefore))
                            w = w.Remove(index, 1);
                    }
                    return w;
                })
                .Select(w => Regex.Replace(w, @"\s+", " "));

            var split = lines
                .Select(w => w.Split(" "));

            var employees = new List<Employee>();
            foreach (var s in split)
            {
                var id = s[0];
                var gross = s[3];
                var deductions = s[4];
                var net = s[5];
                var checkNumber = s[6];

                if (checkNumber.IndexOf(":", StringComparison.Ordinal) != -1)
                    checkNumber = checkNumber.Substring(0, checkNumber.IndexOf(":", StringComparison.Ordinal));

                var employee = new Employee
                {
                    CheckNumber = Convert.ToInt64(checkNumber),
                    Id = Convert.ToInt32(id),
                    GrossPay = Convert.ToDecimal(gross),
                    Deductions = Convert.ToDecimal(deductions),
                    NetPay = Convert.ToDecimal(net)
                };
                employees.Add(employee);
            }
            return employees;
        }
    }
}