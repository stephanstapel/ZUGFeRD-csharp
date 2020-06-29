using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    public class BaseReferencedDocument
    {
        /// <summary>
        /// Bestellnummer / Lieferscheinummer
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Bestelldatum / Lieferdatum
        /// </summary>
        public DateTime? IssueDateTime { get; set; }
    }
}
