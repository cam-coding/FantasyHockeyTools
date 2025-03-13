using DraftAnalysis;
using Objects;

namespace StartVsEndTeamComparison
{
    public static class StartVsEndRunner
    {
        public static void Run(DataSettings settings)
        {
            // var inputFile = Path.Combine(PathProvider.InputDataPath, "JoshLeaguePlayoffsStartingRosters.txt");
            Console.WriteLine("Starting gathering data for 'Start Vs End Team Comparison'");
            var playerDraftDict = DraftDataProvider.GetTeamToDraftedPlayers(settings);
            Console.WriteLine("Got team draft data");
            var startingRosterData = CurrentLineupProvider.GetTeamToLineup(settings);
            Console.WriteLine("Got current lineup data. Beginning Output");
            CollectResultsAndOutput.DoWork(settings, playerDraftDict, startingRosterData);
        }
    }
}