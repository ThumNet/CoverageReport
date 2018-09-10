using System;
using System.Linq;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers
{
    public class JsSrcReplacer : IReplacer {
        private readonly ResourceReader _resourceReader;
        private const string EmbeddedSourcePath = "Thumnet.CoverageReport.Core";

        public JsSrcReplacer (ResourceReader resourceReader) {
            _resourceReader = resourceReader;
        }

        public void ReplaceNodes (HtmlNode docNode) {
            var nodes = docNode.SelectNodes ("//script/@src").ToList();
            foreach (var node in nodes) {
                var path = node.Attributes["src"].Value;
                if (!path.StartsWith(EmbeddedSourcePath))
                {
                    continue;
                }

                var resourceName = path.Replace('/', '.');
                var source = _resourceReader.ReadResource(resourceName);
                var newNodeHtml = $"<script type=\"text/javascript\">//src={path}{Environment.NewLine}{source}</script>";
                var replacement = HtmlNode.CreateNode(newNodeHtml);
                node.ParentNode.ReplaceChild(replacement, node);
            }
        }
    }
}