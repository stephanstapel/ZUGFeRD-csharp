using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRD_Test
{
    public class TestBase
    {
        protected string _makeSurePathIsCrossPlatformCompatible(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            return path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
        } // !_makeSurePathIsCrossPlatformCompatible()
    }
}
