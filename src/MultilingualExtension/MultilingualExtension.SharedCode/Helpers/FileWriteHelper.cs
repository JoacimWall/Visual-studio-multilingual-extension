using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MultilingualExtension.Shared.Helpers
{
    /// <summary>
    /// Writes files through a temp file in the same folder and a rename, so a stopped run never leaves a half-written file.
    /// Only uses APIs available on .NET Framework 4.7.2.
    /// </summary>
    public static class FileWriteHelper
    {
        public static void WriteBytes(string path, byte[] content) => Write(path, temp => File.WriteAllBytes(temp, content));

        public static void WriteText(string path, string content, Encoding encoding = null) =>
            Write(path, temp => File.WriteAllText(temp, content, encoding ?? new UTF8Encoding(true)));

        public static void Save(string path, XmlDocument document) => Write(path, temp => document.Save(temp));

        public static void Save(string path, XDocument document) => Write(path, temp => document.Save(temp));

        /// <summary>Temp path in the target's own folder, so the rename stays on one volume.</summary>
        public static string GetTempPath(string path) =>
            Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path)), "." + Path.GetFileName(path) + "." + Guid.NewGuid().ToString("N") + ".tmp");

        /// <summary>Swaps the temp file in for the target. Deletes the temp file and rethrows on failure.</summary>
        public static void Commit(string tempPath, string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Replace(tempPath, path, null);
                else
                    File.Move(tempPath, path);
            }
            catch
            {
                DeleteQuietly(tempPath);
                throw;
            }
        }

        public static void DeleteQuietly(string path)
        {
            try { File.Delete(path); } catch { }
        }

        private static void Write(string path, Action<string> writeTemp)
        {
            var temp = GetTempPath(path);
            try
            {
                writeTemp(temp);
            }
            catch
            {
                DeleteQuietly(temp);
                throw;
            }
            Commit(temp, path);
        }
    }
}
