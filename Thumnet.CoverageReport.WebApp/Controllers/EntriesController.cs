using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Thumnet.CoverageReport.Core.Entities;
using Thumnet.CoverageReport.Data;
using Thumnet.CoverageReport.WebApp.ViewModels;

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

        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

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
                Branchname = model.BranchName,
                LcovData = model.LcovData,
                Project = project,
                SourceFiles = model.SourceFilesData.Select(s => new SourceFileEntry { FilePath = s.Key, FileData = s.Value }).ToList()
            };
            _coverageContext.Entries.Add(entry);
            _coverageContext.SaveChanges();
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
