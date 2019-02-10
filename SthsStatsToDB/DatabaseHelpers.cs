using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SthsStatsToDB
{
    public static class DatabaseHelpers
    {
        public static void AddRequestTypes()
        {
            BeaujeauxEntities database = new BeaujeauxEntities();

            if (database.RequestTypes.Any())
                return;

            database.RequestTypes.Add(new RequestType()
            {
                Name = "Bug",
                Display = "Something's Broken",
                Rank = 1
            });
            database.RequestTypes.Add(new RequestType()
            {
                Name = "Data",
                Display = "Data is Wrong",
                Rank = 2
            });
            database.RequestTypes.Add(new RequestType()
            {
                Name = "Feature",
                Display = "Feature Request",
                Rank = 3
            });
            database.RequestTypes.Add(new RequestType()
            {
                Name = "General",
                Display = "Something else",
                Rank = 4
            });
            database.SaveChanges();
        }

        public static void AddFranchises()
        {
            BeaujeauxEntities database = new BeaujeauxEntities();
            database.Franchises.RemoveRange(database.Franchises);

            string[] latestTeamsShl = {
                "Buffalo Stampede",
                "Calgary Dragons",
                "Edmonton Blizzard",
                "Hamilton Steelhawks",
                "Manhattan Rage",
                "Toronto North Stars",
                "West Kendall Platoon",
                "Winnipeg Jets",
                "Los Angeles Panthers",
                "Minnesota Chiefs",
                "Texas Renegades",
                "New England Wolfpack",
                "Seattle Riot",
                "San Francisco Pride"
            };

            string[] latestTeamsSmjhl = {
                "Detroit Falcons",
                "Montreal Militia",
                "Prince George Firebirds",
                "Kelowna Knights",
                "St Louis Scarecrows",
                "Vancouver Whalers",
                "Colorado Mammoths",
                "Halifax Raiders",
            };

            League shl = database.Leagues.Where(a => a.Acronym == "SHL").First();
            League smjhl = database.Leagues.Where(a => a.Acronym == "SMJHL").First();

            foreach (var teamName in latestTeamsShl)
            {
                var team = database.Teams.Where(t => t.Name == teamName).FirstOrDefault();
                if (team == null)
                    continue;

                Franchise franchise = new Franchise()
                {
                    LatestTeam = team,
                    League = shl,
                };

                team.Franchise = franchise;

                if (team.Acronym == "NEW")
                {
                    var hydras = database.Teams.Where(t => t.Name == "Hartford Hydras").FirstOrDefault();
                    franchise.Teams.Add(hydras);
                }
                if (team.Acronym == "SEA")
                {
                    var nightmare = database.Teams.Where(t => t.Name == "Vancouver Nightmare").FirstOrDefault();
                    var lvkings = database.Teams.Where(t => t.Name == "Las Vegas Kings").FirstOrDefault();
                    var wolves = database.Teams.Where(t => t.Name == "Vancouver Ice Wolves").FirstOrDefault();
                    franchise.Teams.Add(nightmare);
                    franchise.Teams.Add(lvkings);
                    franchise.Teams.Add(wolves);
                }
                if (team.Acronym == "SFP")
                {
                    var port = database.Teams.Where(t => t.Name == "Portland Admirals").FirstOrDefault();
                    franchise.Teams.Add(port);
                }
                if (team.Acronym == "EDM")
                {
                    var port = database.Teams.Where(t => t.Name == "Edmonton Comets").FirstOrDefault();
                    franchise.Teams.Add(port);
                }

                database.Franchises.Add(franchise);
            }

            foreach (var teamName in latestTeamsSmjhl)
            {
                var team = database.Teams.Where(t => t.Name == teamName).FirstOrDefault();
                if (team == null)
                    continue;

                Franchise franchise = new Franchise()
                {
                    LatestTeam = team,
                    League = smjhl,
                };

                team.Franchise = franchise;

                if (team.Acronym == "MTL")
                {
                    var impact = database.Teams.Where(t => t.Name == "Montreal Impact").FirstOrDefault();
                    franchise.Teams.Add(impact);
                }

                if (team.Acronym == "PGF")
                {
                    var regina = database.Teams.Where(t => t.Name == "Regina Force").FirstOrDefault();
                    franchise.Teams.Add(regina);
                }

                database.Franchises.Add(franchise);
            }

            database.SaveChanges();
        }

        public static void AddLeagues()
        {
            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                if (!database.Leagues.Any())
                {
                    database.Leagues.Add(new League() { Acronym = "SHL", Name = "Simulated Hockey League" });
                    database.Leagues.Add(new League() { Acronym = "SMJHL", Name = "Simulated Major Junior Hockey League" });
                    database.SaveChanges();
                }
            }
        }

        public static void AddSeasonTypes()
        {
            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                if (!database.SeasonTypes.Any())
                {
                    database.SeasonTypes.Add(new SeasonType() { Name = "Regular Season" });
                    database.SeasonTypes.Add(new SeasonType() { Name = "Playoffs" });
                    database.SaveChanges();
                }
            }
        }

        public static void RemoveExtraPlayers()
        {
            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                var extraSkaters = database.Skaters
                    .Where(a => !a.SkaterSeasonStats.Any());
                database.Skaters.RemoveRange(extraSkaters);

                var extraGoalies = database.Goalies
                    .Where(a => !a.GoalieSeasonStats.Any());
                database.Goalies.RemoveRange(extraGoalies);

                database.SaveChanges();
            }
        }

    }
}
