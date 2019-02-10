using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SthsData;

namespace SthsStatsToDB
{
    public class SeasonData
    {
        public SthsData.Season SourceSeason { get; set; }

        public SeasonData(SthsData.Season season)
        {
            SourceSeason = season;
        }

        public void SaveDataToDB()
        {
            using (Database = new BeaujeauxEntities())
            {
                DeleteSeasonIfExists();
                PrepareClassData();
                AddStatsToTeams();
                RemoveStatsWithNoPlaying();
            }
        }

        private void DeleteSeasonIfExists()
        {
            SeasonType seasonType = Database.SeasonTypes.Where(st => st.Name == SourceSeason.Type).First();
            League league = Database.Leagues.Where(l => l.Acronym == SourceSeason.LeagueAcronym).First();

            var matchedSeasons = Database.Seasons
                .Where(a => a.Number == SourceSeason.Number)
                .Where(a => a.SeasonType.Id == seasonType.Id)
                .Where(a => a.League.Id == league.Id)
                .ToList();

            int seasonCount = matchedSeasons.Count();
            foreach (var dbSeason in matchedSeasons)
            {
                Database.SkaterSeasonStats.RemoveRange(dbSeason.SkaterSeasonStats);
                Database.GoalieSeasonStats.RemoveRange(dbSeason.GoalieSeasonStats);
                dbSeason.Teams.Clear();
                Database.Seasons.Remove(dbSeason);
            }

            if (seasonCount > 0)
                Database.SaveChanges();
        }

        private void PrepareClassData()
        {
            dbLeague = GetLeague();
            dbSeasonType = GetSeasonType();
            dbSeason = GetSeason();
            Teams = GetTeams();

            Database.SaveChanges();
        }

        private League GetLeague()
        {
            return Database.Leagues.Where(l => l.Acronym == SourceSeason.LeagueAcronym).First();
        }

        private SeasonType GetSeasonType()
        {
            return Database.SeasonTypes.Where(st => st.Name == SourceSeason.Type).First();
        }

        private DataEF.Season GetSeason()
        {
            var season = new DataEF.Season
            {
                Number = SourceSeason.Number,
                SeasonType = dbSeasonType,
                League = dbLeague,
            };
            Database.Seasons.Add(season);
            return season;
        }

        private IEnumerable<TeamData> GetTeams()
        {
            List<TeamData> teams = new List<TeamData>();
            foreach (var sourceTeam in SourceSeason.Teams)
            {
                var dbTeam = Database.Teams
                    .Where(t => t.Name == sourceTeam.Name)
                    .FirstOrDefault();

                // If the team doesn't exist, create and add one the the DB
                if (dbTeam == null)
                    dbTeam = GetTeam(sourceTeam);

                // Adds team to season if not already there.
                if (!dbSeason.Teams.Contains(dbTeam))
                    dbSeason.Teams.Add(dbTeam);

                teams.Add(new TeamData(Database, sourceTeam, dbTeam));
            }
            return teams;
        }

        private DataEF.Team GetTeam(SthsData.Team team)
        {
            DataEF.Team dbTeam = new DataEF.Team()
            {
                Acronym = team.Acronym,
                Name = team.Name,
            };
            Database.Teams.Add(dbTeam);
            return dbTeam;
        }

        private void AddStatsToTeams()
        {
            foreach (var team in Teams)
            {
                team.SetSkatersAndStats(dbSeason);
                team.SetGoaliesAndStats(dbSeason);
            }
            Database.SaveChanges();
        }

        private void RemoveStatsWithNoPlaying()
        {
            Database.SkaterSeasonStats.RemoveRange(
                Database.SkaterSeasonStats
                    .Where(a => a.SeasonId == dbSeason.Id)
                    .Where(a => a.MP < a.GP * 4)
                );

            Database.SaveChanges();
        }

        private BeaujeauxEntities Database { get; set; }
        private DataEF.League dbLeague { get; set; }
        private DataEF.SeasonType dbSeasonType { get; set; }
        private DataEF.Season dbSeason { get; set; }
        private IEnumerable<TeamData> Teams { get; set; }

    }
}
