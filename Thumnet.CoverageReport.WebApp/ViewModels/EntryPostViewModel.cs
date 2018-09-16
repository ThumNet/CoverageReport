using System.Collections.Generic;

namespace Thumnet.CoverageReport.WebApp.ViewModels
{
    public class EntryPostViewModel
    {
        public string ProjectName { get; set; }
        public string ProjectUrl { get; set; }
        public string BranchName { get; set; }
        public byte[] LcovData { get; set; }
        public Dictionary<string, byte[]> SourceFilesData { get; set; }
    }
}
