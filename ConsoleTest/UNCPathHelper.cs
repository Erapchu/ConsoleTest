using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public static class UNCPathHelper
    {
        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int WNetGetConnection(
            [MarshalAs(UnmanagedType.LPWStr)] string localName,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder remoteName,
            ref int length);

        /// <summary>
        /// Get connection string from path.
        /// Converts <c>"Z:\subfolder"</c> to <c>"\\DESKTOP-AQHF511\top-shared-folder"</c>
        /// </summary>
        /// <param name="originalPath">Usual path with drive letter.</param>
        /// <returns>Connection path.</returns>
        public static string GetConnectionPath(string originalPath)
        {
            StringBuilder sb = new(512);
            int size = sb.Capacity;
            // look for the {LETTER}: combination ...
            if (originalPath.Length > 2 && originalPath[1] == ':')
            {
                // don't use char.IsLetter here - as that can be misleading
                // the only valid drive letters are a-z && A-Z.
                char c = originalPath[0];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    int error = WNetGetConnection(originalPath.Substring(0, 2), sb, ref size);
                    if (error == 0)
                    {
                        return sb.ToString();
                    }
                    else
                    {
                        throw new Win32Exception(error);
                    }
                }
            }
            return originalPath;
        }

        /// <summary>
        /// Combine nested path from original to full UNC.
        /// Converts <c>"Z:\subfolder"</c> to <c>"\\DESKTOP-AQHF511\top-shared-folder\subfolder"</c>.
        /// </summary>
        /// <param name="originalPath">Usual path with drive letter.</param>
        /// <returns>Full UNC path.</returns>
        public static string GetUNCPath(string originalPath)
        {
            var connection = GetConnectionPath(originalPath);

            var dir = new DirectoryInfo(originalPath);
            string path = Path.GetFullPath(originalPath)[Path.GetPathRoot(originalPath).Length..];
            return Path.Combine(connection.TrimEnd(), path);
        }
    }
}
