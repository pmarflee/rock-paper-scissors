namespace RockPaperScissors.Core
{
    public class RoundResult
    {
        public RoundResult(int roundNumber, Shape player1Shape, Shape player2Shape, Player winner = null, Rule rule = null)
        {
            RoundNumber = roundNumber;
            Player1Shape = player1Shape;
            Player2Shape = player2Shape;
            Winner = winner;
            Rule = rule;
        }

        public int RoundNumber { get; }
        public Player Winner { get; }
        public Shape Player1Shape { get; }
        public Shape Player2Shape { get; }
        public Rule Rule { get; }

        public string Description
        {
            get
            {
                return Winner == null
                    ? $"Round number: {RoundNumber} Result: Draw"
                    : $"Round number: {RoundNumber} Result: {Rule.Description} - Win for player {Winner.Number}";
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as RoundResult;

            if (other == null) return false;

            return RoundNumber == other.RoundNumber &&
                  Winner == other.Winner &&
                  Player1Shape == other.Player1Shape &&
                  Player2Shape == other.Player2Shape;
        }
    }
}
