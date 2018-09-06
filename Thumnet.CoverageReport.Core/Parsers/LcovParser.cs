using System.Collections.Generic;
using System.IO;
using System.Linq;
using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.Core.Parsers
{
    /// <summary>
    /// LCOV file parser
    /// based on the lcov-parse (Javascript) - https://github.com/davglass/lcov-parse/blob/master/lib/index.js
    /// </summary>
    public class LcovParser : ICoverageParser
    {
        private readonly string _filepath;

        public LcovParser(string filepath)
        {
            _filepath = filepath;
        }

        public string[] TextLines { get; private set; }

        public IEnumerable<CoverageItem> ReadItems()
        {
            TextLines = File.ReadAllLines(_filepath);
            var assembly = new CoverageItem();

            foreach (var line in TextLines)
            {
                var allparts = line.Split(':');
                var identifier = allparts[0].ToUpper();
                var value = allparts.Length > 1 ? string.Join(':', allparts.Skip(1)).Trim() : "";
                var parts = value.Split(',');

                switch (identifier)
                {
                    case "TN":
                        assembly.Title = value;
                        break;
                    case "SF":
                        assembly.File = value;
                        break;
                    case "FNF":
                        assembly.Functions.Found = int.Parse(value);
                        break;
                    case "FNH":
                        assembly.Functions.Hit = int.Parse(value);
                        break;
                    case "LF":
                        assembly.Lines.Found = int.Parse(value);
                        break;
                    case "LH":
                        assembly.Lines.Hit = int.Parse(value);
                        break;
                    case "DA":
                        assembly.Lines.Details.Add(new CoverageLineDetail
                        {
                            Line = int.Parse(parts[0]),
                            Hit = int.Parse(parts[1]),
                        });
                        break;
                    case "FN":
                        assembly.Functions.Details.Add(new CoverageFunctionDetail
                        {
                            Line = int.Parse(parts[0]),
                            Name = string.Join(',', parts.Skip(1))
                        });
                        break;
                    case "FNDA":
                        assembly.Functions.Details
                            .Single(d => d.Name == string.Join(',', parts.Skip(1)))
                            .Hit = int.Parse(parts[0]);
                        break;
                    case "BRDA":
                        assembly.Branches.Details.Add(new CoverageBranchDetail
                        {
                            Line = int.Parse(parts[0]),
                            Block = int.Parse(parts[1]),
                            Branch = int.Parse(parts[2]),
                            Taken = parts[3] == "-" ? 0 : int.Parse(parts[2])
                        });
                        break;
                    case "BRF":
                        assembly.Branches.Found = int.Parse(value);
                        break;
                    case "BRH":
                        assembly.Branches.Hit = int.Parse(value);
                        break;
                    case "END_OF_RECORD":
                        yield return assembly;
                        assembly = new CoverageItem();
                        break;
                }
            }
        }
    }
}
