using System;
using System.Linq;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers
{
    public class StyleLinkReplacer : IReplacer {
        private readonly ResourceReader _resourceReader;
        private const string EmbeddedSourcePath = "Thumnet.CoverageReport.Core";

        public StyleLinkReplacer (ResourceReader resourceReader) {
            _resourceReader = resourceReader;
        }

        public void ReplaceNodes (HtmlNode docNode) {
            var nodes = docNode.SelectNodes ("//link[@rel='stylesheet']/@href").ToList();
            foreach (var node in nodes) {
                var path = node.Attributes["href"].Value;
                if (!path.StartsWith(EmbeddedSourcePath))
                {
                    continue;
                }

                var resourceName = path.Replace('/', '.');
                var source = _resourceReader.ReadResource(resourceName);
                var newNodeHtml = $"<style type=\"text/css\">/*href={path}*/{Environment.NewLine}{source}</style>";
                var replacement = HtmlNode.CreateNode(newNodeHtml);
                node.ParentNode.ReplaceChild(replacement, node);
            }
        }
    }
}