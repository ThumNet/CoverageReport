using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers {
    public class JsSrcReplacer : IReplacer {
        private readonly string _htmlPath;
        public JsSrcReplacer (string htmlPath) {
            _htmlPath = htmlPath;
        }

        public void ReplaceNodes (HtmlNode docNode) {
            var nodes = docNode.SelectNodes ("//script/@src").ToList();
            foreach (var node in nodes) {
                var path = node.Attributes["src"].Value;
                var scriptSource = File.ReadAllText (Path.Combine (_htmlPath, path));
                var newNodeHtml = $"<script type=\"text/javascript\">//src={path}{Environment.NewLine}{scriptSource}</script>";
                var replacement = HtmlNode.CreateNode(newNodeHtml);
                node.ParentNode.ReplaceChild(replacement, node);
            }
        }
    }
}