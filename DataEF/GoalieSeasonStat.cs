//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoalieSeasonStat
    {
        public int Id { get; set; }
        public int GoalieId { get; set; }
        public int TeamId { get; set; }
        public int SeasonId { get; set; }
        public bool IsSubtotal { get; set; }
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
    
        public virtual Goalie Goalie { get; set; }
        public virtual Season Season { get; set; }
        public virtual Team Team { get; set; }
    }
}
