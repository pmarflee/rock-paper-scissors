using System;
using System.Linq;

namespace RockPaperScissors.Core
{
    public enum PlayerType { Human, Computer }

    public abstract class Player
    {
        protected Player(int number)
        {
            Number = number;
        }

        public abstract PlayerType Type { get; }

        public int Number { get; }

        public abstract Shape MakeSelection(GameState game);
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(int number) : base(number) { }

        public override PlayerType Type => PlayerType.Human;

        public override Shape MakeSelection(GameState game)
        {
            throw new NotImplementedException();
        }
    }

    public class RandomComputerPlayer : Player
    {
        private readonly CryptoRandom _random = new CryptoRandom();

        public RandomComputerPlayer(int number) : base(number) { }

        public override PlayerType Type => PlayerType.Computer;

        public override Shape MakeSelection(GameState game)
        {
            var shapes = game.Rules.Select(rule => rule.Winner).Distinct().ToList();
            var index = _random.Next(shapes.Count);

            return shapes[index];
        }
    }

    public class TacticalComputerPlayer : RandomComputerPlayer
    {
        public TacticalComputerPlayer(int number) : base(number) { }

        public override Shape MakeSelection(GameState game)
        {
            if (game.RoundNumber == 1) return base.MakeSelection(game);

            var mostRecentRoundResult = game.History.Last();

            var previousSelection = Number == 1 ? mostRecentRoundResult.Player1Shape : mostRecentRoundResult.Player2Shape;

            return game.Rules.First(rule => rule.Loser == previousSelection).Winner;
        }
    }
}
