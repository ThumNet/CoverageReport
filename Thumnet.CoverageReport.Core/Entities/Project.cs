using System.Collections.Generic;

namespace Thumnet.CoverageReport.Core.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<CoverageEntry> Entries { get; set; }
    }
}
