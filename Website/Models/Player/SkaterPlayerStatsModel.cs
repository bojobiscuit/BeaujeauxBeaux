using DataEF;
using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
    public class SkaterPlayerStatsModel : PlayerStatsModel
    {
        public Skater Skater { get; set; }
        public IEnumerable<SkaterStatGroup> GroupedStats { get; set; }

        public SkaterPlayerStatsModel(BeaujeauxEntities database, Skater skater, int leagueId) : base(database, leagueId)
        {
            Skater = skater;

            var seasonTypes = Skater.SkaterSeasonStats
                .Select(sss => sss.Season.SeasonType)
                .Distinct()
                .OrderBy(st => st.Id)
                .ToList();

            var statGroups = new List<SkaterStatGroup>();
            foreach (var type in seasonTypes)
            {
                var stats = database.SkaterSeasonStats
                    .Where(sss => sss.Season.SeasonTypeId == type.Id)
                    .Where(sss => sss.Season.LeagueId == leagueId)
                    .Where(sss => sss.SkaterId == Skater.Id)
                    .Where(sss => !sss.IsSubtotal)
                    .OrderByDescending(sss => sss.Season.Number)
                    .ToList();

                SkaterSeasonStat totals = new SkaterSeasonStat()
                {
                    GP = stats.Sum(a => a.GP),
                    G = stats.Sum(a => a.G),
                    A = stats.Sum(a => a.A),
                    P = stats.Sum(a => a.P),
                    PLMI = stats.Sum(a => a.PLMI),
                    SHT = stats.Sum(a => a.SHT),
                    SB = stats.Sum(a => a.SB),
                    MP = stats.Sum(a => a.MP),
                    PIM = stats.Sum(a => a.PIM),
                    PM5 = stats.Sum(a => a.PM5),
                    HIT = stats.Sum(a => a.HIT),
                    HTT = stats.Sum(a => a.HTT),
                    PPG = stats.Sum(a => a.PPG),
                    PPA = stats.Sum(a => a.PPA),
                    PPP = stats.Sum(a => a.PPP),
                    PPS = stats.Sum(a => a.PPS),
                    PPM = stats.Sum(a => a.PPM),
                    PKG = stats.Sum(a => a.PKG),
                    PKA = stats.Sum(a => a.PKA),
                    PKP = stats.Sum(a => a.PKP),
                    PKS = stats.Sum(a => a.PKS),
                    PKM = stats.Sum(a => a.PKM),
                    GW = stats.Sum(a => a.GW),
                    GT = stats.Sum(a => a.GT),
                    EG = stats.Sum(a => a.EG),
                    HT = stats.Sum(a => a.HT),
                    FOW = stats.Sum(a => a.FOW),
                    FOT = stats.Sum(a => a.FOT),
                    PSG = stats.Sum(a => a.PSG),
                    PSS = stats.Sum(a => a.PSS),
                    FW = stats.Sum(a => a.FW),
                    FL = stats.Sum(a => a.FL),
                    FT = stats.Sum(a => a.FT),
                    S1 = stats.Sum(a => a.S1),
                    S2 = stats.Sum(a => a.S2),
                    S3 = stats.Sum(a => a.S3),
                };

                statGroups.Add(new SkaterStatGroup()
                {
                    SeasonType = type,
                    Stats = stats,
                    TotalStats = totals,
                });
            }
            GroupedStats = statGroups;
        }

        protected override string PageName => "Skater";
        protected override int PlayerId => Skater != null ? Skater.Id : 0;
    }

    public class SkaterStatGroup
    {
        public SeasonType SeasonType { get; set; }
        public IEnumerable<SkaterSeasonStat> Stats { get; set; }
        public SkaterSeasonStat TotalStats { get; set; }
    }

}