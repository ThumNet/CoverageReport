using System;
using System.IO;
using System.Linq;
using Thumnet.CoverageReport.Core;
using Thumnet.CoverageReport.Core.Generators;
using Thumnet.CoverageReport.Core.Parsers;

namespace Thumnet.CoverageReport.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var parser = new LcovParser(@"./coverlet.lcov");
            
            var items = parser.ReadItems().ToList();
            var sourceFiles = items
                .Select(i => i.File)
                .ToDictionary(k => k, v => File.ReadAllText(v));

            var inlineTemplate = new HtmlInlineTemplate(string.Join(Environment.NewLine, parser.TextLines), sourceFiles);
            var generated = inlineTemplate.TransformText();
            File.WriteAllText(@".\generated.html", generated);

            Console.WriteLine(items.Count);

        }
    }
}
