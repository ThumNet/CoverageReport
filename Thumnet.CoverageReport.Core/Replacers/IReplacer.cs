using System.Collections.Generic;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers
{
    public interface IReplacer
    {
        void ReplaceNodes(HtmlNode docNode);
    }
}