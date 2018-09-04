using System.Collections.Generic;

namespace Thumnet.CoverageReport.Core.Models
{
    public class CoveragePart<T>
    {
        public int Hit { get; set; }
        public int Found { get; set; }
        public List<T> Details { get; set; } = new List<T>();
    }
}