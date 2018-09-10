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
            var slnDir = GetParentDir(AppDomain.CurrentDomain.BaseDirectory, 5); // 5 -> slndir/projdir/bin/debug/netxx

            #if DEBUG
            args = new[]
            {
                @"E:\Sources\GIT\DPS.Potjes\DEV\DPS.Potjes.Tests\coverage.info",
                slnDir
            };
            #endif

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: dotnet Thumnet.CoverageReport.ConsoleApp.dll <inputFile> <outputDir>");
                return;
            }

            var inputFile = args[0];
            var outputDir = args[1];

            Console.WriteLine(args.Length);

            var parser = new LcovParser(inputFile);
            Func<string, string> compressMethod = LzString.CompressToBase64;

            var items = parser.ReadItems().ToList();
            var sourceFiles = items
                .Select(i => i.File)
                .ToDictionary(k => k, v => compressMethod(File.ReadAllText(v)));

            var lcovSource = compressMethod(string.Join(Environment.NewLine, parser.TextLines));
            var inlineTemplate = new HtmlInlineTemplate(lcovSource, sourceFiles);
            var generated = inlineTemplate.TransformText();

            var resourceReader = new ResourceReader(typeof(IReplacer).Assembly);
            var inliner = new HtmlResourceInliner(new IReplacer[] {
                new JsSrcReplacer(resourceReader),
                new StyleLinkReplacer(resourceReader)
            });
            var inlined = inliner.Replace(generated);

            var outputFile = Path.Combine(outputDir, "report.html");
            Console.WriteLine($"Writing report to: {outputFile}");
            File.WriteAllText(outputFile, inlined);
        }

        static string GetParentDir(string path, int up)
        {
            var parent = path;
            for (int i = 0; i < up; i++)
            {
                parent = Path.GetDirectoryName(parent);
            }

            return parent;
        }
    }
}
