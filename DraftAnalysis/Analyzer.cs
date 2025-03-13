using Objects;
using Utilities;

namespace DraftAnalysis
{
    internal class Analyzer
    {
        private List<PlayerDrafted> _data;
        private readonly string _baseOutputPath;

        internal Analyzer(List<PlayerDrafted> data, DataSettings settings)
        {
            _data = data;
            _baseOutputPath = Path.Combine(PathHelper.GetDraftAnalysisPath(settings, false), settings.League);
        }

        public void Analyze()
        {
            AnalyzeTeams();
            AnalyzePositions();
            AnalyzeAges();
        }

        private void AnalyzeTeams()
        {
            List<string[]> outputData = new List<string[]>
            {
                new string[] { "Team", "Count" }, // Header row
            };

            var teamCounts = _data.GroupBy(p => p.NHLTeam).Select(g => new { Team = g.Key, Count = g.Count() });

            var sortedCount = teamCounts.OrderByDescending(t => t.Count);
            foreach (var item in sortedCount)
            {
                outputData.Add(new string[] { item.Team, item.Count.ToString() });
            }

            /*
            outputData.Add(new string[] { "", "" });
            outputData.Add(new string[] { "", "" });
            outputData.Add(new string[] { "", "" });
            outputData.Add(new string[] { "Team", "Count" });

            var sortedAlphabetically = teamCounts.OrderBy(t => t.Team);
            foreach (var item in sortedAlphabetically)
            {
                outputData.Add(new string[] { item.Team, item.Count.ToString() });
            }*/

            string filePath = CreateOutputPath("TEAMS");
            WriteToCsv(filePath, outputData);
        }

        private void AnalyzePositions()
        {
            var positionCounts = new Dictionary<string, int>();
            positionCounts.Add("C", 0);
            positionCounts.Add("RW", 0);
            positionCounts.Add("LW", 0);
            positionCounts.Add("D", 0);
            positionCounts.Add("G", 0);

            foreach (var player in _data)
            {
                var positions = player.Position.Split('/');
                foreach (var position in positions)
                {
                    positionCounts[position]++;
                }
            }
            // Sort the dictionary by values in descending order
            var sortedPositionCounts = positionCounts.OrderByDescending(pc => pc.Value);
            List<string[]> outputData = new List<string[]>
            {
                new string[] { "Position", "Count" }, // Header row
            };

            foreach (var item in sortedPositionCounts)
            {
                outputData.Add(new string[] { item.Key, item.Value.ToString() });
            }

            string filePath = CreateOutputPath("POS");
            WriteToCsv(filePath, outputData);
        }

        private void AnalyzeAges()
        {
            List<string[]> outputData = new List<string[]>
            {
                new string[] { "Age", "Count" }, // Header row
            };

            var teamCounts = _data.GroupBy(p => p.Age).Select(g => new { Age = g.Key, Count = g.Count() });

            var sortedCount = teamCounts.OrderBy(t => t.Age);
            foreach (var item in sortedCount)
            {
                outputData.Add(new string[] { item.Age.ToString(), item.Count.ToString() });
            }

            string filePath = CreateOutputPath("AGE");
            WriteToCsv(filePath, outputData);
        }

        private void WriteToCsv(string filePath, List<string[]> outputData)
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

        private string CreateOutputPath(string dataTypeName)
        {
            return _baseOutputPath + $"_{dataTypeName}.csv";
        }
    }
}