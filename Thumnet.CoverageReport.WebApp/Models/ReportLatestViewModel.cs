using System;
using System.Collections.Generic;

namespace Thumnet.CoverageReport.WebApp.Models
{
    public class ReportLatestViewModel
    {
        public DateTime Created { get; set; }
        public string ProjectName { get; set; }
        public string ProjectUrl { get; set; }
        public string LcovSource { get; set; }
        public Dictionary<string, string> SourceFiles { get; set; }
    }
}