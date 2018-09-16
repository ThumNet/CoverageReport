using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.Core.Interfaces
{
    public interface IReportRepository
    {
        ReportOutput GetLatest(string projectName);
        void AddCoverageReport(ReportInput report);
    }
}
