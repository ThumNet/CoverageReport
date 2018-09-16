using Microsoft.AspNetCore.Mvc;
using Thumnet.CoverageReport.Core.Interfaces;

namespace Thumnet.CoverageReport.WebApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public IActionResult Latest(string id)
        {
            var model = _reportRepository.GetLatest(id);

            return View(model);
        }
    }
}