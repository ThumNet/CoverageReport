using System.IO;
using System.Reflection;
using System.Text;

namespace Thumnet.CoverageReport.Core.Replacers
{
    public class ResourceReader
    {
        private readonly Assembly _assembly;

        public ResourceReader(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string ReadResource(string name)
        {
            var resourceStream = _assembly.GetManifestResourceStream(name);
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
