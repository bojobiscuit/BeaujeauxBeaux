using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SthsStatsToDB
{
    internal class TeamData
    {

        public TeamData(BeaujeauxEntities database, SthsData.Team source, DataEF.Team db)
        {
            Database = database;
            DataSource = source;
            DataDB = db;
        }

        public void SetSkatersAndStats(DataEF.Season dbSeason)
        {
            foreach (var sourceSkater in DataSource.Skaters)
            {
                var dbSkater = GetSkaterFromDB(sourceSkater);
                SetSkaterStats(dbSeason, dbSkater, sourceSkater.SeasonTotals);

                // If the team was traded, add the subtotal seasons as well
                if (sourceSkater.SeasonSubTotals != null)
                {
                    foreach (var subTotal in sourceSkater.SeasonSubTotals)
                        SetSubtotalSkaterStats(dbSeason, dbSkater, subTotal);
                }
            }
        }

        public void SetGoaliesAndStats(DataEF.Season dbSeason)
        {
            foreach (var sourceGoalie in DataSource.Goalies)
            {
                var dbGoalie = GetGoalieFromDB(sourceGoalie);
                SetGoalieStats(dbSeason, dbGoalie, sourceGoalie.SeasonTotals);

                // If the team was traded, add the subtotal seasons as well
                if (sourceGoalie.SeasonSubTotals != null)
                {
                    foreach (var subTotal in sourceGoalie.SeasonSubTotals)
                        SetSubtotalGoalieStats(dbSeason, dbGoalie, subTotal);
                }
            }
        }


        private DataEF.Skater GetSkaterFromDB(SthsData.Skater sourceSkater)
        {
            var dbSkater = Database.Skaters
                .Where(p => p.Name == sourceSkater.Name)
                .FirstOrDefault();

            if (dbSkater == null)
            {
                dbSkater = new DataEF.Skater()
                {
                    Name = sourceSkater.Name,
                };
                Database.Skaters.Add(dbSkater);
            }
            return dbSkater;
        }

        private DataEF.SkaterSeasonStat SetSkaterStats(DataEF.Season dbSeason, DataEF.Skater dbSkater, SthsData.SkaterSeasonStats skaterStats)
        {
            var dbSeasonStats = new DataEF.SkaterSeasonStat()
            {
                Season = dbSeason,
                Skater = dbSkater,
                Team = DataDB,
            };

            PopulateSkaterStats(dbSeasonStats, skaterStats);
            Database.SkaterSeasonStats.Add(dbSeasonStats);
            return dbSeasonStats;
        }

        private DataEF.SkaterSeasonStat SetSubtotalSkaterStats(DataEF.Season dbSeason, DataEF.Skater dbSkater, SthsData.SkaterSeasonStats skaterStats)
        {
            var dbSeasonSubtotalStats = new DataEF.SkaterSeasonStat()
            {
                Season = dbSeason,
                Skater = dbSkater,
                Team = dbSeason.Teams.Where(a => a.Acronym == skaterStats.TeamAcronym).First(),
                IsSubtotal = true
            };

            PopulateSkaterStats(dbSeasonSubtotalStats, skaterStats);
            Database.SkaterSeasonStats.Add(dbSeasonSubtotalStats);
            return dbSeasonSubtotalStats;
        }

        private static void PopulateSkaterStats(DataEF.SkaterSeasonStat dbSeasonStats, SthsData.SkaterSeasonStats skaterStats)
        {
            dbSeasonStats.GP = skaterStats.GP;
            dbSeasonStats.G = skaterStats.G;
            dbSeasonStats.A = skaterStats.A;
            dbSeasonStats.P = skaterStats.P;
            dbSeasonStats.PLMI = skaterStats.PLMI;
            dbSeasonStats.PIM = skaterStats.PIM;
            dbSeasonStats.PM5 = skaterStats.PM5;
            dbSeasonStats.HIT = skaterStats.HIT;
            dbSeasonStats.HTT = skaterStats.HTT;
            dbSeasonStats.SHT = skaterStats.SHT;
            dbSeasonStats.OSB = skaterStats.OSB;
            dbSeasonStats.OSM = skaterStats.OSM;
            dbSeasonStats.SB = skaterStats.SB;
            dbSeasonStats.MP = skaterStats.MP;
            dbSeasonStats.PPG = skaterStats.PPG;
            dbSeasonStats.PPA = skaterStats.PPA;
            dbSeasonStats.PPP = skaterStats.PPP;
            dbSeasonStats.PPS = skaterStats.PPS;
            dbSeasonStats.PPM = skaterStats.PPM;
            dbSeasonStats.PKG = skaterStats.PKG;
            dbSeasonStats.PKA = skaterStats.PKA;
            dbSeasonStats.PKP = skaterStats.PKP;
            dbSeasonStats.PKS = skaterStats.PKS;
            dbSeasonStats.PKM = skaterStats.PKM;
            dbSeasonStats.GW = skaterStats.GW;
            dbSeasonStats.GT = skaterStats.GT;
            dbSeasonStats.FOW = skaterStats.FOW;
            dbSeasonStats.FOT = skaterStats.FOT;
            dbSeasonStats.GA = skaterStats.GA;
            dbSeasonStats.TA = skaterStats.TA;
            dbSeasonStats.EG = skaterStats.EG;
            dbSeasonStats.HT = skaterStats.HT;
            dbSeasonStats.PSG = skaterStats.PSG;
            dbSeasonStats.PSS = skaterStats.PSS;
            dbSeasonStats.FW = skaterStats.FW;
            dbSeasonStats.FL = skaterStats.FL;
            dbSeasonStats.FT = skaterStats.FT;
            dbSeasonStats.GS = skaterStats.GS;
            dbSeasonStats.PS = skaterStats.PS;
            dbSeasonStats.WG = skaterStats.WG;
            dbSeasonStats.WP = skaterStats.WP;
            dbSeasonStats.S1 = skaterStats.S1;
            dbSeasonStats.S2 = skaterStats.S2;
            dbSeasonStats.S3 = skaterStats.S3;
        }


        private DataEF.Goalie GetGoalieFromDB(SthsData.Goalie goalie)
        {
            var dbGoalie = Database.Goalies
                .Where(p => p.Name == goalie.Name)
                .FirstOrDefault();

            if (dbGoalie == null)
            {
                dbGoalie = new DataEF.Goalie()
                {
                    Name = goalie.Name,
                };
                Database.Goalies.Add(dbGoalie);
            }
            return dbGoalie;
        }

        private DataEF.GoalieSeasonStat SetGoalieStats(DataEF.Season dbSeason, DataEF.Goalie dbGoalie, SthsData.GoalieSeasonStats goalieStats)
        {
            var dbSeasonStats = new DataEF.GoalieSeasonStat()
            {
                Season = dbSeason,
                Goalie = dbGoalie,
                Team = DataDB,
            };

            PopulateGoalieStats(dbSeasonStats, goalieStats);
            Database.GoalieSeasonStats.Add(dbSeasonStats);
            return dbSeasonStats;
        }

        private DataEF.GoalieSeasonStat SetSubtotalGoalieStats(DataEF.Season dbSeason, DataEF.Goalie dbGoalie, SthsData.GoalieSeasonStats goalieStats)
        {
            var dbSeasonStats = new DataEF.GoalieSeasonStat()
            {
                Season = dbSeason,
                Goalie = dbGoalie,
                Team = dbSeason.Teams.Where(a => a.Acronym == goalieStats.TeamAcronym).First(),
                IsSubtotal = true,
            };

            PopulateGoalieStats(dbSeasonStats, goalieStats);
            Database.GoalieSeasonStats.Add(dbSeasonStats);
            return dbSeasonStats;
        }

        private static void PopulateGoalieStats(DataEF.GoalieSeasonStat dbSeasonStats, SthsData.GoalieSeasonStats goalieStats)
        {
            dbSeasonStats.GP = goalieStats.GP;
            dbSeasonStats.W = goalieStats.W;
            dbSeasonStats.L = goalieStats.L;
            dbSeasonStats.OTL = goalieStats.OTL;
            dbSeasonStats.MP = goalieStats.MP;
            dbSeasonStats.PIM = goalieStats.PIM;
            dbSeasonStats.SO = goalieStats.SO;
            dbSeasonStats.GA = goalieStats.GA;
            dbSeasonStats.SA = goalieStats.SA;
            dbSeasonStats.SAR = goalieStats.SAR;
            dbSeasonStats.A = goalieStats.A;
            dbSeasonStats.EG = goalieStats.EG;
            dbSeasonStats.PSS = goalieStats.PSS;
            dbSeasonStats.PSA = goalieStats.PSA;
            dbSeasonStats.ST = goalieStats.ST;
            dbSeasonStats.BG = goalieStats.BG;
            dbSeasonStats.S1 = goalieStats.S1;
            dbSeasonStats.S2 = goalieStats.S2;
            dbSeasonStats.S3 = goalieStats.S3;
        }



        private BeaujeauxEntities Database { get; set; }
        private SthsData.Team DataSource { get; set; }
        private DataEF.Team DataDB { get; set; }

    }
}
