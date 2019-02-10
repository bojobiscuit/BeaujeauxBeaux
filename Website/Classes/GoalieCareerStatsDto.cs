using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class GoalieCareerStatsDto
    {
        public int GoalieId { get; set; }
        public string GoalieName { get; set; }
        public int SeasonCount { get; set; }
        public int TeamCount { get; set; }
        public int GP { get; set; }
        public int W { get; set; }
        public int L { get; set; }
        public int OTL { get; set; }
        public int MP { get; set; }
        public int PIM { get; set; }
        public int SO { get; set; }
        public int GA { get; set; }
        public int SA { get; set; }
        public int SAR { get; set; }
        public int A { get; set; }
        public int EG { get; set; }
        public int PSS { get; set; }
        public int PSA { get; set; }
        public int ST { get; set; }
        public int BG { get; set; }
        public int S1 { get; set; }
        public int S2 { get; set; }
        public int S3 { get; set; }
    }
}