using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Classes
{
    public static class StatsHelper
    {

        public class LinkInfo
        {
            public int? seasonNum = null;
            public int? seasonType = null;
            public int? teamId = null;
            public int? pageNum = null;
            public string sortOrder = null;
            public bool sortDesc = true;
            public int? era = null;
        }

    }
}