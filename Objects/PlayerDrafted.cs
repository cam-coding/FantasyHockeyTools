using Objects;

namespace DraftAnalysis
{
    public class PlayerDrafted : PlayerBasic
    {
        public PlayerDrafted(string name, string team, string age, string position)
            : base(name)
        {
            NHLTeam = team;
            Age = int.Parse(age);
            Position = position;
        }

        public int PickNumber { get; set; }

        public string NHLTeam { get; set; }

        public int Age { get; set; }

        public string Position { get; set; }
    }
}