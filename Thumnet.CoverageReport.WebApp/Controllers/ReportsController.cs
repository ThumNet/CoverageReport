using LZString;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Thumnet.CoverageReport.Data;
using Thumnet.CoverageReport.WebApp.Models;

namespace Thumnet.CoverageReport.WebApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly CoverageContext _context;

        public ReportsController(CoverageContext context)
        {
            _context = context;
        }

        public IActionResult Latest(string id)
        {
            var entry = _context.Entries
                .Include(e => e.Project)
                .Include(e => e.SourceFiles)
                .Where(e => e.Project.Name == id)
                .OrderByDescending(e => e.Created)
                .FirstOrDefault();

            var model = new ReportLatestViewModel
            {
                Created = entry.Created,
                ProjectName = entry.Project.Name,
                ProjectUrl = entry.Project.Url,
                LcovSource = LzString.CompressToBase64(LzString.DecompressFromUint8Array(entry.LcovData)),
                SourceFiles = entry.SourceFiles.ToDictionary(
                    k => k.FilePath,
                    v => LzString.CompressToBase64(LzString.DecompressFromUint8Array(v.FileData)))
            };

            return View(model);
        }
    }
}