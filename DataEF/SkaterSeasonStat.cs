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
    
    public partial class SkaterSeasonStat
    {
        public int Id { get; set; }
        public int SkaterId { get; set; }
        public Nullable<int> TeamId { get; set; }
        public int SeasonId { get; set; }
        public bool IsSubtotal { get; set; }
        public int GP { get; set; }
        public int G { get; set; }
        public int A { get; set; }
        public int P { get; set; }
        public int PLMI { get; set; }
        public int PIM { get; set; }
        public int PM5 { get; set; }
        public int HIT { get; set; }
        public int HTT { get; set; }
        public int SHT { get; set; }
        public int OSB { get; set; }
        public int OSM { get; set; }
        public int SB { get; set; }
        public int MP { get; set; }
        public int PPG { get; set; }
        public int PPA { get; set; }
        public int PPP { get; set; }
        public int PPS { get; set; }
        public int PPM { get; set; }
        public int PKG { get; set; }
        public int PKA { get; set; }
        public int PKP { get; set; }
        public int PKS { get; set; }
        public int PKM { get; set; }
        public int GW { get; set; }
        public int GT { get; set; }
        public int FOW { get; set; }
        public int FOT { get; set; }
        public int GA { get; set; }
        public int TA { get; set; }
        public int EG { get; set; }
        public int HT { get; set; }
        public int PSG { get; set; }
        public int PSS { get; set; }
        public int FW { get; set; }
        public int FL { get; set; }
        public int FT { get; set; }
        public int GS { get; set; }
        public int PS { get; set; }
        public int WG { get; set; }
        public int WP { get; set; }
        public int S1 { get; set; }
        public int S2 { get; set; }
        public int S3 { get; set; }
    
        public virtual Season Season { get; set; }
        public virtual Skater Skater { get; set; }
        public virtual Team Team { get; set; }
    }
}
