using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Objects;

namespace Utilities
{
    public static class PathHelper
    {
        public static string TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory.FullName;
        }

        public static string DataRootPath => Path.Combine(TryGetSolutionDirectoryInfo(), "Data");

        public static string InputDirectoryName => "InputData";

        public static string OutputDirectoryName => "OutputData";

        public static string GetDraftAnalysisPath(DataSettings settings, bool input)
        {
            return GetProgramSpecificDataPath(settings, input, "DraftAnalysis");
        }

        public static string GetCurrentLineupPath(DataSettings settings, bool input)
        {
            return GetProgramSpecificDataPath(settings, input, "CurrentLineup");
        }

        private static string GetProgramSpecificDataPath(DataSettings settings, bool input, string programName)
        {
            var path = input ? Path.Combine(DataRootPath, InputDirectoryName) : Path.Combine(DataRootPath, OutputDirectoryName);
            path = Path.Combine(path, programName);
            path = Path.Combine(path, settings.Year.ToString());
            Directory.CreateDirectory(path);
            return path;
        }

        private static string GetCurrentCsFileDirectory([CallerFilePath] string path = null)
        {
            return Path.GetDirectoryName(path);
        }

        public static string GetInputDirectoryPath([CallerFilePath] string path = null)
        {
            return Path.Combine(Path.GetDirectoryName(path), "Data\\InputData");
        }

        public static string GetOutputDirectoryPath([CallerFilePath] string path = null)
        {
            return Path.Combine(Path.GetDirectoryName(path), "Data\\OutputData");
        }
    }
}
