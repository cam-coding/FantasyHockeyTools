using Objects;
using Utilities;

namespace DraftAnalysis
{
    public static class DraftAnalysisRunner
    {
        public static void Run(DataSettings settings)
        {
            Console.WriteLine("starting draft program");
            var playerDraftDict = DraftDataProvider.GetNameToDraftPosistion(settings);
            var data = PlayerDraftedFactory.CreatePlayerObjects(playerDraftDict, settings);
            Console.WriteLine("Good Data Gathered");

            var analyzer = new Analyzer(data, settings);
            analyzer.Analyze();
        }
    }
}