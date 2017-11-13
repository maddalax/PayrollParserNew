using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PayrollParser.Models;
using PayrollParser.Services;

namespace PayrollParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            BuildWebHost(args).Run();
            
            /*
            var service = new PayrollParseService();
            var employees = service.ParseToEmployees("/Users/maddev/Downloads/payoll.pdf");
            var exportService = new MoneyExportService();
            var file = exportService.ExportFile(new MoneyExportRequest
            {
                Employees = employees,
                Date = 1510517593789
            });
            Console.WriteLine(file);
            */
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();
    }
}