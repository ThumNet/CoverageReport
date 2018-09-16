namespace System.IO
{
    public static class PathExtensions
    {
        public static string GetParentDirectory(this string path, int upCount)
        {
            for (var i = 0; i < upCount; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            return path;
        }
    }
}
