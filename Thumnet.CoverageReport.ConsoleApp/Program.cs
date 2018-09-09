using LZString;
using System;
using System.IO;
using System.Linq;
using Thumnet.CoverageReport.Core.Generators;
using Thumnet.CoverageReport.Core.Parsers;
using Thumnet.CoverageReport.Core.Replacers;

namespace Thumnet.CoverageReport.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // var lcovPath = @"./coverlet.lcov"; // @"E:\Sources\GIT\DPS.Potjes\DEV\DPS.Potjes.Tests\coverage.info";
            // var parser = new LcovParser(lcovPath);

            // Func<string, string> compressMethod = LzString.CompressToBase64;//LzString.CompressToUtf16;
            
            // var items = parser.ReadItems().ToList();
            // var sourceFiles = items
            //     .Select(i => i.File)
            //     .ToDictionary(k => k, v => compressMethod(File.ReadAllText(v)));

            // var lcovSource = compressMethod(string.Join(Environment.NewLine, parser.TextLines));
            // var inlineTemplate = new HtmlInlineTemplate(lcovSource, sourceFiles);
            // var generated = inlineTemplate.TransformText();
            
            var rootPath = @"/Users/jeffreytummers/Sources/CoverageReport/";
            var generated = File.ReadAllText(Path.Combine(rootPath, "generated-sample.html"));

            var inliner = new HtmlResourceInliner(new IReplacer[] { 
                new JsSrcReplacer(rootPath),
                new StyleLinkReplacer(rootPath)
            });
            var inlined = inliner.Replace(generated);
            
            File.WriteAllText(Path.Combine(rootPath, "generated.html"), inlined);

            // Console.WriteLine(items.Count);

        }
    }
}
