using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class SeasonStatsParameters
    {
        public SeasonStatsParameters(int li, int? st, int? pn, string so, int? sd, int? ti)
        {
            leagueId = li;
            seasonTypeId = st;
            pageNumber = pn;
            sortOrder = so;
            teamId = ti;
            sortDescending = !sd.HasValue || sd.Value == 0;
        }

        public SeasonStatsParameters(int li, int? sn, int? st, int? pn, int? ti, string so, int? sd, int? era)
        {
            leagueId = li;
            seasonNumber = sn;
            seasonTypeId = st;
            pageNumber = pn;
            teamId = ti;
            sortOrder = so;
            sortDescending = !sd.HasValue || sd.Value == 0;
            leagueEra = era;
        }

        public int leagueId { get; set; }
        public int? seasonNumber { get; set; }
        public int? seasonTypeId { get; set; }
        public int? pageNumber { get; set; }
        public int? teamId { get; set; }
        public string sortOrder { get; set; }
        public bool sortDescending { get; set; }
        public int? leagueEra { get; set; }
    }
}