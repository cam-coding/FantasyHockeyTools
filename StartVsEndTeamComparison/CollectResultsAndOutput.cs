using System.Xml.Linq;
using Objects;
using Utilities;

namespace StartVsEndTeamComparison
{
    public static class CollectResultsAndOutput
    {
        public static void DoWork(
            DataSettings settings,
            Dictionary<string, List<PlayerBasic>> teamsAtDraft,
            Dictionary<string, List<PlayerBasic>> teamsAtPlayoffs)
        {
            var data = GenerateOutputData(teamsAtDraft, teamsAtPlayoffs);
            var outputStrings = FormOutput(data, 4);
            var outputPath = Path.Combine(PathHelper.GetCurrentLineupPath(settings, false), $"{settings.League}Comparison.csv");
            WriteToCsv(outputPath, outputStrings);
            Console.WriteLine($"Output written to {outputPath}");
        }

        private static Dictionary<string, List<(string currentPlayer, string draftPlayer)>> GenerateOutputData(
            Dictionary<string, List<PlayerBasic>> teamsAtDraft,
            Dictionary<string, List<PlayerBasic>> teamsAtPlayoffs)
        {
            var results = new Dictionary<string, List<(string currentPlayer, string draftPlayer)>>();
            foreach (var team in teamsAtDraft.Keys)
            {
                var draftedPlayers = teamsAtDraft[team];
                var playoffPlayers = teamsAtPlayoffs[team];
                var i = 0;
                var pairings = new List<(string currentPlayer, string draftPlayer)>();
                while (i < playoffPlayers.Count)
                {
                    var draftPlayer = draftedPlayers.FirstOrDefault(p => p.PlayerName.Equals(playoffPlayers[i].PlayerName));
                    if (draftPlayer != null)
                    {
                        pairings.Add((playoffPlayers[i].PlayerName, draftPlayer.PlayerName));
                        playoffPlayers.RemoveAt(i);
                        draftedPlayers.Remove(draftPlayer);
                    }
                    else
                    {
                        i++;
                    }
                }

                for (var j = 0; j < Math.Max(playoffPlayers.Count, draftedPlayers.Count); j++)
                {
                    pairings.Add((GetValueOrEmpty(playoffPlayers, j), GetValueOrEmpty(draftedPlayers, j)));
                }
                results.Add(team, pairings);
            }
            return results;
        }

        private static List<List<string>> FormOutput(
            Dictionary<string, List<(string currentPlayer, string draftPlayer)>> data,
            int columns)
        {
            var teamNames = data.Keys.ToList();
            var pairs = data.Values.ToList();
            var currentLow = 0;
            var outputData = new List<List<string>>();

            while (currentLow < teamNames.Count - 1)
            {
                var maxLength = pairs.GetRange(currentLow, columns).Max(p => p.Count);
                var headerList = new List<string>();
                for (var i = 0; i < columns; i++)
                {
                    var currentTeamId = currentLow + i;
                    // headerRow
                    headerList.AddRange(new List<string>() { teamNames[currentTeamId], string.Empty, string.Empty });
                }
                outputData.Add(headerList);
                // counting rows
                for (var row = 0; row < maxLength; row++)
                {
                    var rowList = new List<string>();
                    for (var i = 0; i < columns; i++)
                    {
                        var currentTeamId = currentLow + i;
                        var currentPlayer = row < pairs[currentTeamId].Count ? pairs[currentTeamId][row].currentPlayer : string.Empty;
                        var draftPlayer = row < pairs[currentTeamId].Count ? pairs[currentTeamId][row].draftPlayer : string.Empty;
                        rowList.AddRange(new List<string>() { currentPlayer, draftPlayer, string.Empty });
                    }
                    outputData.Add(rowList);
                }

                outputData.Add(Enumerable.Repeat(string.Empty, columns * 3).ToList());
                outputData.Add(Enumerable.Repeat(string.Empty, columns * 3).ToList());

                currentLow += columns;
            }
            return outputData;
        }

        private static void WriteToCsv(string filePath, List<List<string>> outputData)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var row in outputData)
                {
                    string line = string.Join(",", row);
                    writer.WriteLine(line);
                }
            }
        }

        private static string GetValueOrEmpty(List<PlayerBasic> listy, int index)
        {
            if (index < listy.Count)
            {
                return listy[index].PlayerName;
            }
            return string.Empty;
        }
    }
}
