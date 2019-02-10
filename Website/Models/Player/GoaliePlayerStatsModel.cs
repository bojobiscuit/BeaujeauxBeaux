using DataEF;
using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
    public class GoaliePlayerStatsModel : PlayerStatsModel
    {
        public Goalie Goalie { get; set; }
        public IEnumerable<GoalieStatGroup> GroupedStats { get; set; }

        public GoaliePlayerStatsModel(BeaujeauxEntities database, Goalie goalie, int leagueId) : base (database, leagueId)
        {
            Goalie = goalie;

            var seasonTypes = Goalie.GoalieSeasonStats
                .Select(sss => sss.Season.SeasonType)
                .Distinct()
                .ToList();

            var statGroups = new List<GoalieStatGroup>();
            foreach (var type in seasonTypes)
            {
                var stats = database.GoalieSeasonStats
                    .Where(sss => sss.Season.SeasonTypeId == type.Id)
                    .Where(sss => sss.Season.LeagueId == leagueId)
                    .Where(sss => sss.GoalieId == Goalie.Id)
                    .Where(sss => !sss.IsSubtotal)
                    .OrderByDescending(sss => sss.Season.Number)
                    .ToList();

                GoalieSeasonStat totals = new GoalieSeasonStat()
                {
                    GP = stats.Sum(a => a.GP),
                    W = stats.Sum(a => a.W),
                    L = stats.Sum(a => a.L),
                    OTL = stats.Sum(a => a.OTL),
                    MP = stats.Sum(a => a.MP),
                    PIM = stats.Sum(a => a.PIM),
                    SO = stats.Sum(a => a.SO),
                    A = stats.Sum(a => a.A),
                    EG = stats.Sum(a => a.EG),
                    GA = stats.Sum(a => a.GA),
                    SA = stats.Sum(a => a.SA),
                    PSS = stats.Sum(a => a.PSS),
                    PSA = stats.Sum(a => a.PSA),
                    ST = stats.Sum(a => a.ST),
                    BG = stats.Sum(a => a.BG),
                    S1 = stats.Sum(a => a.S1),
                    S2 = stats.Sum(a => a.S2),
                    S3 = stats.Sum(a => a.S3),
                };

                statGroups.Add(new GoalieStatGroup()
                {
                    SeasonType = type,
                    Stats = stats,
                    TotalStats = totals,
                });
            }
            GroupedStats = statGroups;
        }

        protected override string PageName => "Goalie";

        protected override int PlayerId => Goalie != null ? Goalie.Id : 0;
    }

    public class GoalieStatGroup
    {
        public SeasonType SeasonType { get; set; }
        public IEnumerable<GoalieSeasonStat> Stats { get; set; }
        public GoalieSeasonStat TotalStats { get; set; }
    }
}