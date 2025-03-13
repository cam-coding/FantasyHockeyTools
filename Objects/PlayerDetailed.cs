using Objects;

namespace DraftAnalysis
{
    public class PlayerDetailed : PlayerBasic
    {
        public PlayerDetailed(string name, string team, string age, string position)
            : base(name)
        {
            NHLTeam = team;
            Age = int.Parse(age);
            Position = position;
        }

        public string NHLTeam { get; set; }

        public int Age { get; set; }

        public string Position { get; set; }
    }
}