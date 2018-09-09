using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Thumnet.CoverageReport.Core.Replacers {
    public class StyleLinkReplacer : IReplacer {
        private readonly string _htmlPath;
        public StyleLinkReplacer (string htmlPath) {
            _htmlPath = htmlPath;
        }

        public void ReplaceNodes (HtmlNode docNode) {
            var nodes = docNode.SelectNodes ("//link[@rel='stylesheet']/@href").ToList();
            foreach (var node in nodes) {
                var path = node.Attributes["href"].Value;
                var source = File.ReadAllText (Path.Combine (_htmlPath, path));
                var newNodeHtml = $"<style type=\"text/css\">/*href={path}*/{Environment.NewLine}{source}</style>";
                var replacement = HtmlNode.CreateNode(newNodeHtml);
                node.ParentNode.ReplaceChild(replacement, node);
            }
        }
    }
}