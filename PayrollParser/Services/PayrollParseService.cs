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
                .Where(w => w.Contains('-') && w.Contains('$') && w.Contains('.'))
                .Select(w => Regex.Replace(w, @"\s+", " "))
                .Select(w => new string(w.Where(s => char.IsNumber(s) || char.IsWhiteSpace(s) || s is '.' or ',').ToArray()))
                .Select(w => Regex.Replace(w, @"\s+", " "))
                .Where(w => w.Length > 0)
                .Select(w => w.Trim());

            var split = lines
                .Select(w => w.Split(" "))
                .ToArray();

            var employees = new List<Employee>();
            foreach (var s in split)
            {
                var v = s.Where(c => !c.Equals(".") && !c.Equals(",")).ToArray();
                try
                {
                    var id = v[0];
                    var gross = v[3];
                    var deductions = v[4];
                    var net = v[5];
                    var checkNumber = v[6];

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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return employees;
        }
    }
}