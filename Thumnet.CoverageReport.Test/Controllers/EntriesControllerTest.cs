using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thumnet.CoverageReport.Core.Entities;
using Thumnet.CoverageReport.Data;
using Thumnet.CoverageReport.WebApp.Controllers;
using Thumnet.CoverageReport.WebApp.ViewModels;
using Xunit;

namespace Thumnet.CoverageReport.Test.Controllers
{
    public class EntriesControllerTest
    {
        private readonly CoverageContext _context;

        public EntriesControllerTest()
        {
            var options = new DbContextOptionsBuilder<CoverageContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new CoverageContext(options);
        }

        [Fact]
        public void Post_AddsToDb()
        {
            // Arrange
            var model = new EntryPostViewModel
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
            var controller = new EntriesController(_context);

            // Act
            controller.Post(model);

            // Assert
            Assert.Equal(1, _context.Projects.Count());
            var project = _context.Projects.First();
            Assert.Equal(model.ProjectName, project.Name);
            Assert.Equal(model.ProjectUrl, project.Url);

            Assert.Equal(1, _context.Entries.Count());
            var entry = _context.Entries.First();
            Assert.Equal(model.BranchName, entry.Branchname);
            Assert.Equal(model.LcovData, entry.LcovData);
            Assert.Equal(project.Id, entry.ProjectId);

            Assert.Equal(1, _context.SourceFiles.Count());
            var sourceFile = _context.SourceFiles.First();
            Assert.Equal(model.SourceFilesData.First().Key, sourceFile.FilePath);
            Assert.Equal(model.SourceFilesData.First().Value, sourceFile.FileData);
            Assert.Equal(entry.Id, sourceFile.CoverageEntryId);
        }

        [Fact]
        public void Post_AddsToDb_LinksToExistingProject()
        {
            // Arrange            
            var model = new EntryPostViewModel
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
            AddToInMemoryDb(new Project
            {
                Name = model.ProjectName,
                Url = model.ProjectUrl,
            });

            var controller = new EntriesController(_context);

            // Act
            controller.Post(model);

            // Assert
            Assert.Equal(1, _context.Projects.Count());
            var project = _context.Projects.First();
            Assert.Equal(1, _context.Entries.Count());
            var entry = _context.Entries.First();
            Assert.Equal(project.Id, entry.ProjectId);
        }

        private void AddToInMemoryDb(object entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
    }
}
