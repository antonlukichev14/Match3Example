using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    internal class Path
    {
        private static string absoluteAssetsPath = "D:/Works/VS/OpenTK/Match3Example/Assets/";
        private static string relativeAssetsPath = "Assets/";

        public static bool useAbsolute = true;

        public static string GetAssetPath(string filename)
        {
            if (useAbsolute)
            {
                return absoluteAssetsPath + filename;
            }
            return relativeAssetsPath + filename;
        }
    }
}
