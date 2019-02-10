using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class SkaterCareerStatsModel : CareerStatsModel
    {
        public IEnumerable<SkaterCareerStatsDto> GroupedStats { get; set; }

        public SkaterCareerStatsModel(SeasonStatsParameters seasonParameters) : base(seasonParameters)
        {
        }

        protected override int GetTotalPageCount()
        {
            return (int)Math.Ceiling(GetSkaterStatsCount() / (decimal)PageSize);
        }

        protected override void SetStatsForPage()
        {
            SetColumnHeaders();
            GroupedStats = SortBy(GetSkaterStatsQuery(), _seasonStatsParameters.sortOrder, _seasonStatsParameters.sortDescending)
                .Skip(PageIndex * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var stats in GroupedStats)
            {
                stats.SkaterName = _database.Skaters
                    .First(a => a.Id == stats.SkaterId).Name;
            }
        }

        private IQueryable<SkaterCareerStatsDto> SortBy(IQueryable<SkaterCareerStatsDto> groupedStats, string column, bool sortDesc)
        {
            if (column == null)
                column = "P";

            IsSortDescending = sortDesc;

            IOrderedQueryable<SkaterCareerStatsDto> orderableStats;
            if (IsSortDescending)
            {
                switch (column)
                {
                    case "GP": orderableStats = groupedStats.OrderByDescending(s => s.GP); break;
                    case "G": orderableStats = groupedStats.OrderByDescending(s => s.G); break;
                    case "A": orderableStats = groupedStats.OrderByDescending(s => s.A); break;
                    case "P": orderableStats = groupedStats.OrderByDescending(s => s.P).ThenByDescending(s => s.G); break;
                    case "PLMI": orderableStats = groupedStats.OrderByDescending(s => s.PLMI); break;
                    case "SHT": orderableStats = groupedStats.OrderByDescending(s => s.SHT); break;
                    case "SB": orderableStats = groupedStats.OrderByDescending(s => s.SB); break;
                    case "MP": orderableStats = groupedStats.OrderByDescending(s => s.MP); break;
                    case "PIM": orderableStats = groupedStats.OrderByDescending(s => s.PIM); break;
                    case "PM5": orderableStats = groupedStats.OrderByDescending(s => s.PM5); break;
                    case "HIT": orderableStats = groupedStats.OrderByDescending(s => s.HIT); break;
                    case "HTT": orderableStats = groupedStats.OrderByDescending(s => s.HTT); break;
                    case "PPG": orderableStats = groupedStats.OrderByDescending(s => s.PPG); break;
                    case "PPA": orderableStats = groupedStats.OrderByDescending(s => s.PPA); break;
                    case "PPP": orderableStats = groupedStats.OrderByDescending(s => s.PPP); break;
                    case "PPS": orderableStats = groupedStats.OrderByDescending(s => s.PPS); break;
                    case "PPM": orderableStats = groupedStats.OrderByDescending(s => s.PPM); break;
                    case "PKG": orderableStats = groupedStats.OrderByDescending(s => s.PKG); break;
                    case "PKA": orderableStats = groupedStats.OrderByDescending(s => s.PKA); break;
                    case "PKP": orderableStats = groupedStats.OrderByDescending(s => s.PKP); break;
                    case "PKS": orderableStats = groupedStats.OrderByDescending(s => s.PKS); break;
                    case "PKM": orderableStats = groupedStats.OrderByDescending(s => s.PKM); break;
                    case "GW": orderableStats = groupedStats.OrderByDescending(s => s.GW); break;
                    case "GT": orderableStats = groupedStats.OrderByDescending(s => s.GT); break;
                    case "EG": orderableStats = groupedStats.OrderByDescending(s => s.EG); break;
                    case "HT": orderableStats = groupedStats.OrderByDescending(s => s.HT); break;
                    case "FOW": orderableStats = groupedStats.OrderByDescending(s => s.FOW); break;
                    case "FOT": orderableStats = groupedStats.OrderByDescending(s => s.FOT); break;
                    case "PSG": orderableStats = groupedStats.OrderByDescending(s => s.PSG); break;
                    case "PSS": orderableStats = groupedStats.OrderByDescending(s => s.PSS); break;
                    case "FW": orderableStats = groupedStats.OrderByDescending(s => s.FW); break;
                    case "FL": orderableStats = groupedStats.OrderByDescending(s => s.FL); break;
                    case "FT": orderableStats = groupedStats.OrderByDescending(s => s.FT); break;
                    case "S1": orderableStats = groupedStats.OrderByDescending(s => s.S1); break;
                    case "S2": orderableStats = groupedStats.OrderByDescending(s => s.S2); break;
                    case "S3": orderableStats = groupedStats.OrderByDescending(s => s.S3); break;
                    default:
                        {
                            AlertMessage = "Column does not exist.";
                            goto case "P";
                        }
                }
            }
            else
            {
                switch (column)
                {
                    case "GP": orderableStats = groupedStats.OrderBy(s => s.GP); break;
                    case "G": orderableStats = groupedStats.OrderBy(s => s.G); break;
                    case "A": orderableStats = groupedStats.OrderBy(s => s.A); break;
                    case "P": orderableStats = groupedStats.OrderBy(s => s.P).ThenByDescending(s => s.G); break;
                    case "PLMI": orderableStats = groupedStats.OrderBy(s => s.PLMI); break;
                    case "SHT": orderableStats = groupedStats.OrderBy(s => s.SHT); break;
                    case "SB": orderableStats = groupedStats.OrderBy(s => s.SB); break;
                    case "MP": orderableStats = groupedStats.OrderBy(s => s.MP); break;
                    case "PIM": orderableStats = groupedStats.OrderBy(s => s.PIM); break;
                    case "PM5": orderableStats = groupedStats.OrderBy(s => s.PM5); break;
                    case "HIT": orderableStats = groupedStats.OrderBy(s => s.HIT); break;
                    case "HTT": orderableStats = groupedStats.OrderBy(s => s.HTT); break;
                    case "PPG": orderableStats = groupedStats.OrderBy(s => s.PPG); break;
                    case "PPA": orderableStats = groupedStats.OrderBy(s => s.PPA); break;
                    case "PPP": orderableStats = groupedStats.OrderBy(s => s.PPP); break;
                    case "PPS": orderableStats = groupedStats.OrderBy(s => s.PPS); break;
                    case "PPM": orderableStats = groupedStats.OrderBy(s => s.PPM); break;
                    case "PKG": orderableStats = groupedStats.OrderBy(s => s.PKG); break;
                    case "PKA": orderableStats = groupedStats.OrderBy(s => s.PKA); break;
                    case "PKP": orderableStats = groupedStats.OrderBy(s => s.PKP); break;
                    case "PKS": orderableStats = groupedStats.OrderBy(s => s.PKS); break;
                    case "PKM": orderableStats = groupedStats.OrderBy(s => s.PKM); break;
                    case "GW": orderableStats = groupedStats.OrderBy(s => s.GW); break;
                    case "GT": orderableStats = groupedStats.OrderBy(s => s.GT); break;
                    case "EG": orderableStats = groupedStats.OrderBy(s => s.EG); break;
                    case "HT": orderableStats = groupedStats.OrderBy(s => s.HT); break;
                    case "FOW": orderableStats = groupedStats.OrderBy(s => s.FOW); break;
                    case "FOT": orderableStats = groupedStats.OrderBy(s => s.FOT); break;
                    case "PSG": orderableStats = groupedStats.OrderBy(s => s.PSG); break;
                    case "PSS": orderableStats = groupedStats.OrderBy(s => s.PSS); break;
                    case "FW": orderableStats = groupedStats.OrderBy(s => s.FW); break;
                    case "FL": orderableStats = groupedStats.OrderBy(s => s.FL); break;
                    case "FT": orderableStats = groupedStats.OrderBy(s => s.FT); break;
                    case "S1": orderableStats = groupedStats.OrderBy(s => s.S1); break;
                    case "S2": orderableStats = groupedStats.OrderBy(s => s.S2); break;
                    case "S3": orderableStats = groupedStats.OrderBy(s => s.S3); break;
                    default:
                        {
                            AlertMessage = "Column does not exist.";
                            goto case "P";
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

        protected override string PageName { get { return "Skater"; } }

        private IQueryable<SkaterCareerStatsDto> GetSkaterStatsQuery()
        {
            var stats = _database.SkaterSeasonStats
                .Where(sss => sss.Season.LeagueId == SelectedLeague.Id);

            if (SelectedSeasonType != null)
                stats = stats.Where(sss => SelectedSeasonType.Id == sss.Season.SeasonTypeId);

            if (SelectedTeam != null)
            {
                // Filters by team, the only gets either the main total stats or the traded subtotal stats.
                stats = stats
                    .Where(sss => sss.TeamId == SelectedTeam.Id);
                   // .GroupBy(
                   //     x => new { x.SkaterId, x.SeasonId },
                   //     (a, b) => b.Where(c => b.Count() == 1 || c.IsSubtotal).FirstOrDefault()
                   //);
            }
            else
            {
                stats = stats
                    .Where(sss => !sss.IsSubtotal);
            }

            var results = stats.GroupBy(
                x => new { x.SkaterId },
                (a, b) => new SkaterCareerStatsDto
                {
                    SkaterId = a.SkaterId,
                    SeasonCount = b.Select(s => s.SeasonId).Distinct().Count(),
                    TeamCount = b.Select(t => t.TeamId).Distinct().Count(),
                    GP = b.Sum(s => s.GP),
                    G = b.Sum(s => s.G),
                    A = b.Sum(s => s.A),
                    P = b.Sum(s => s.P),
                    PLMI = b.Sum(s => s.PLMI),
                    SHT = b.Sum(s => s.SHT),
                    SB = b.Sum(s => s.SB),
                    MP = b.Sum(s => s.MP),
                    PIM = b.Sum(s => s.PIM),
                    PM5 = b.Sum(s => s.PM5),
                    HIT = b.Sum(s => s.HIT),
                    HTT = b.Sum(s => s.HTT),
                    PPG = b.Sum(s => s.PPG),
                    PPA = b.Sum(s => s.PPA),
                    PPP = b.Sum(s => s.PPP),
                    PPS = b.Sum(s => s.PPS),
                    PPM = b.Sum(s => s.PPM),
                    PKG = b.Sum(s => s.PKG),
                    PKA = b.Sum(s => s.PKA),
                    PKP = b.Sum(s => s.PKP),
                    PKS = b.Sum(s => s.PKS),
                    PKM = b.Sum(s => s.PKM),
                    GW = b.Sum(s => s.GW),
                    GT = b.Sum(s => s.GT),
                    EG = b.Sum(s => s.EG),
                    HT = b.Sum(s => s.HT),
                    FOW = b.Sum(s => s.FOW),
                    FOT = b.Sum(s => s.FOT),
                    PSG = b.Sum(s => s.PSG),
                    PSS = b.Sum(s => s.PSS),
                    FW = b.Sum(s => s.FW),
                    FL = b.Sum(s => s.FL),
                    FT = b.Sum(s => s.FT),
                    S1 = b.Sum(s => s.S1),
                    S2 = b.Sum(s => s.S2),
                    S3 = b.Sum(s => s.S3),
                });

            return results;
        }

        private int GetSkaterStatsCount()
        {
            var stats = _database.SkaterSeasonStats
                .Where(sss => sss.Season.LeagueId == SelectedLeague.Id);

            if (SelectedSeasonType != null)
                stats = stats.Where(sss => SelectedSeasonType.Id == sss.Season.SeasonTypeId);

            return stats.Select(a => a.SkaterId).Distinct().Count();
        }

        private void SetColumnHeaders()
        {
            List<ColumnHeader> columnHeaders = new List<ColumnHeader>();
            bool isSpacer = true;
            columnHeaders.Add(new ColumnHeader("GP", "GP"));
            columnHeaders.Add(new ColumnHeader("G", "G"));
            columnHeaders.Add(new ColumnHeader("A", "A"));
            columnHeaders.Add(new ColumnHeader("P", "P"));
            columnHeaders.Add(new ColumnHeader("PLMI", "+/-"));
            columnHeaders.Add(new ColumnHeader("SHT", "SHT"));
            columnHeaders.Add(new ColumnHeader("SB", "SB"));
            columnHeaders.Add(new ColumnHeader("MP", "MP"));
            columnHeaders.Add(new ColumnHeader("PIM", "PIM"));
            columnHeaders.Add(new ColumnHeader("PM5", "PM5"));
            columnHeaders.Add(new ColumnHeader("HIT", "HIT", isSpacer));
            columnHeaders.Add(new ColumnHeader("HTT", "HTT"));
            columnHeaders.Add(new ColumnHeader("PPG", "PPG", isSpacer));
            columnHeaders.Add(new ColumnHeader("PPA", "PPA"));
            columnHeaders.Add(new ColumnHeader("PPP", "PPP"));
            columnHeaders.Add(new ColumnHeader("PPS", "PPS"));
            columnHeaders.Add(new ColumnHeader("PPM", "PPM"));
            columnHeaders.Add(new ColumnHeader("PKG", "PKG", isSpacer));
            columnHeaders.Add(new ColumnHeader("PKA", "PKA"));
            columnHeaders.Add(new ColumnHeader("PKP", "PKP"));
            columnHeaders.Add(new ColumnHeader("PKS", "PKS"));
            columnHeaders.Add(new ColumnHeader("PKM", "PKM"));
            columnHeaders.Add(new ColumnHeader("GW", "GW", isSpacer));
            columnHeaders.Add(new ColumnHeader("GT", "GT"));
            columnHeaders.Add(new ColumnHeader("EG", "EG"));
            columnHeaders.Add(new ColumnHeader("HT", "HT"));
            columnHeaders.Add(new ColumnHeader("FOW", "FOW", isSpacer));
            columnHeaders.Add(new ColumnHeader("FOT", "FOT"));
            columnHeaders.Add(new ColumnHeader("PSG", "PSG", isSpacer));
            columnHeaders.Add(new ColumnHeader("PSS", "PSS"));
            columnHeaders.Add(new ColumnHeader("FW", "FW", isSpacer));
            columnHeaders.Add(new ColumnHeader("FL", "FL"));
            columnHeaders.Add(new ColumnHeader("FT", "FT"));
            columnHeaders.Add(new ColumnHeader("S1", "S1", isSpacer));
            columnHeaders.Add(new ColumnHeader("S2", "S2"));
            columnHeaders.Add(new ColumnHeader("S3", "S3"));
            ColumnHeaders = columnHeaders;
        }

    }
}