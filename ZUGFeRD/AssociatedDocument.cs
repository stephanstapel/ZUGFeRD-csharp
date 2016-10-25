using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    public class AssociatedDocument
    {
        public string Content { get; set; }
        public SubjectCodes ContentSubjectCode { get; set; }
        public int? LineID { get; set; }

        public AssociatedDocument()
        {
            this.ContentSubjectCode = SubjectCodes.Unknown;
        }
    }
}
