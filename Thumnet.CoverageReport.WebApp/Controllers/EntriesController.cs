using Microsoft.AspNetCore.Mvc;
using Thumnet.CoverageReport.Core.Interfaces;
using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public EntriesController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ReportInput report)
        {
            _reportRepository.AddCoverageReport(report);
        }
    }
}
