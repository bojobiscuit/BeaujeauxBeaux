using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public abstract class SeasonStatsModel
    {
        public League SelectedLeague { get; set; }
        public Season SelectedSeason { get; set; }
        public Team SelectedTeam { get; set; }
        public SeasonType SelectedSeasonType { get; set; }
        public string SelectedColumnSort { get; set; }
        public int SelectedColumnSortIndex { get; set; }
        public bool IsSortDescending { get; set; }
        public int? SelectedLeagueEra { get; set; }

        public IEnumerable<int> SeasonNumberOptions { get; set; }
        public IEnumerable<SeasonType> SeasonTypeOptions { get; set; }
        public IEnumerable<Team> TeamOptions { get; set; }
        public IEnumerable<League> LeagueOptions { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string AlertMessage { get; set; }
        public IList<ColumnHeader> ColumnHeaders { get; set; }

        protected SeasonStatsParameters _seasonStatsParameters;

        public SeasonStatsModel(SeasonStatsParameters seasonParameters)
        {
            _seasonStatsParameters = seasonParameters;
            _database = new BeaujeauxEntities();

            // League
            LeagueOptions = _database.Leagues.ToList();
            SelectedLeague = LeagueOptions.Where(s => s.Id == _seasonStatsParameters.leagueId).FirstOrDefault();
            SelectedLeague = SelectedLeague ?? LeagueOptions.FirstOrDefault();

            // All Seasons for the League
            var _seasons = _database.Seasons
                .Where(s => s.LeagueId == SelectedLeague.Id);

            SelectedLeagueEra = seasonParameters.leagueEra;

            SetSeasonNumberOptions();
            SetSeasonTypeOptions(_seasons);
            SetSelectedSeasonType(_seasonStatsParameters.seasonTypeId);
            SetSelectedSeason(_seasonStatsParameters.seasonNumber);
            SetTeamOptions(_seasons);
            SetSelectedTeam(_seasonStatsParameters.teamId);
            SetPageNumberInfo(seasonParameters.pageNumber);

            SetStatsForPage();
            TotalPages = GetTotalPageCount();
            BoundCurrentPage();
        }

        private void SetSelectedTeam(int? teamId)
        {
            SelectedTeam = null;
            if (teamId != null)
            {
                SelectedTeam = TeamOptions
                    .Where(to => to.Id == teamId)
                    .FirstOrDefault();

                if (SelectedTeam == null)
                    AlertMessage = "That team could not be found.";
            }
        }

        private void SetTeamOptions(IQueryable<Season> _seasons)
        {
            if (SelectedSeason != null)
            {
                TeamOptions = SelectedSeason
                    .GoalieSeasonStats
                    .Select(gss => gss.Team)
                    .Distinct()
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            else
            {
                TeamOptions = _seasons
                    .SelectMany(s => s.Teams)
                    .Distinct()
                    .OrderBy(t => t.Name)
                    .ToList();
            }
        }

        private void BoundCurrentPage()
        {
            PageNumber = Math.Min(TotalPages, PageNumber);
            PageNumber = Math.Max(1, PageNumber);
        }

        protected abstract void SetStatsForPage();

        private void SetSeasonNumberOptions()
        {
            SeasonNumberOptions = _database.Seasons
                            .Where(s => s.LeagueId == SelectedLeague.Id)
                            .Select(s => s.Number)
                            .Distinct()
                            .OrderByDescending(s => s)
                            .ToList();
        }

        private void SetSeasonTypeOptions(IQueryable<Season> _seasons)
        {
            SeasonTypeOptions = _seasons
                            .Select(s => s.SeasonType)
                            .Distinct()
                            .ToList();
        }

        private void SetSelectedSeasonType(int? seasonTypeId)
        {
            SelectedSeasonType = null;
            if (seasonTypeId != null)
            {
                SelectedSeasonType = SeasonTypeOptions
                    .Where(st => st.Id == seasonTypeId)
                    .FirstOrDefault();
            }
            if (SelectedSeasonType == null)
                SelectedSeasonType = SeasonTypeOptions.First();
        }

        private void SetSelectedSeason(int? seasonNumber)
        {
            SelectedSeason = null;
            if (seasonNumber != null)
            {
                IQueryable<Season> seasons = GetSeasons()
                    .Where(s => s.Number == seasonNumber)
                    .Where(s => s.SeasonTypeId == SelectedSeasonType.Id);

                if (!seasons.Any())
                {
                    AlertMessage = $"There's no data for season {seasonNumber} during the {SelectedSeasonType.Name}.";
                    SelectedSeason = null;
                }
                else
                {
                    SelectedSeason = seasons.First();
                }
            }
        }

        private IQueryable<Season> GetSeasons()
        {
            var seasons = _database.Seasons.Where(s => s.LeagueId == SelectedLeague.Id);
            if (SelectedSeasonType != null)
                seasons = seasons.Where(s => s.SeasonTypeId == SelectedSeasonType.Id);
            return seasons;
        }

        protected abstract int GetTotalPageCount();

        private void SetPageNumberInfo(int? pageNumber)
        {
            PageSize = 25;
            PageNumber = pageNumber ?? 1;

        }

        public string GetLastName(string name)
        {
            var splitName = name.Split(' ');
            if (splitName.Length <= 1)
                return name;

            return splitName.Last();
        }

        public string GetImagePath(Team team)
        {
            string leagueAcro = SelectedLeague.Acronym;
            string iconName = team == null ? "shl" : team.IconName ?? team.Acronym;
            return $"{leagueAcro}\\{iconName}.png";
        }

        public string GetSeasonLink(int sn)
        {
            var link = new LinkInfo() { seasonNum = sn };
            return GetLink(link);
        }

        public string GetLeagueLink(int li)
        {
            var link = new LinkInfo() { leagueId = li };
            return GetLink(link);
        }

        public string GetAllSeasonLink()
        {
            var link = new LinkInfo() { seasonNum = 0 };
            return GetLink(link);
        }

        public string GetAllErasLink()
        {
            var link = new LinkInfo() { era = 0 };
            return GetLink(link);
        }

        public string GetEraLink(int? eraId)
        {
            var link = new LinkInfo() { era = eraId };
            return GetLink(link);
        }

        public string GetSeasonTypeLink(int st)
        {
            var link = new LinkInfo() { seasonType = st };
            return GetLink(link);
        }

        public string GetTeamLink(int ti)
        {
            var link = new LinkInfo() { teamId = ti };
            return GetLink(link);
        }

        public string GetAllTeamLink()
        {
            var link = new LinkInfo() { teamId = 0 };
            return GetLink(link);
        }

        public string GetPageLink(int pn)
        {
            var link = new LinkInfo() { pageNum = pn };
            return GetLink(link);
        }

        public string GetGoalieLink()
        {
            var link = new LinkInfo() { playerType = "Goalie" };
            return GetLink(link);
        }

        public string GetSkaterLink()
        {
            var link = new LinkInfo() { playerType = "Skater" };
            return GetLink(link);
        }

        public string GetPrevPage()
        {
            int destination = PageNumber - 1;
            if (destination < 1) destination = 1;
            var link = new LinkInfo() { pageNum = destination };
            return GetLink(link);
        }

        public string GetNextPage()
        {
            int destination = PageNumber + 1;
            if (destination > TotalPages) destination = TotalPages;
            var link = new LinkInfo() { pageNum = destination };
            return GetLink(link);
        }

        public string GetSortLink(string so)
        {
            var link = new LinkInfo() { sortOrder = so };
            return GetLink(link);
        }

        public string GetSortLinkSelected(string so)
        {
            var link = new LinkInfo() { sortOrder = so, sortDesc = !IsSortDescending };
            return GetLink(link);
        }

        private string GetLink(LinkInfo info)
        {
            if (SelectedSeasonType != null)
                info.seasonType = info.seasonType ?? SelectedSeasonType.Id;
            else
                info.seasonType = info.seasonType ?? _database.SeasonTypes.First().Id;

            if (SelectedSeason != null)
                info.seasonNum = info.seasonNum ?? SelectedSeason.Number;
            if (info.seasonNum == 0)
                info.seasonNum = null;

            if (SelectedTeam != null)
                info.teamId = info.teamId ?? SelectedTeam.Id;
            if (info.teamId == 0)
                info.teamId = null;

            if (SelectedColumnSort != null)
                info.sortOrder = info.sortOrder ?? SelectedColumnSort;

            if (SelectedLeagueEra != null)
                info.era = info.era ?? SelectedLeagueEra;
            if (info.era == 0)
                info.era = null;

            if (info.playerType != null)
            {
                info.sortDesc = true;
                info.sortOrder = null;
                info.pageNum = null;
            }
            else info.playerType = PageName;

            List<string> parameters = new List<string>();
            if (info.leagueId != null)
                parameters.Add($"li={info.leagueId}");
            else
            {
                if (info.seasonNum != null)
                    parameters.Add($"sn={info.seasonNum}");
                if (info.seasonType != null)
                    parameters.Add($"st={info.seasonType}");
                if (info.teamId != null)
                    parameters.Add($"ti={info.teamId}");
                if (info.era != null)
                    parameters.Add($"era={info.era}");
                if (info.pageNum != null)
                    parameters.Add($"pn={info.pageNum}");
                if (info.sortOrder != null)
                    parameters.Add($"so={info.sortOrder}");
                if (info.sortDesc == false)
                    parameters.Add($"sd=1");
            }

            return $"{info.playerType}?{string.Join("&", parameters)}";
        }

        protected abstract IEnumerable<Team> GetTeamOptions();

        protected void SetSelectedTeam()
        {
            TeamOptions = GetTeamOptions();

            SelectedTeam = null;
            if (_seasonStatsParameters.teamId != null)
            {
                SelectedTeam = TeamOptions
                    .Where(t => t.Id == _seasonStatsParameters.teamId)
                    .FirstOrDefault();

                // Check if team was under a different name
                if (SelectedTeam == null)
                {
                    var franchise = _database.Franchises
                        .Where(f => f.Teams
                            .Select(t => t.Id)
                            .ToList()
                            .Contains(_seasonStatsParameters.teamId.Value))
                        .FirstOrDefault();

                    if (franchise != null)
                    {
                        SelectedTeam = franchise.Teams
                            .Where(t => t.Id == _seasonStatsParameters.teamId)
                            .FirstOrDefault();
                    }
                }
            }
        }

        protected BeaujeauxEntities _database;

        protected abstract string PageName { get; }

        protected int PageIndex { get { return PageNumber - 1; } }

        private class LinkInfo
        {
            public int? leagueId = null;
            public int? seasonNum = null;
            public int? seasonType = null;
            public int? teamId = null;
            public int? pageNum = null;
            public string sortOrder = null;
            public bool sortDesc = true;
            public int? era = null;
            public string playerType = null;
        }

    }
}