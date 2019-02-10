using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace LegacySthsStats
{
    internal class SeasonFile
    {
        public SeasonFile(string filePath)
        {
            LoadFile(filePath);
        }

        public IEnumerable<TeamSection> GetTeamSections()
        {
            IEnumerable<string> teamSectionData = _htmlDocument.DocumentNode.Descendants("pre").Select(a => a.InnerText);
            List<TeamSection> teamSections = new List<TeamSection>();
            foreach (var data in teamSectionData)
                teamSections.Add(new TeamSection(data));

            return teamSections;
        }

        private void LoadFile(string filePath)
        {
            _htmlDocument = new HtmlDocument();
            _htmlDocument.Load(filePath);
        }

        private HtmlDocument _htmlDocument;
    }
}
