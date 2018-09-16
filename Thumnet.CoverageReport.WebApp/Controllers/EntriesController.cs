using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Thumnet.CoverageReport.Core.Entities;
using Thumnet.CoverageReport.Data;
using Thumnet.CoverageReport.WebApp.Models;

namespace Thumnet.CoverageReport.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly CoverageContext _coverageContext;

        public EntriesController(CoverageContext coverageContext)
        {
            _coverageContext = coverageContext;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] EntryPostViewModel model)
        {
            var project = _coverageContext.Projects.FirstOrDefault(p => p.Name == model.ProjectName);
            if (project == null)
            {
                project = new Project
                {
                    Name = model.ProjectName,
                    Url = model.ProjectUrl
                };
                _coverageContext.Projects.Add(project);
            }

            var entry = new CoverageEntry
            {
                Created = DateTime.Now,
                BranchName = model.BranchName,
                LcovData = model.LcovData,
                Project = project,
                SourceFiles = model.SourceFilesData.Select(s => new SourceFileEntry { FilePath = s.Key, FileData = s.Value }).ToList()
            };
            _coverageContext.Entries.Add(entry);
            _coverageContext.SaveChanges();
        }
    }
}
