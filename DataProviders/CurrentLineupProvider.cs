using HtmlAgilityPack;
using Objects;
using Utilities;

namespace StartVsEndTeamComparison
{
    // parses the html copied from this page: https://hockey.fantasysports.yahoo.com/hockey/47130/startingrosters
    public static class CurrentLineupProvider
    {
        public static Dictionary<string, List<PlayerBasic>> GetTeamToLineup(DataSettings settings)
        {
            var teamToRoster = new Dictionary<string, List<PlayerBasic>>();
            var lineupDataPath = PathHelper.GetCurrentLineupPath(settings, true);
            var dataFilePath = Path.Combine(lineupDataPath, $"{settings.League}LeaguePlayoffsStartingLineups.txt");
            var text = System.IO.File.ReadAllText(dataFilePath);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);

            var rootNode = doc.DocumentNode;
            HtmlNode mainNode = HtmlParsingHelper.FindNodesByClassName(rootNode, "Bd Gp-lg");
            var paragraphNodes = mainNode.SelectNodes("//p").ToList().GetRange(2,12);
            var teamNames = paragraphNodes.Select(x => x.InnerText.Trim()).ToList();
            var tableNodes = mainNode.SelectNodes("//table").ToList();

            foreach (var table in tableNodes)
            {
                var teamPlayers = new List<PlayerBasic>();
                var tbodyNode = table.SelectSingleNode("tbody");
                foreach (var player in tbodyNode.SelectNodes("tr"))
                {
                    var playerNode = HtmlParsingHelper.FindNodesByClassName(player, "Nowrap name F-link playernote");
                    if (playerNode != null)
                    {
                        teamPlayers.Add(new PlayerBasic(playerNode.InnerText));
                    }
                }
                teamToRoster.Add(teamNames[tableNodes.IndexOf(table)], teamPlayers);
            }
            return teamToRoster;
        }
    }
}