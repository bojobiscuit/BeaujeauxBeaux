using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public abstract class PlayerStatsModel
    {
        public League SelectedLeague { get; set; }
        public IEnumerable<League> LeagueOptions { get; set; }

        public PlayerStatsModel(BeaujeauxEntities database, int leagueId)
        {
            LeagueOptions = database.Leagues.ToList();
            SelectedLeague = LeagueOptions.Where(s => s.Id == leagueId).FirstOrDefault();
            SelectedLeague = SelectedLeague ?? LeagueOptions.FirstOrDefault();
        }

        public string GetImagePath(Season season, Team team)
        {
            string leagueAcro = season.League.Acronym;
            string iconName = team == null ? "shl" : team.IconName ?? team.Acronym;
            return $"{leagueAcro}\\{iconName}.png";
        }

        public string GetLeagueLink(int li)
        {
            var link = new LinkInfo() { leagueId = li };
            return GetLink(link);
        }

        protected abstract string PageName { get; }
        protected abstract int PlayerId { get; }

        private string GetLink(LinkInfo info)
        {
            //if (SelectedSeasonType != null)
            //    info.seasonType = info.seasonType ?? SelectedSeasonType.Id;
            //else
            //    info.seasonType = info.seasonType ?? _database.SeasonTypes.First().Id;

            //if (SelectedColumnSort != null)
            //    info.sortOrder = info.sortOrder ?? SelectedColumnSort;

            List<string> parameters = new List<string>();
            if (info.leagueId != null)
                parameters.Add($"li={info.leagueId}");
            else
            {
                if (info.seasonType != null)
                    parameters.Add($"st={info.seasonType}");
                if (info.pageNum != null)
                    parameters.Add($"pn={info.pageNum}");
                if (info.sortOrder != null)
                    parameters.Add($"so={info.sortOrder}");
                if (info.sortDesc == false)
                    parameters.Add($"sd=1");
            }

            parameters.Add($"pi={PlayerId}");


            return $"{PageName}?{string.Join("&", parameters)}";
        }


        private class LinkInfo
        {
            public int? leagueId = null;
            public int? seasonType = null;
            public int? pageNum = null;
            public string sortOrder = null;
            public bool sortDesc = true;
        }

    }
}