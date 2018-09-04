using System.Collections.Generic;
using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.Core.Parsers
{
    public interface ICoverageParser
    {
        IEnumerable<CoverageItem> ReadItems();
    }
}
