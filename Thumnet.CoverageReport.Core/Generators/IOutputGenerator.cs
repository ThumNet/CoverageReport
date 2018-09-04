using System;
using System.Collections.Generic;
using System.Text;
using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.Core.Generators
{
    interface IOutputGenerator
    {
        string GenerateOutput(IEnumerable<CoverageItem> items);
    }
}
