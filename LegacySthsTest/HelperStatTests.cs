using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SthsData;

namespace SthsDataTest
{
    [TestClass]
    public class HelperStatTests
    {
        [TestMethod]
        public void Helper_GetAcronyms()
        {
            string line = "First Last (AAA) (BBB)(CCC) (D)";
            var acronyms = Helper.GetAcronymns(line).ToArray();

            Assert.AreEqual(acronyms[0], "AAA");
            Assert.AreEqual(acronyms[1], "BBB");
            Assert.AreEqual(acronyms[2], "CCC");
            Assert.AreEqual(acronyms[3], "D");
        }

        [TestMethod]
        public void Helper_RemoveAcronyms()
        {
            string line = "First Last (AAA) (BBB)(CCC) (D)";
            line = Helper.RemoveAcronyms(line);

            Assert.AreEqual(line, "First Last");
        }

        [TestMethod]
        public void Helper_GetTeamAcronym()
        {
            string line = "First Last (TOT) (WKP)";
            var acronyms = Helper.GetAcronymns(line).ToArray();
            var teamAcronym = Helper.GetTeamAcronym(acronyms);

            Assert.AreEqual(teamAcronym, "WKP");
        }

        [TestMethod]
        public void Helper_SplitCamelCase()
        {
            string line = "St.WestKendallPlatoonWKPMeow ";
            var split = Helper.SplitCamelCase(line);

            Assert.AreEqual(split, "St West Kendall Platoon WKP Meow");
        }

        [TestMethod]
        public void Helper_GetPercentageAmount()
        {
            double percent = 57.456;
            int total = 1000;
            var amount = Helper.GetPercentageAmount(percent, total);

            Assert.AreEqual(amount, 575);
        }
    }
}
