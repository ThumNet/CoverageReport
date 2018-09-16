using System.Linq;
using System.Security.Cryptography;

namespace Thumnet.CoverageReport.Core.Helpers
{
    public static class Md5Hash
    {
        public static string Create(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                return string.Join("", hash.Select(b => b.ToString("X2")));
            }
        }
    }
}
