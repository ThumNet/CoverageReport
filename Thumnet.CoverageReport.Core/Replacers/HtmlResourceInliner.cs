using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers
{
    public class HtmlResourceInliner
    {
        private readonly IEnumerable<IReplacer> _replacers;

        public HtmlResourceInliner(IEnumerable<IReplacer> replacers)
        {
            _replacers = replacers;
        }

        public string Replace(string input)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(input);
            
            foreach (var replacer in _replacers)
            {
                replacer.ReplaceNodes(doc.DocumentNode);
            } 

            return doc.DocumentNode.OuterHtml;
        }
    }
}