using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class GoalieCareerStatsModel : CareerStatsModel
    {
        public IEnumerable<GoalieCareerStatsDto> GroupedStats { get; set; }

        public GoalieCareerStatsModel(SeasonStatsParameters seasonParameters) : base(seasonParameters)
        {
        }

        protected override int GetTotalPageCount()
        {
            return (int)Math.Ceiling(GetGoalieStatsCount() / (decimal)PageSize);
        }

        protected override void SetStatsForPage()
        {
            SetColumnHeaders();
            GroupedStats = SortBy(GetGoalieStatsQuery(), _seasonStatsParameters.sortOrder, _seasonStatsParameters.sortDescending)
                .Skip(PageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var stats in GroupedStats)
            {
                var goalie = _database.Goalies
                    .Where(a => a.Id == stats.GoalieId)
                    .FirstOrDefault();

                stats.GoalieName = (goalie == null) ?
                    "--" : goalie.Name;
            }
        }

        protected override string PageName { get { return "Goalie"; } }

        private IQueryable<GoalieCareerStatsDto> GetGoalieStatsQuery()
        {
            var stats = _database.GoalieSeasonStats
                .Where(sss => sss.Season.LeagueId == SelectedLeague.Id);

            if (SelectedSeasonType != null)
                stats = stats.Where(sss => SelectedSeasonType.Id == sss.Season.SeasonTypeId);

            if (SelectedTeam != null)
            {
                // Filters by team, the only gets either the main total stats or the traded subtotal stats.
                stats = stats.Where(sss => sss.TeamId == SelectedTeam.Id);
                stats = stats.GroupBy(
                    x => new { x.GoalieId, x.SeasonId },
                    (a, b) => b.Where(c => b.Count() == 1 || c.IsSubtotal).FirstOrDefault()
                );
            }
            else
            {
                stats = stats.Where(sss => !sss.IsSubtotal);
            }

            return stats.GroupBy(
                x => new { x.GoalieId },
                (a, b) => new GoalieCareerStatsDto
                {
                    GoalieId = a.GoalieId,
                    SeasonCount = b.Select(s => s.SeasonId).Distinct().Count(),
                    TeamCount = b.Select(t => t.TeamId).Distinct().Count(),
                    GP = b.Sum(s => s.GP),
                    W = b.Sum(s => s.W),
                    L = b.Sum(s => s.L),
                    OTL = b.Sum(s => s.OTL),
                    MP = b.Sum(s => s.MP),
                    PIM = b.Sum(s => s.PIM),
                    SO = b.Sum(s => s.SO),
                    GA = b.Sum(s => s.GA),
                    SA = b.Sum(s => s.SA),
                    SAR = b.Sum(s => s.SAR),
                    A = b.Sum(s => s.A),
                    EG = b.Sum(s => s.EG),
                    PSS = b.Sum(s => s.PSS),
                    PSA = b.Sum(s => s.PSA),
                    ST = b.Sum(s => s.ST),
                    BG = b.Sum(s => s.BG),
                    S1 = b.Sum(s => s.S1),
                    S2 = b.Sum(s => s.S2),
                    S3 = b.Sum(s => s.S3),
                });
        }

        private int GetGoalieStatsCount()
        {
            var stats = _database.GoalieSeasonStats
                .Where(sss => sss.Season.LeagueId == SelectedLeague.Id);

            if (SelectedSeasonType != null)
                stats = stats.Where(sss => SelectedSeasonType.Id == sss.Season.SeasonTypeId);

            return stats.Select(a => a.GoalieId).Distinct().Count();
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

        private IQueryable<GoalieCareerStatsDto> SortBy(IQueryable<GoalieCareerStatsDto> groupedStats, string column, bool sortDesc)
        {
            if (column == null)
                column = "W";

            IsSortDescending = sortDesc;

            IOrderedQueryable<GoalieCareerStatsDto> orderableStats;
            if (IsSortDescending)
            {
                switch (column)
                {
                    case "GP": orderableStats = groupedStats.OrderByDescending(s => s.GP); break;
                    case "W": orderableStats = groupedStats.OrderByDescending(s => s.W); break;
                    case "L": orderableStats = groupedStats.OrderByDescending(s => s.L); break;
                    case "OTL": orderableStats = groupedStats.OrderByDescending(s => s.OTL); break;
                    case "MP": orderableStats = groupedStats.OrderByDescending(s => s.MP); break;
                    case "PIM": orderableStats = groupedStats.OrderByDescending(s => s.PIM); break;
                    case "SO": orderableStats = groupedStats.OrderByDescending(s => s.SO); break;
                    case "A": orderableStats = groupedStats.OrderByDescending(s => s.A); break;
                    case "EG": orderableStats = groupedStats.OrderByDescending(s => s.EG); break;
                    case "GA": orderableStats = groupedStats.OrderByDescending(s => s.GA); break;
                    case "SA": orderableStats = groupedStats.OrderByDescending(s => s.SA); break;
                    case "PSS": orderableStats = groupedStats.OrderByDescending(s => s.PSS); break;
                    case "PSA": orderableStats = groupedStats.OrderByDescending(s => s.PSA); break;
                    case "ST": orderableStats = groupedStats.OrderByDescending(s => s.ST); break;
                    case "BG": orderableStats = groupedStats.OrderByDescending(s => s.BG); break;
                    case "S1": orderableStats = groupedStats.OrderByDescending(s => s.S1); break;
                    case "S2": orderableStats = groupedStats.OrderByDescending(s => s.S2); break;
                    case "S3": orderableStats = groupedStats.OrderByDescending(s => s.S3); break;
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
                    case "GP": orderableStats = groupedStats.OrderBy(s => s.GP); break;
                    case "W": orderableStats = groupedStats.OrderBy(s => s.W); break;
                    case "L": orderableStats = groupedStats.OrderBy(s => s.L); break;
                    case "OTL": orderableStats = groupedStats.OrderBy(s => s.OTL); break;
                    case "MP": orderableStats = groupedStats.OrderBy(s => s.MP); break;
                    case "PIM": orderableStats = groupedStats.OrderBy(s => s.PIM); break;
                    case "SO": orderableStats = groupedStats.OrderBy(s => s.SO); break;
                    case "A": orderableStats = groupedStats.OrderBy(s => s.A); break;
                    case "EG": orderableStats = groupedStats.OrderBy(s => s.EG); break;
                    case "GA": orderableStats = groupedStats.OrderBy(s => s.GA); break;
                    case "SA": orderableStats = groupedStats.OrderBy(s => s.SA); break;
                    case "PSS": orderableStats = groupedStats.OrderBy(s => s.PSS); break;
                    case "PSA": orderableStats = groupedStats.OrderBy(s => s.PSA); break;
                    case "ST": orderableStats = groupedStats.OrderBy(s => s.ST); break;
                    case "BG": orderableStats = groupedStats.OrderBy(s => s.BG); break;
                    case "S1": orderableStats = groupedStats.OrderBy(s => s.S1); break;
                    case "S2": orderableStats = groupedStats.OrderBy(s => s.S2); break;
                    case "S3": orderableStats = groupedStats.OrderBy(s => s.S3); break;
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