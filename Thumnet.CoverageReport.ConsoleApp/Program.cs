using LZString;
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

            var parser = new LcovParser(@"E:\Sources\GIT\DPS.Potjes\DEV\DPS.Potjes.Tests\coverage.info");

            Func<string, string> compressMethod = LzString.CompressToBase64;//LzString.CompressToUtf16;
            
            var items = parser.ReadItems().ToList();
            var sourceFiles = items
                .Select(i => i.File)
                .ToDictionary(k => k, v => compressMethod(File.ReadAllText(v)));

            //var source = File.ReadAllText(items[0].File);
            //Console.WriteLine(source);
            //var compressed = LzString.CompressToUtf16(source);
            //Console.WriteLine();
            //Console.WriteLine(compressed);
            //var decompressed = LzString.DecompressFromUtf16(compressed);

            //Console.ReadLine();
            var lcovSource = compressMethod(string.Join(Environment.NewLine, parser.TextLines));
            var inlineTemplate = new HtmlInlineTemplate(lcovSource, sourceFiles);
            var generated = inlineTemplate.TransformText();
            File.WriteAllText(@".\generated.html", generated);

            Console.WriteLine(items.Count);

        }
    }
}
