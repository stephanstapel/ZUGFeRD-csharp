using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    public class AssociatedDocument
    {
        public List<Note> Notes { get; set; } = new List<Note>();
        public int? LineID { get; set; }

        public AssociatedDocument()
        {

        }


        public AssociatedDocument(int? lineID)
        {
            this.LineID = lineID;
        }
    }
}
