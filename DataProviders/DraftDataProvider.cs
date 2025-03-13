using HtmlAgilityPack;
using Objects;
using Utilities;

namespace DraftAnalysis
{
    // pases the data from this page https://hockey.fantasysports.yahoo.com/hockey/47130/draftresults?drafttab=round&draft_results_period=current
    // note the "round" format
    public static class DraftDataProvider
    {
        public static Dictionary<string, int> GetNameToDraftPosistion(DataSettings settings)
        {
            var draftFilepath = PathHelper.GetDraftAnalysisPath(settings, true);
            var dataFilePath = Path.Combine(draftFilepath, $"{settings.League}LeagueDraftResults.txt");

            var data = new Dictionary<string, int>();
            var text = System.IO.File.ReadAllText(dataFilePath);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);

            var rootNode = doc.DocumentNode;
            HtmlNode draftRoot = HtmlParsingHelper.FindNodeById(rootNode, "drafttables");
            var tableNodes = draftRoot.SelectNodes("//table");

            foreach (var table in tableNodes)
            {
                var roundText = HtmlParsingHelper.FindNodesByClassName(table, "Fw-b").InnerText;
                var roundNumber = int.Parse(roundText.Split(' ')[1]);
                var tbodyNode = table.SelectSingleNode("tbody");
                foreach (var pick in tbodyNode.SelectNodes("tr"))
                {
                    var pickNumber = int.Parse(pick.SelectSingleNode("td").InnerText.Split('.')[0]) + ((roundNumber - 1) * 12);
                    var player = HtmlParsingHelper.FindNodesByClassName(pick, "name").InnerText;
                    data.Add(player, pickNumber);
                }
            }
            return data;
        }

        public static Dictionary<string, List<PlayerBasic>> GetTeamToDraftedPlayers(DataSettings settings)
        {
            var draftFilepath = PathHelper.GetDraftAnalysisPath(settings, true);
            var dataFilePath = Path.Combine(draftFilepath, $"{settings.League}LeagueDraftResults.txt");

            var data = new Dictionary<string, List<PlayerBasic>>();
            var text = System.IO.File.ReadAllText(dataFilePath);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);

            var rootNode = doc.DocumentNode;
            HtmlNode draftRoot = HtmlParsingHelper.FindNodeById(rootNode, "drafttables");
            var tableNodes = draftRoot.SelectNodes("//table");

            foreach (var table in tableNodes)
            {
                var roundText = HtmlParsingHelper.FindNodesByClassName(table, "Fw-b").InnerText;
                var roundNumber = int.Parse(roundText.Split(' ')[1]);
                var tbodyNode = table.SelectSingleNode("tbody");
                foreach (var pick in tbodyNode.SelectNodes("tr"))
                {
                    var pickNumber = int.Parse(pick.SelectSingleNode("td").InnerText.Split('.')[0]) + ((roundNumber - 1) * 12);
                    var playerName = HtmlParsingHelper.FindNodesByClassName(pick, "name").InnerText;
                    var fantasyTeam = HtmlParsingHelper.FindNodesByClassName(pick, "last Px-sm").Attributes["title"].Value.Trim();
                    if (data.ContainsKey(fantasyTeam))
                    {
                        data[fantasyTeam].Add(new PlayerBasic(playerName));
                    }
                    else
                    {
                        data.Add(fantasyTeam, new List<PlayerBasic> { new PlayerBasic(playerName) });
                    }
                }
            }
            return data;
        }
    }
}