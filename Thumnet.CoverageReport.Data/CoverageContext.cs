using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Thumnet.CoverageReport.Core.Entities;

namespace Thumnet.CoverageReport.Data
{
    public class CoverageContext : DbContext
    {
        public CoverageContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<CoverageEntry> Entries { get; set; }
        public DbSet<SourceFileEntry> SourceFiles { get; set; }
        public DbSet<FileData> FilesData { get; set; }

    }
}
