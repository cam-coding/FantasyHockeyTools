using Objects;
using StartVsEndTeamComparison;
using Utilities;

internal class Program
{
    private static void Main(string[] args)
    {
        var dataPath = PathHelper.DataRootPath;
        var settings = new DataSettings(dataPath, "Josh", 2024);
        StartVsEndRunner.Run(settings);
    }
}