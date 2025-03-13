namespace Objects
{
    public class DataSettings
    {
        public DataSettings(string dataDirectoryPath, string league, int year)
        {
            DataDirectoryPath = dataDirectoryPath;
            League = league;
            Year = year;
        }

        public string DataDirectoryPath { get; set; }

        public string League { get; set; }

        public int Year { get; set; }
    }
}
