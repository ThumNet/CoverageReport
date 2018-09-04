namespace Thumnet.CoverageReport.Core.Models
{
    public class CoverageBranchDetail
    {
        public int Line { get; set; }
        public int Block { get; set; }
        public int Branch { get; set; }
        public int Taken { get; set; }
    }
}