using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public class IllegalStreamException : Exception
    {
        public IllegalStreamException(string message = "")
            : base(message)
        { 
        }
    }
}
