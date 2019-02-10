using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class ColumnHeader
    {
        public string Display { get; set; }
        public string Name { get; set; }
        public bool IsSpacer { get; set; }

        public ColumnHeader(string name, string display, bool spacer = false)
        {
            Display = display;
            Name = name;
            IsSpacer = spacer;
        }

    }
}