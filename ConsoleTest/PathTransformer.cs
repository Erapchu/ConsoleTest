using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    internal class PathTransformer
    {
        private static readonly string _directorySeparatorString = Path.DirectorySeparatorChar.ToString();
        private static readonly string _altDirectorySeparatorString = Path.AltDirectorySeparatorChar.ToString();
        private static readonly string _specializedSeparator = "XXX XXX";
        private const string _specializedSymbolSequence = "XXX";

        /// <summary>
        /// Gets searching version of local path.
        /// Converts from C:\Windows\System32 to "XXXWindowsXXX XXXSystem32"
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <returns>Searcing string representation</returns>
        public static string GetSearchingPath(string localPath)
        {
            var result = PreparePath(localPath, _specializedSeparator);

            if (localPath.StartsWith(_directorySeparatorString) || localPath.StartsWith(_altDirectorySeparatorString))
                result = _specializedSymbolSequence + result;

            if (localPath.EndsWith(_directorySeparatorString) || localPath.EndsWith(_altDirectorySeparatorString))
                result += _specializedSymbolSequence;

            return result;
        }

        /// <summary>
        /// Gets indexing version of local path.
        /// Converts from C:\Windows\System32 to "XXXWindowsXXX XXXSystem32XXX"
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <returns>Indexing string representation</returns>
        public static string GetIndexingPath(string localPath)
        {
            var preparedPath = PreparePath(localPath, _specializedSeparator);
            var result = _specializedSymbolSequence + preparedPath + _specializedSymbolSequence;
            return result;
        }

        /// <summary>
        /// Constructs well-formed string array from local path.
        /// C:\Windows\System32 => "Windows System32".
        /// </summary>
        /// <returns>Well-formed array</returns>
        private static string PreparePath(string localPath, string separator = " ")
        {
            var splittedArray = localPath.Split(@"\/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var firstElement = splittedArray.FirstOrDefault();
            var skipFirstElement = false;

            if (firstElement != null && firstElement.Contains(Path.VolumeSeparatorChar))
                skipFirstElement = true;

            var wellFormedArray = skipFirstElement ? splittedArray.Skip(1) : splittedArray;
            var joinedString = string.Join(separator, wellFormedArray);
            return joinedString;
        }

        /// <summary>
        /// Gets splitted version of folder local path.
        /// Converts from C:\Windows\System32 to "Windows System32"
        /// </summary>
        /// <param name="localPath">Local path</param>
        /// <returns>Splitted string version</returns>
        public static string GetSplittedPath(string localPath)
        {
            var splittedPath = PreparePath(localPath);
            return splittedPath;
        }
    }
}
