using LegacySthsStats;
using SthsData;
using SthsStatsToDB;
using System;
using System.Collections.Generic;

namespace LegacySthsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SeasonData> seasons = new List<SeasonData>();

            Console.WriteLine("Starting Legacy Extractions");

            seasons = GetAllSeasons();
            //seasons = GetOneShlSeason(22);

            Console.WriteLine("----");
            Console.WriteLine("Uploading Data to DB");

            DatabaseHelpers.AddLeagues();
            DatabaseHelpers.AddSeasonTypes();

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

        private static List<SeasonData> GetAllSeasons()
        {
            List<SeasonData> seasons = new List<SeasonData>();
            string[] leagueOptions = { "SHL", "SMJHL" };
            bool[] isPlayoffsOptions = { false, true };

            foreach (var isPlayoffs in isPlayoffsOptions)
            {
                foreach (var leagueAcronym in leagueOptions)
                {
                    string seasonType = isPlayoffs ? "Playoffs" : "Regular Season";
                    Console.WriteLine($"### {leagueAcronym} - {seasonType}");

                    for (int seasonNumber = 1; seasonNumber <= 28; seasonNumber++)
                    {
                        string seasonValue = seasonNumber.ToString();
                        if (seasonNumber < 10) seasonValue = " " + seasonValue;
                        Console.Write(" - Season " + seasonValue);

                        var sourceSeason = SeasonStatsExtractor.ExtractSeason(seasonNumber, isPlayoffs, leagueAcronym);
                        if (sourceSeason != null)
                        {
                            seasons.Add(new SeasonData(sourceSeason));
                            Console.Write(" [DONE]");
                        }
                        else
                        {
                            Console.Write(" [Not Found]");
                        }
                        Console.WriteLine();
                    }
                }
            }

            return seasons;
        }
        private static List<SeasonData> GetOneShlSeason(int seasonNumber)
        {
            List<SeasonData> seasons = new List<SeasonData>();
            string leagueAcronym = "SHL";
            bool isPlayoffs = false;

            string seasonType = isPlayoffs ? "Playoffs" : "Regular Season";
            Console.WriteLine($"### {leagueAcronym} - {seasonType}");

            string seasonValue = seasonNumber.ToString();
            if (seasonNumber < 10) seasonValue = " " + seasonValue;
            Console.Write(" - Season " + seasonValue);

            var sourceSeason = SeasonStatsExtractor.ExtractSeason(seasonNumber, isPlayoffs, leagueAcronym);
            if (sourceSeason != null)
            {
                seasons.Add(new SeasonData(sourceSeason));
                Console.Write(" [DONE]");
            }
            else
            {
                Console.Write(" [Not Found]");
            }
            Console.WriteLine();

            return seasons;
        }
    }
}
