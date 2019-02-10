using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class CareerStatsController : Controller
    {
        public ActionResult Skater(int? li, int? st, int? pn, string so, int? sd, int? ti)
        {
            int leagueId = GetLeagueId(li);
            SeasonStatsParameters seasonParameters = new SeasonStatsParameters(leagueId, st, pn, so, sd, ti);
            SkaterCareerStatsModel seasonStatsModel = new SkaterCareerStatsModel(seasonParameters);
            return View(seasonStatsModel);
        }

        public ActionResult Goalie(int? li, int? st, int? pn, string so, int? sd, int? ti)
        {
            int leagueId = GetLeagueId(li);
            SeasonStatsParameters seasonParameters = new SeasonStatsParameters(leagueId, st, pn, so, sd, ti);
            GoalieCareerStatsModel seasonStatsModel = new GoalieCareerStatsModel(seasonParameters);
            return View(seasonStatsModel);
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