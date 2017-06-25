namespace RockPaperScissors.Core
{
    public class Rule
    {
        public Rule(Shape winner, Shape loser, string action)
        {
            Winner = winner;
            Loser = loser;
            Description = $"{winner} {action} {loser}";
        }

        public Shape Winner { get; }

        public Shape Loser { get; }

        public string Description { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Rule;

            if (other == null) return false;

            return Winner == other.Winner && Loser == other.Loser && Description == other.Description;
        }
    }
}
