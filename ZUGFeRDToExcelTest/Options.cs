using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRDToExcel
{
    public class Options
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public bool Help { get; set; }
        public bool Recursive { get; set; }
    }
}
