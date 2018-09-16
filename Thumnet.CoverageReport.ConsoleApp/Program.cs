using Coverlet.Core;
using Coverlet.Core.Reporters;
using LZString;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Thumnet.CoverageReport.ConsoleApp.Api;
using Thumnet.CoverageReport.Core.Generators;
using Thumnet.CoverageReport.Core.Parsers;
using Thumnet.CoverageReport.Core.Replacers;

namespace Thumnet.CoverageReport.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var assemblyDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var slnDir = assemblyDir.GetParentDirectory(4);
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

            TestCoverage();
            
            return;

            var inputFile = args[0];
            var outputDir = args[1];

            Console.WriteLine(args.Length);
            var lcovSource = File.ReadAllText(inputFile);
            var parser = new LcovParser(lcovSource);
            var items = parser.ReadItems().ToList();
            var inlineReport = GenerateInlineReport(lcovSource, items.Select(i => i.File));

            var outputFile = Path.Combine(outputDir, "report.html");
            Console.WriteLine($"Writing report to: {outputFile}");
            File.WriteAllText(outputFile, inlineReport);
        }

        


        private static void TestCoverage()
        {
            var assemblyDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var slnDir = assemblyDir.GetParentDirectory(4);

            var module = Path.Combine(slnDir, @"Thumnet.CoverageReport.Test\bin\Debug\netcoreapp2.1\Thumnet.CoverageReport.Test.dll");
            var excludeFilters = new string[] { };
            var includeFilters = new string[] { };
            var excludedSourceFiles = new string[] { };
            string mergeWith = "";
            var coverage = new Coverage(module, excludeFilters, includeFilters, excludedSourceFiles, mergeWith);
            coverage.PrepareModules();

            var projectPath = Path.Combine(slnDir, @"Thumnet.CoverageReport.Test\Thumnet.CoverageReport.Test.csproj");
            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"test {projectPath} --no-build";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var result = coverage.GetCoverageResult();

            var reporter = new ReporterFactory("lcov").CreateReporter();
            var lcovReport = reporter.Report(result);

            var sourceFilePaths = result.Modules.SelectMany(m => m.Value.Keys);

            var inlineReport = GenerateInlineReport(lcovReport, sourceFilePaths);
            File.WriteAllText(Path.Combine(slnDir, @"report.html"), inlineReport);

            PostToApi(lcovReport, sourceFilePaths);
        }

        private static string GenerateInlineReport(string lcovReport, IEnumerable<string> sourceFilePaths)
        {
            Func<string, string> compressMethod = LzString.CompressToBase64;

            var sourceFiles = sourceFilePaths
                .ToDictionary(k => k, v => compressMethod(File.ReadAllText(v)));
            var lcovSource = compressMethod(string.Join(Environment.NewLine, lcovReport));
            var inlineTemplate = new HtmlInlineTemplate(lcovSource, sourceFiles);
            var generated = inlineTemplate.TransformText();

            var resourceReader = new ResourceReader(typeof(IReplacer).Assembly);
            var inliner = new HtmlResourceInliner(new IReplacer[] {
                new JsSrcReplacer(resourceReader),
                new StyleLinkReplacer(resourceReader)
            });
            var inlined = inliner.Replace(generated);

            return inlined;
        }

        private static void PostToApi(string lcovReport, IEnumerable<string> sourceFilePaths)
        {
            var projectName = "Thumnet.CoverageReport";
            var projectUrl = "https://github.com/ThumNet/CoverageReport/";

            var client = new EntriesClient();
            var model = new ReportInput
            {
                ProjectName = projectName,
                ProjectUrl = projectUrl,
                LcovData = LzString.CompressToUint8Array(lcovReport),
                SourceFilesData = sourceFilePaths
                    .ToDictionary(k => k, v => LzString.CompressToUint8Array(File.ReadAllText(v)))
            };
            client.PostAsync(model).GetAwaiter().GetResult();
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
