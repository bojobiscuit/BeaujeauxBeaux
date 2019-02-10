using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class SkaterSeasonStatsModel : SeasonStatsModel
    {
        public IEnumerable<SkaterSeasonStat> Stats { get; set; }

        public SkaterSeasonStatsModel(SeasonStatsParameters seasonParameters) : base(seasonParameters)
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
            return (int)Math.Ceiling(GetSkaterStatsQuery().Count() / (decimal)PageSize);
        }

        protected override void SetStatsForPage()
        {
            SetColumnHeaders();
            Stats = SortBy(GetSkaterStatsQuery(), _seasonStatsParameters.sortOrder, _seasonStatsParameters.sortDescending)
                .Skip(PageIndex * PageSize)
                .Take(PageSize)
                .ToList();
        }

        protected override string PageName { get { return "Skater"; } }

        private IQueryable<SkaterSeasonStat> GetSkaterStatsQuery()
        {
            var stats = _database.SkaterSeasonStats
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
                    stats = stats.Where(sss => sss.Season.Number < 12);
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

        private IQueryable<SkaterSeasonStat> SortBy(IQueryable<SkaterSeasonStat> stats, string column, bool sortDesc)
        {
            if (column == null)
                column = "P";

            IsSortDescending = sortDesc;

            IOrderedQueryable<SkaterSeasonStat> orderableStats;
            if (IsSortDescending)
            {
                switch (column)
                {
                    case "GP": orderableStats = stats.OrderByDescending(s => s.GP); break;
                    case "G": orderableStats = stats.OrderByDescending(s => s.G); break;
                    case "A": orderableStats = stats.OrderByDescending(s => s.A); break;
                    case "P": orderableStats = stats.OrderByDescending(s => s.P).ThenByDescending(s => s.G); break;
                    case "PLMI": orderableStats = stats.OrderByDescending(s => s.PLMI); break;
                    case "SHT": orderableStats = stats.OrderByDescending(s => s.SHT); break;
                    case "SB": orderableStats = stats.OrderByDescending(s => s.SB); break;
                    case "MP": orderableStats = stats.OrderByDescending(s => s.MP); break;
                    case "PIM": orderableStats = stats.OrderByDescending(s => s.PIM); break;
                    case "PM5": orderableStats = stats.OrderByDescending(s => s.PM5); break;
                    case "HIT": orderableStats = stats.OrderByDescending(s => s.HIT); break;
                    case "HTT": orderableStats = stats.OrderByDescending(s => s.HTT); break;
                    case "PPG": orderableStats = stats.OrderByDescending(s => s.PPG); break;
                    case "PPA": orderableStats = stats.OrderByDescending(s => s.PPA); break;
                    case "PPP": orderableStats = stats.OrderByDescending(s => s.PPP); break;
                    case "PPS": orderableStats = stats.OrderByDescending(s => s.PPS); break;
                    case "PPM": orderableStats = stats.OrderByDescending(s => s.PPM); break;
                    case "PKG": orderableStats = stats.OrderByDescending(s => s.PKG); break;
                    case "PKA": orderableStats = stats.OrderByDescending(s => s.PKA); break;
                    case "PKP": orderableStats = stats.OrderByDescending(s => s.PKP); break;
                    case "PKS": orderableStats = stats.OrderByDescending(s => s.PKS); break;
                    case "PKM": orderableStats = stats.OrderByDescending(s => s.PKM); break;
                    case "GW": orderableStats = stats.OrderByDescending(s => s.GW); break;
                    case "GT": orderableStats = stats.OrderByDescending(s => s.GT); break;
                    case "EG": orderableStats = stats.OrderByDescending(s => s.EG); break;
                    case "HT": orderableStats = stats.OrderByDescending(s => s.HT); break;
                    case "FOW": orderableStats = stats.OrderByDescending(s => s.FOW); break;
                    case "FOT": orderableStats = stats.OrderByDescending(s => s.FOT); break;
                    case "PSG": orderableStats = stats.OrderByDescending(s => s.PSG); break;
                    case "PSS": orderableStats = stats.OrderByDescending(s => s.PSS); break;
                    case "FW": orderableStats = stats.OrderByDescending(s => s.FW); break;
                    case "FL": orderableStats = stats.OrderByDescending(s => s.FL); break;
                    case "FT": orderableStats = stats.OrderByDescending(s => s.FT); break;
                    case "S1": orderableStats = stats.OrderByDescending(s => s.S1); break;
                    case "S2": orderableStats = stats.OrderByDescending(s => s.S2); break;
                    case "S3": orderableStats = stats.OrderByDescending(s => s.S3); break;
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
                    case "GP": orderableStats = stats.OrderBy(s => s.GP); break;
                    case "G": orderableStats = stats.OrderBy(s => s.G); break;
                    case "A": orderableStats = stats.OrderBy(s => s.A); break;
                    case "P": orderableStats = stats.OrderBy(s => s.P).ThenByDescending(s => s.G); break;
                    case "PLMI": orderableStats = stats.OrderBy(s => s.PLMI); break;
                    case "SHT": orderableStats = stats.OrderBy(s => s.SHT); break;
                    case "SB": orderableStats = stats.OrderBy(s => s.SB); break;
                    case "MP": orderableStats = stats.OrderBy(s => s.MP); break;
                    case "PIM": orderableStats = stats.OrderBy(s => s.PIM); break;
                    case "PM5": orderableStats = stats.OrderBy(s => s.PM5); break;
                    case "HIT": orderableStats = stats.OrderBy(s => s.HIT); break;
                    case "HTT": orderableStats = stats.OrderBy(s => s.HTT); break;
                    case "PPG": orderableStats = stats.OrderBy(s => s.PPG); break;
                    case "PPA": orderableStats = stats.OrderBy(s => s.PPA); break;
                    case "PPP": orderableStats = stats.OrderBy(s => s.PPP); break;
                    case "PPS": orderableStats = stats.OrderBy(s => s.PPS); break;
                    case "PPM": orderableStats = stats.OrderBy(s => s.PPM); break;
                    case "PKG": orderableStats = stats.OrderBy(s => s.PKG); break;
                    case "PKA": orderableStats = stats.OrderBy(s => s.PKA); break;
                    case "PKP": orderableStats = stats.OrderBy(s => s.PKP); break;
                    case "PKS": orderableStats = stats.OrderBy(s => s.PKS); break;
                    case "PKM": orderableStats = stats.OrderBy(s => s.PKM); break;
                    case "GW": orderableStats = stats.OrderBy(s => s.GW); break;
                    case "GT": orderableStats = stats.OrderBy(s => s.GT); break;
                    case "EG": orderableStats = stats.OrderBy(s => s.EG); break;
                    case "HT": orderableStats = stats.OrderBy(s => s.HT); break;
                    case "FOW": orderableStats = stats.OrderBy(s => s.FOW); break;
                    case "FOT": orderableStats = stats.OrderBy(s => s.FOT); break;
                    case "PSG": orderableStats = stats.OrderBy(s => s.PSG); break;
                    case "PSS": orderableStats = stats.OrderBy(s => s.PSS); break;
                    case "FW": orderableStats = stats.OrderBy(s => s.FW); break;
                    case "FL": orderableStats = stats.OrderBy(s => s.FL); break;
                    case "FT": orderableStats = stats.OrderBy(s => s.FT); break;
                    case "S1": orderableStats = stats.OrderBy(s => s.S1); break;
                    case "S2": orderableStats = stats.OrderBy(s => s.S2); break;
                    case "S3": orderableStats = stats.OrderBy(s => s.S3); break;
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

    }
}