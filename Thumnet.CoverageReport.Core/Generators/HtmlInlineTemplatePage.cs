using System;
using System.Collections.Generic;
using System.Text;

namespace Thumnet.CoverageReport.Core.Generators
{
    public partial class HtmlInlineTemplate
    {
        public HtmlInlineTemplate(string lcovSource, Dictionary<string,string> sourceFiles)
        {
            LcovSource = lcovSource;
            SourceFiles = sourceFiles;
        }

        public string LcovSource { get; }
        public Dictionary<string, string> SourceFiles { get; }
    }
}
