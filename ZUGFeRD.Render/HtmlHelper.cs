using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace s2industries.ZUGFeRD.Render
{
    public class HtmlHelper
    {
        public HtmlString Raw(string source)
        {
            return new HtmlString(source);
        }
    }
}
