namespace Thumnet.CoverageReport.Core.Entities
{
    public class SourceFileEntry
    {
        public int Id { get; set; }
        public int CoverageEntryId { get; set; }
        public CoverageEntry CoverageEntry { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }
    }
}
