using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayrollParser.Models;
using PayrollParser.Services;

namespace PayrollParser.Controllers
{
    [Route("Api/Payroll")]
    public class PayrollController : Controller
    {
        [Route("Upload")]
        [HttpPost]
        public async Task<IActionResult> UploadPdf()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
                return BadRequest(new {Error = "You must upload a file."});
            var file = files[0];
            var service = new PayrollParseService();
            try
            {
                var employees = service.ParseToEmployees(file.OpenReadStream());
                return Ok(new Payroll
                {
                    Employees = employees,
                    TotalNetPay = employees.Sum(w => w.NetPay)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }        
        }

        [Route("ExportFile")]
        [HttpPost]
        public async Task<IActionResult> ExportFile([FromBody] MoneyExportRequest request)
        {
            try
            {
                var exporter = new MoneyExportService();
                return Ok(exporter.ExportFile(request));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}