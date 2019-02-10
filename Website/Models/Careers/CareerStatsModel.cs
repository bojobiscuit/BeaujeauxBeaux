using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public abstract class CareerStatsModel
    {
        public League SelectedLeague { get; set; }
        public Team SelectedTeam { get; set; }
        public SeasonType SelectedSeasonType { get; set; }
        public string SelectedColumnSort { get; set; }
        public int SelectedColumnSortIndex { get; set; }
        public bool IsSortDescending { get; set; }

        public IEnumerable<SeasonType> SeasonTypeOptions { get; set; }
        public IEnumerable<League> LeagueOptions { get; set; }
        public IEnumerable<Team> TeamOptions { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string AlertMessage { get; set; }
        public IList<ColumnHeader> ColumnHeaders { get; set; }

        protected SeasonStatsParameters _seasonStatsParameters;

        public CareerStatsModel(SeasonStatsParameters seasonParameters)
        {
            _seasonStatsParameters = seasonParameters;
            _database = new BeaujeauxEntities();

            LeagueOptions = _database.Leagues.ToList();
            SelectedLeague = LeagueOptions.Where(s => s.Id == _seasonStatsParameters.leagueId).FirstOrDefault();
            SelectedLeague = SelectedLeague ?? LeagueOptions.FirstOrDefault();

            SetSeasonTypeOptions();
            SetSelectedSeasonType(_seasonStatsParameters.seasonTypeId);

            SetTeamOptions();
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
                    .Where(t => t.Id == teamId)
                    .FirstOrDefault();
            }
        }

        private void SetTeamOptions()
        {
            TeamOptions = _database.GoalieSeasonStats
                .Where(a => a.Season.LeagueId == SelectedLeague.Id)
                .Select(a => a.Team)
                .Distinct()
                .OrderBy(a => a.Name)
                .ToList();
        }

        private void BoundCurrentPage()
        {
            PageNumber = Math.Min(TotalPages, PageNumber);
            PageNumber = Math.Max(1, PageNumber);
        }

        protected abstract void SetStatsForPage();

        private void SetSeasonTypeOptions()
        {
            SeasonTypeOptions = _database.SkaterSeasonStats
                            .Where(s => s.Season.LeagueId == SelectedLeague.Id)
                            .Select(s => s.Season.SeasonType)
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

        public string GetSeasonTypeLink(int st)
        {
            var link = new LinkInfo() { seasonType = st };
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

        public string GetLeagueLink(int li)
        {
            var link = new LinkInfo() { leagueId = li };
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

        public string GetAllTeamLink()
        {
            var link = new LinkInfo() { teamId = 0 };
            return GetLink(link);
        }

        public string GetTeamLink(int ti)
        {
            var link = new LinkInfo() { teamId = ti };
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

            if (SelectedColumnSort != null)
                info.sortOrder = info.sortOrder ?? SelectedColumnSort;

            if (info.playerType != null)
            {
                info.sortDesc = true;
                info.sortOrder = null;
                info.pageNum = null;
            }
            else info.playerType = PageName;

            if (SelectedTeam != null)
                info.teamId = info.teamId ?? SelectedTeam.Id;
            if (info.teamId == 0)
                info.teamId = null;

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
                if (info.teamId != null)
                    parameters.Add($"ti={info.teamId}");
            }

            return $"{info.playerType}?{string.Join("&", parameters)}";
        }

        protected BeaujeauxEntities _database;

        protected abstract string PageName { get; }

        protected int PageIndex { get { return PageNumber - 1; } }

        private class LinkInfo
        {
            public int? leagueId = null;
            public int? seasonType = null;
            public int? pageNum = null;
            public int? teamId = null;
            public string sortOrder = null;
            public bool sortDesc = true;
            public string playerType = null;
        }

    }
}