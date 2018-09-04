
namespace Thumnet.CoverageReport.Core.Models
{
    public class CoverageItem
    {
        public string File { get; set; }
        public string Title { get; set; }

        public CoveragePart<CoverageLineDetail> Lines { get; set; } = new CoveragePart<CoverageLineDetail>();
        public CoveragePart<CoverageFunctionDetail> Functions { get; set; } = new CoveragePart<CoverageFunctionDetail>();
        public CoveragePart<CoverageBranchDetail> Branches { get; set; } = new CoveragePart<CoverageBranchDetail>();
    }
}
