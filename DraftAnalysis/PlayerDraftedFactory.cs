using Microsoft.VisualBasic.FileIO;
using Objects;
using Utilities;

namespace DraftAnalysis
{
    internal static class PlayerDraftedFactory
    {
        public static List<PlayerDrafted> CreatePlayerObjects(Dictionary<string, int> draftData, DataSettings settings)
        {
            var draftFilepath = PathHelper.GetDraftAnalysisPath(settings, true);
            var dataFilePath = Path.Combine(draftFilepath, "PlayerData.csv");
            var playerData = GetPlayersFromCsv(dataFilePath);

            var goodData = new List<PlayerDrafted>();
            foreach (var player in draftData)
            {
                if (playerData.ContainsKey(player.Key))
                {
                    playerData[player.Key].PickNumber = player.Value;
                    goodData.Add(playerData[player.Key]);
                }
                else
                {
                    Console.WriteLine($"Player {player.Key} not found in player data.");
                }
            }
            return goodData;
        }

        private static Dictionary<string, PlayerDrafted> GetPlayersFromCsv(string dataFilePath)
        {
            var dict = new Dictionary<string, PlayerDrafted>();
            using (TextFieldParser parser = new TextFieldParser(dataFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the header row if needed
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        // Read fields in the current row
                        string[] fields = parser.ReadFields();
                        dict.Add(fields[0], new PlayerDrafted(fields[0], fields[1], fields[2], fields[3]));
                    }
                    catch (MalformedLineException ex)
                    {
                        Console.WriteLine($"Line {ex.LineNumber} is invalid. Skipping.");
                    }
                }
            }

            return dict;
        }
    }
}