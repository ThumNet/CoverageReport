using LZString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Thumnet.CoverageReport.Core.Entities;
using Thumnet.CoverageReport.Core.Helpers;
using Thumnet.CoverageReport.Core.Interfaces;
using Thumnet.CoverageReport.Core.Models;

namespace Thumnet.CoverageReport.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private CoverageContext _coverageContext;

        public ReportRepository(CoverageContext coverageContext)
        {
            _coverageContext = coverageContext;
        }

        public void AddCoverageReport(ReportInput report)
        {
            var project = _coverageContext.Projects.FirstOrDefault(p => p.Name == report.ProjectName);
            if (project == null)
            {
                project = new Project
                {
                    Name = report.ProjectName,
                    Url = report.ProjectUrl
                };
                _coverageContext.Projects.Add(project);
            }

            var sourceChecksumsDictionary = report.SourceFilesData
                .ToDictionary(k => k.Key, v => Md5Hash.Create(v.Value));

            var sourceChecksums = sourceChecksumsDictionary.Values.ToList();
            var filesData = _coverageContext.FilesData
                .Where(f => sourceChecksums.Contains(f.Checksum))
                .ToDictionary(k => k.Checksum, v => v);

            var entry = new CoverageEntry
            {
                Created = DateTime.Now,
                BranchName = report.BranchName,
                LcovData = report.LcovData,
                Project = project,
            };

            foreach (var filePath in report.SourceFilesData.Keys)
            {
                var sourceFile = new SourceFileEntry
                {
                    FilePath = filePath,
                    Data = filesData.ContainsKey(sourceChecksumsDictionary[filePath])
                        ? filesData[sourceChecksumsDictionary[filePath]]
                        : new FileData { Checksum = sourceChecksumsDictionary[filePath], Bytes = report.SourceFilesData[filePath] }
                };
                entry.SourceFiles.Add(sourceFile);
            }

            _coverageContext.Entries.Add(entry);
            _coverageContext.SaveChanges();
        }

        public ReportOutput GetLatest(string projectName)
        {
            var entry = _coverageContext.Entries
                .Include(e => e.Project)
                .Include(e => e.SourceFiles)
                .ThenInclude(s => s.Data)
                .Where(e => e.Project.Name == projectName)
                .OrderByDescending(e => e.Created)
                .FirstOrDefault();

            return new ReportOutput
            {
                Created = entry.Created,
                ProjectName = entry.Project.Name,
                ProjectUrl = entry.Project.Url,
                LcovSource = LzString.CompressToBase64(LzString.DecompressFromUint8Array(entry.LcovData)),
                SourceFiles = entry.SourceFiles.ToDictionary(
                    k => k.FilePath,
                    v => LzString.CompressToBase64(LzString.DecompressFromUint8Array(v.Data.Bytes)))
            };
        }
    }
}
