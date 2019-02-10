using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class PlayerStatsController : Controller
    {
        public ActionResult Index(string searchTerm)
        {
            PlayerSearchModel searchModel = searchTerm == null ? null : new PlayerSearchModel(searchTerm);
            return View(searchModel);
        }

        public ActionResult Skater(int? li, int? pi)
        {
            if (pi == null)
                RedirectToAction("Search");

            var _database = new BeaujeauxEntities();
            Skater skater = _database.Skaters.Where(s => s.Id == pi).FirstOrDefault();

            if (skater == null)
                throw new Exception("no player found");

            int leagueId = GetLeagueId(li);
            return View(new SkaterPlayerStatsModel(_database, skater, leagueId));
        }

        public ActionResult Goalie(int? li, int? pi)
        {
            if (pi == null)
                RedirectToAction("Search");

            var _database = new BeaujeauxEntities();
            Goalie goalie = _database.Goalies.Where(g => g.Id == pi).FirstOrDefault();

            if (goalie == null)
                throw new Exception("no player found");

            int leagueId = GetLeagueId(li);
            return View(new GoaliePlayerStatsModel(_database, goalie, leagueId));
        }

        private int GetLeagueId(int? li)
        {
            if (li.HasValue)
                Session.Add("LeagueId", li.Value);

            if (Session["LeagueId"] == null)
                Session["LeagueId"] = 1;

            int leagueId = (int)Session["LeagueId"];
            return leagueId;
        }

    }
}