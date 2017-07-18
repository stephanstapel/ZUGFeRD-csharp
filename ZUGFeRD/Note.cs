using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    public class Note
    {
        public string Content { get; set; }
        public SubjectCodes SubjectCode { get; set; } = SubjectCodes.Unknown;
        public ContentCodes ContentCode { get; set; } = ContentCodes.Unknown;

        public Note(string content, SubjectCodes subjectCode, ContentCodes contentCode)
        {
            this.Content = content;
            this.SubjectCode = subjectCode;
            this.ContentCode = contentCode;
        }
    }
}
