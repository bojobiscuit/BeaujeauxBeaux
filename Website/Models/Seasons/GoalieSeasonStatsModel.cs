using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class GoalieSeasonStatsModel : SeasonStatsModel
    {
        public IEnumerable<GoalieSeasonStat> Stats { get; set; }

        public GoalieSeasonStatsModel(SeasonStatsParameters parameters) : base(parameters)
        {
        }

        protected override IEnumerable<Team> GetTeamOptions()
        {
            return Stats
                .Select(sss => sss.Team)
                .Distinct()
                .OrderBy(t => t.Name)
                .ToList();
        }

        protected override int GetTotalPageCount()
        {
            return (int)Math.Ceiling(GetGoalieStatsQuery().Count() / (decimal)PageSize);
        }

        protected override void SetStatsForPage()
        {
            SetColumnHeaders();
            Stats = SortBy(GetGoalieStatsQuery(), _seasonStatsParameters.sortOrder, _seasonStatsParameters.sortDescending)
                .Skip(PageIndex * PageSize)
                .Take(PageSize)
                .ToList();
        }

        protected override string PageName { get { return "Goalie"; } }

        private IQueryable<GoalieSeasonStat> GetGoalieStatsQuery()
        {
            var stats = _database.GoalieSeasonStats
                .Where(sss => sss.Season.LeagueId == SelectedLeague.Id);

            if (SelectedSeason != null)
                stats = stats.Where(sss => sss.Season.Id == SelectedSeason.Id);
            if (SelectedTeam != null)
                stats = stats.Where(sss => sss.Team.FranchiseId == SelectedTeam.FranchiseId);
            if (SelectedSeasonType != null)
                stats = stats.Where(sss => SelectedSeasonType.Id == sss.Season.SeasonTypeId);
            if (SelectedLeagueEra != null)
            {
                if (SelectedLeagueEra == 1)
                    stats = stats.Where(sss => sss.Season.Number >= 12);
                else if (SelectedLeagueEra == 2)
                    stats = stats.Where(sss => sss.Season.Number <= 11 && sss.Season.Number >= 9);
                else if (SelectedLeagueEra == 3)
                    stats = stats.Where(sss => sss.Season.Number <= 8);
                else
                {
                    AlertMessage = "No Era with that ID";
                }
            }

            return stats.Where(sss => !sss.IsSubtotal);
        }

        private void SetColumnHeaders()
        {
            List<ColumnHeader> columnHeaders = new List<ColumnHeader>();
            bool isSpacer = true;
            columnHeaders.Add(new ColumnHeader("GP", "GP"));
            columnHeaders.Add(new ColumnHeader("W", "W"));
            columnHeaders.Add(new ColumnHeader("L", "L"));
            columnHeaders.Add(new ColumnHeader("OTL", "OTL"));
            columnHeaders.Add(new ColumnHeader("MP", "MP", isSpacer));
            columnHeaders.Add(new ColumnHeader("PIM", "PIM"));
            columnHeaders.Add(new ColumnHeader("SO", "SO", isSpacer));
            columnHeaders.Add(new ColumnHeader("A", "A"));
            columnHeaders.Add(new ColumnHeader("EG", "EG"));
            columnHeaders.Add(new ColumnHeader("GA", "GA", isSpacer));
            columnHeaders.Add(new ColumnHeader("SA", "SA"));
            columnHeaders.Add(new ColumnHeader("PSS", "PSS", isSpacer));
            columnHeaders.Add(new ColumnHeader("PSA", "PSA"));
            columnHeaders.Add(new ColumnHeader("ST", "ST", isSpacer));
            columnHeaders.Add(new ColumnHeader("BG", "BG"));
            columnHeaders.Add(new ColumnHeader("S1", "S1", isSpacer));
            columnHeaders.Add(new ColumnHeader("S2", "S2"));
            columnHeaders.Add(new ColumnHeader("S3", "S3"));
            ColumnHeaders = columnHeaders;
        }

        private IQueryable<GoalieSeasonStat> SortBy(IQueryable<GoalieSeasonStat> stats, string column, bool sortDesc)
        {
            if (column == null)
                column = "W";

            IsSortDescending = sortDesc;

            IOrderedQueryable<GoalieSeasonStat> orderableStats;
            if (IsSortDescending)
            {
                switch (column)
                {
                    case "GP": orderableStats = stats.OrderByDescending(s => s.GP); break;
                    case "W": orderableStats = stats.OrderByDescending(s => s.W); break;
                    case "L": orderableStats = stats.OrderByDescending(s => s.L); break;
                    case "OTL": orderableStats = stats.OrderByDescending(s => s.OTL); break;
                    case "MP": orderableStats = stats.OrderByDescending(s => s.MP); break;
                    case "PIM": orderableStats = stats.OrderByDescending(s => s.PIM); break;
                    case "SO": orderableStats = stats.OrderByDescending(s => s.SO); break;
                    case "A": orderableStats = stats.OrderByDescending(s => s.A); break;
                    case "EG": orderableStats = stats.OrderByDescending(s => s.EG); break;
                    case "GA": orderableStats = stats.OrderByDescending(s => s.GA); break;
                    case "SA": orderableStats = stats.OrderByDescending(s => s.SA); break;
                    case "PSS": orderableStats = stats.OrderByDescending(s => s.PSS); break;
                    case "PSA": orderableStats = stats.OrderByDescending(s => s.PSA); break;
                    case "ST": orderableStats = stats.OrderByDescending(s => s.ST); break;
                    case "BG": orderableStats = stats.OrderByDescending(s => s.BG); break;
                    case "S1": orderableStats = stats.OrderByDescending(s => s.S1); break;
                    case "S2": orderableStats = stats.OrderByDescending(s => s.S2); break;
                    case "S3": orderableStats = stats.OrderByDescending(s => s.S3); break;
                    default:
                        {
                            AlertMessage = "Column does not exist.";
                            goto case "W";
                        }
                }
            }
            else
            {
                switch (column)
                {
                    case "GP": orderableStats = stats.OrderBy(s => s.GP); break;
                    case "W": orderableStats = stats.OrderBy(s => s.W); break;
                    case "L": orderableStats = stats.OrderBy(s => s.L); break;
                    case "OTL": orderableStats = stats.OrderBy(s => s.OTL); break;
                    case "MP": orderableStats = stats.OrderBy(s => s.MP); break;
                    case "PIM": orderableStats = stats.OrderBy(s => s.PIM); break;
                    case "SO": orderableStats = stats.OrderBy(s => s.SO); break;
                    case "A": orderableStats = stats.OrderBy(s => s.A); break;
                    case "EG": orderableStats = stats.OrderBy(s => s.EG); break;
                    case "GA": orderableStats = stats.OrderBy(s => s.GA); break;
                    case "SA": orderableStats = stats.OrderBy(s => s.SA); break;
                    case "PSS": orderableStats = stats.OrderBy(s => s.PSS); break;
                    case "PSA": orderableStats = stats.OrderBy(s => s.PSA); break;
                    case "ST": orderableStats = stats.OrderBy(s => s.ST); break;
                    case "BG": orderableStats = stats.OrderBy(s => s.BG); break;
                    case "S1": orderableStats = stats.OrderBy(s => s.S1); break;
                    case "S2": orderableStats = stats.OrderBy(s => s.S2); break;
                    case "S3": orderableStats = stats.OrderBy(s => s.S3); break;
                    default:
                        {
                            AlertMessage = "Column does not exist.";
                            goto case "W";
                        }
                }
            }

            SelectedColumnSort = column;

            var matchedColumn = ColumnHeaders.Where(a => a.Name == column).FirstOrDefault();
            SelectedColumnSortIndex = ColumnHeaders.IndexOf(matchedColumn);

            if (column == "MP")
                return orderableStats;

            return (IsSortDescending) ?
                orderableStats.ThenBy(s => s.MP) :
                orderableStats.ThenByDescending(s => s.MP);
        }


    }
}