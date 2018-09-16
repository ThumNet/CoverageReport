using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thumnet.CoverageReport.Core.Entities;
using Thumnet.CoverageReport.Core.Helpers;
using Thumnet.CoverageReport.Core.Models;
using Thumnet.CoverageReport.Data;
using Thumnet.CoverageReport.Data.Repositories;
using Xunit;

namespace Thumnet.CoverageReport.Test.Data.Repositories
{
    public class ReportRepositoryTest
    {
        private readonly CoverageContext _context;

        public ReportRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<CoverageContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new CoverageContext(options);
        }

        [Fact]
        public void AddCoverageReport_HappyFlow()
        {
            // Arrange
            var report = new ReportInput
            {
                ProjectName = "Test",
                ProjectUrl = "http://test.test/test",
                BranchName = "master",
                LcovData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                SourceFilesData = new Dictionary<string, byte[]>
                    {
                        { "test.cs", new byte[] { 0x20, 0x20, 0x20 } }
                    }
            };
            var repository = new ReportRepository(_context);

            // Act
            repository.AddCoverageReport(report);

            // Assert
            Assert.Equal(1, _context.Projects.Count());
            var project = _context.Projects.First();
            Assert.Equal(report.ProjectName, project.Name);
            Assert.Equal(report.ProjectUrl, project.Url);

            Assert.Equal(1, _context.Entries.Count());
            var entry = _context.Entries.First();
            Assert.Equal(report.BranchName, entry.BranchName);
            Assert.Equal(report.LcovData, entry.LcovData);
            Assert.Equal(project.Id, entry.ProjectId);

            Assert.Equal(1, _context.SourceFiles.Count());
            var sourceFile = _context.SourceFiles.First();
            Assert.Equal(report.SourceFilesData.First().Key, sourceFile.FilePath);            
            Assert.Equal(entry.Id, sourceFile.CoverageEntryId);

            Assert.Equal(1, _context.FilesData.Count());
            var fileData = _context.FilesData.First();
            Assert.Equal(report.SourceFilesData.First().Value, fileData.Bytes);
            Assert.Equal(sourceFile.DataId, fileData.Id);
        }

        [Fact]
        public void Post_AddsToDb_LinksToExistingProject()
        {
            var report = new ReportInput
            {
                ProjectName = "Test",
                ProjectUrl = "http://test.test/test",
                BranchName = "master",
                LcovData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                SourceFilesData = new Dictionary<string, byte[]>
                    {
                        { "test.cs", new byte[] { 0x20, 0x20, 0x20 } }
                    }
            };
            var repository = new ReportRepository(_context);

            AddToInMemoryDb(new Project
            {
                Name = report.ProjectName,
                Url = report.ProjectUrl,
            });

            // Act
            repository.AddCoverageReport(report);

            // Assert
            Assert.Equal(1, _context.Projects.Count());
            var project = _context.Projects.First();
            Assert.Equal(1, _context.Entries.Count());
            var entry = _context.Entries.First();
            Assert.Equal(project.Id, entry.ProjectId);
        }

        [Fact]
        public void Post_AddsToDb_LinksToFileData()
        {
            var report = new ReportInput
            {
                ProjectName = "Test",
                ProjectUrl = "http://test.test/test",
                BranchName = "master",
                LcovData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
                SourceFilesData = new Dictionary<string, byte[]>
                    {
                        { "test.cs", new byte[] { 0x20, 0x20, 0x20 } }
                    }
            };
            var repository = new ReportRepository(_context);

            AddToInMemoryDb(new FileData
            {
                Bytes = report.SourceFilesData.First().Value,
                Checksum = Md5Hash.Create(report.SourceFilesData.First().Value)
            });

            // Act
            repository.AddCoverageReport(report);

            // Assert
            Assert.Equal(1, _context.Projects.Count());
            Assert.Equal(1, _context.Entries.Count());
            Assert.Equal(1, _context.SourceFiles.Count());
            Assert.Equal(1, _context.FilesData.Count());

            var project = _context.Projects.First();
            var entry = _context.Entries.First();
            var sourceFile = _context.SourceFiles.First(); 
            var fileData = _context.FilesData.First();

            Assert.Equal(project.Id, entry.ProjectId);
            Assert.Equal(entry.Id, sourceFile.CoverageEntryId);
            Assert.Equal(fileData.Id, sourceFile.DataId);
        }

        private void AddToInMemoryDb(object entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
    }
}
