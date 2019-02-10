using DataEF;
using ModernSthsStats;
using SthsData;
using SthsStatsToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSthsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int season = 45;
            string url;

            //SplitCareerSameName("John Langabeer", 32);
            //MergeIntoExistingSkater("John Langabeer II", "John Langabeer");
            //ChangeSkaterName("John Langabeer II", "John Langabeer");

            //url = "http://simulationhockey.com/games/shl/S{0}/Season/SHL-ProTeamScoring.html";
            //GetOneShlSeason(season, url, "SHL");

            //url = "http://simulationhockey.com/games/shl/S{0}/Playoff/SHL-PLF-ProTeamScoring.html";
            //GetOneShlSeason(season, url, "SHL");

            //url = "http://simulationhockey.com/games/smjhl/S{0}/Season/SMJHL-ProTeamScoring.html";
            //GetOneShlSeason(season, url, "SMJHL");

            //url = "http://simulationhockey.com/games/smjhl/S{0}/Playoffs/SMJHL-PLF-ProTeamScoring.html";
            //GetOneShlSeason(season, url, "SMJHL");

            //RemoveTeamIdForTotal();

            //GetEverything();
        }

        public static void GetOneShlSeason(int seasonNumber, string url, string leagueAcronym)
        {
            SeasonData season;

            bool isPlayoffs = url.Contains("-PLF-");

            var extractedSeason = SeasonStatsExtractor.ExtractSeason(url, seasonNumber, isPlayoffs, leagueAcronym);
            if (extractedSeason != null && extractedSeason.Teams.Count > 0)
            {
                season = new SeasonData(extractedSeason);
                Console.Write(" [DONE]");
                Console.WriteLine();
                Console.WriteLine("----");
                Console.WriteLine("Uploading Data to DB");
                Console.Write(" - Season " + season.SourceSeason.Number);
                season.SaveDataToDB();
                Console.Write(" [DONE]");
                Console.WriteLine();
            }
            else
            {
                Console.Write(" [Not Found]");
            }
            Console.WriteLine("----");
            Console.WriteLine("Extraction Complete");
            Console.WriteLine();
            Console.ReadKey();
        }

        public class SeasonPage
        {
            public string Url { get; set; }
            public string LeagueAcronym { get; set; }
            public int StartIndex { get; set; }
            public bool IsPlayoffs { get; set; }
        }


        public static void RemoveTeamIdForTotal()
        {
            using (var db = new BeaujeauxEntities())
            {
                var stats = db.SkaterSeasonStats
                    .GroupBy(
                        x => new { x.SkaterId, x.SeasonId },
                        (a, b) => b.Where(c => b.Count() > 1)
                   ).Where(a => a.Count() > 1).ToList();

                Console.WriteLine(stats.Count() + " people");
                Console.WriteLine();

                int z = 0;
                List<SkaterSeasonStat> allStats = new List<SkaterSeasonStat>();
                foreach(var season in stats)
                {
                    foreach(var statLine in season)
                    {
                        var stat = db.SkaterSeasonStats.First(a => a.Id == statLine.Id);
                        if (!stat.IsSubtotal)
                        {
                            stat.TeamId = null;
                            allStats.Add(stat);
                            break;
                        }
                    }
                    Console.Write(++z + " ");
                }

                db.SaveChanges();
            }
        }

        public static void MergeIntoExistingSkater(string oldName, string newName)
        {
            using (var db = new BeaujeauxEntities())
            {
                var skaters = db.Skaters.Where(a => a.Name == newName);
                if (skaters.Count() != 1)
                    throw new Exception("too many skaters");
                var newSkater = skaters.First();

                skaters = db.Skaters.Where(a => a.Name == oldName);
                if (skaters.Count() != 1)
                    throw new Exception("too many skaters");
                var oldSkater = skaters.First();

                foreach (var season in oldSkater.SkaterSeasonStats)
                    season.SkaterId = newSkater.Id;

                db.Skaters.Remove(oldSkater);
                db.SaveChanges();
            }
        }

        public static void ChangeSkaterName(string oldName, string newName)
        {
            using (var db = new BeaujeauxEntities())
            {
                var skaters = db.Skaters.Where(a => a.Name == oldName);
                if (skaters.Count() != 1)
                    throw new Exception("too many skaters");
                var oldSkater = skaters.First();

                oldSkater.Name = newName;

                db.SaveChanges();
            }
        }

        public static void SplitCareerSameName(string name, int startCareer2)
        {
            using (var db = new BeaujeauxEntities())
            {
                var skaters = db.Skaters.Where(a => a.Name == name);
                if (skaters.Count() != 1)
                    throw new Exception("too many skaters");
                var skater = skaters.First();

                skater.Name = name + " I";

                var secondSkater = new DataEF.Skater();
                secondSkater.Name = name + " II";
                db.Skaters.Add(secondSkater);

                db.SaveChanges();

                secondSkater = db.Skaters.Where(a => a.Name == name + " II").First();

                foreach (var season in skater.SkaterSeasonStats.Where(a => a.Season.Number >= startCareer2))
                    season.SkaterId = secondSkater.Id;

                db.SaveChanges();
            }
        }

        public static void GetEverything()
        {
            List<SeasonData> seasons = new List<SeasonData>();
            List<SeasonPage> seasonPages = new List<SeasonPage>
            {
                new SeasonPage()
                {
                    Url = "http://www.shlstuff.wtgbear.com/RegSeason/S{0}/SHL-ProTeamScoring.html",
                    StartIndex = 29,
                    LeagueAcronym = "SHL",
                    IsPlayoffs = false,
                },
                new SeasonPage()
                {
                    Url = "http://www.shlstuff.wtgbear.com/Playoffs/S{0}/SHL-PLF-ProTeamScoring.html",
                    StartIndex = 30,
                    LeagueAcronym = "SHL",
                    IsPlayoffs = true,
                },
                new SeasonPage()
                {
                    Url = "http://www.smjhlstuff.wtgbear.com/S{0}Reg/SMJHL-ProTeamScoring.html",
                    StartIndex = 23,
                    LeagueAcronym = "SMJHL",
                    IsPlayoffs = false,
                },
                new SeasonPage()
                {
                    Url = "http://www.smjhlstuff.wtgbear.com/S{0}PO/SMJHL-PLF-ProTeamScoring.html",
                    StartIndex = 22,
                    LeagueAcronym = "SMJHL",
                    IsPlayoffs = true,
                }
            };
            int endIndex = 40;

            Console.WriteLine("Starting Modern Extractions");
            foreach (var page in seasonPages)
            {
                Console.WriteLine($"### {page.LeagueAcronym} - {(page.IsPlayoffs ? "Playoffs" : "Regular Season")}");
                for (int seasonNumber = page.StartIndex; seasonNumber <= endIndex; seasonNumber++)
                {
                    Console.Write(" - Season " + (seasonNumber < 10 ? " " : "") + seasonNumber);

                    var season = SeasonStatsExtractor.ExtractSeason(page.Url, seasonNumber, page.IsPlayoffs, page.LeagueAcronym);
                    if (season != null && season.Teams.Count > 0)
                    {
                        seasons.Add(new SeasonData(season));
                        Console.Write(" [DONE]");
                    }
                    else
                    {
                        Console.Write(" [Not Found]");
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("----");
            Console.WriteLine("Uploading Data to DB");

            DatabaseHelpers.AddLeagues();
            DatabaseHelpers.AddRequestTypes();

            foreach (var season in seasons)
            {
                Console.Write(" - Season " + season.SourceSeason.Number);
                season.SaveDataToDB();
                Console.Write(" [DONE]");
                Console.WriteLine();
            }

            DatabaseHelpers.AddFranchises();
            DatabaseHelpers.RemoveExtraPlayers();

            Console.WriteLine("----");
            Console.WriteLine("Extraction Complete");
            Console.WriteLine();
        }

    }
}
