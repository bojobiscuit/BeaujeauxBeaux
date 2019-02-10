using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class PlayerSearchModel
    {

        public string SearchTerm { get; set; }
        public IEnumerable<SearchResult> SearchResults { get; set; }
        public string AlertMessage { get; set; }
        //public League League { get; set; } // Replace with Domain object

        public PlayerSearchModel()
        {
            SearchTerm = null;
            SearchResults = new List<SearchResult>();
        }

        public PlayerSearchModel(string searchTerm)
        {
            if (searchTerm == null)
                searchTerm = "";

            if (searchTerm.Length < 3)
            {
                AlertMessage = "Search Term needs to be at least 3 characters long. You tryna ruin my database, mofo? It probably doesn't really matter, but come on, brah.";
                SearchTerm = "";
                SearchResults = new List<SearchResult>();
            }
            else
            {
                SearchTerm = searchTerm.Trim().ToLower();
                using (var database = new BeaujeauxEntities())
                {
                    var results = new List<SearchResult>();
                    results.AddRange(database.SkaterSeasonStats
                        //.Where(sss => sss.Season.LeagueId == League.Id) // Replace with a domain object instead
                        .Where(sss => sss.Skater.Name.Contains(SearchTerm))
                        .Select(sss => new SearchResult { PlayerName = sss.Skater.Name, PlayerId = sss.SkaterId, IsGoalie = false })
                        .Distinct()
                    );

                    results.AddRange(database.GoalieSeasonStats
                        //.Where(gss => gss.Season.LeagueId == League.Id)
                        .Where(gss => gss.Goalie.Name.Contains(SearchTerm))
                        .Select(gss => new SearchResult { PlayerName = gss.Goalie.Name, PlayerId = gss.GoalieId, IsGoalie = true })
                        .Distinct()
                    );

                    SearchResults = results
                        .OrderBy(p => p.PlayerName)
                        .ToList();
                }
            }
        }
    }

    public class SearchResult
    {
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public bool IsGoalie { get; set; }
    }
}