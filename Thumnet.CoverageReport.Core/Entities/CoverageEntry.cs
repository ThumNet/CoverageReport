using System;
using System.Collections.Generic;

namespace Thumnet.CoverageReport.Core.Entities
{
    public class CoverageEntry
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime Created { get; set; }
        public string BranchName { get; set; }
        public string BuildId { get; set; }

        public byte[] LcovData { get; set; }
        public List<SourceFileEntry> SourceFiles { get; set; } = new List<SourceFileEntry>();
    }
}
