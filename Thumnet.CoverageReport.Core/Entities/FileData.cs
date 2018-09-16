namespace Thumnet.CoverageReport.Core.Entities
{
    public class FileData
    {
        public int Id { get; set; }
        public string Checksum { get; set; }
        public byte[] Bytes { get; set; }
    }
}
