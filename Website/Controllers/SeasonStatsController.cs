using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class SeasonStatsController : Controller
    {
        public ActionResult Skater(int? li, int? sn, int? st, int? pn, int? ti, string so, int? sd, int? era)
        {
            int leagueId = GetLeagueId(li);
            SeasonStatsParameters seasonParameters = new SeasonStatsParameters(leagueId, sn, st, pn, ti, so, sd, era);
            SkaterSeasonStatsModel seasonStatsModel = new SkaterSeasonStatsModel(seasonParameters);
            return View(seasonStatsModel);
        }

        public ActionResult Goalie(int? li, int? sn, int? st, int? pn, int? ti, string so, int? sd, int? era)
        {
            int leagueId = GetLeagueId(li);
            SeasonStatsParameters seasonParameters = new SeasonStatsParameters(leagueId, sn, st, pn, ti, so, sd, era);
            GoalieSeasonStatsModel seasonStatsModel = new GoalieSeasonStatsModel(seasonParameters);
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