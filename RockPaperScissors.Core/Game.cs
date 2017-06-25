using RockPaperScissors.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Core
{
    public class GameState
    {
        public GameState(IEnumerable<Rule> rules, Player player1, Player player2, 
            int roundNumber, int maxRounds, IReadOnlyList<RoundResult> history)
        {
            Rules = rules;
            Player1 = player1;
            Player2 = player2;
            MaxRounds = maxRounds;
            RoundNumber = roundNumber;
            History = history;
        }

        public IEnumerable<Rule> Rules { get; }
        public int MaxRounds { get; }
        public int RoundNumber { get; }
        public IReadOnlyList<RoundResult> History { get; }
        public Player Player1 { get; }
        public Player Player2 { get; }

        public int Player1Score
        {
            get { return Scores.First(score => score.Item1 == Player1).Item2; }
        }

        public int Player2Score
        {
            get { return Scores.First(score => score.Item1 == Player2).Item2; }
        }

        public bool IsInProgress
        {
            get
            {
                var roundsRemaining = MaxRounds - History.Count;

                if (roundsRemaining == 0) return false;

                var stats = Scores.ToList();
                var scoreDifference = stats.First().Item2 - stats.Last().Item2;

                return roundsRemaining >= scoreDifference; ;
            }
        }

        private IEnumerable<Tuple<Player, int>> Scores
        {
            get
            {
                var stats = from player in new[] { Player1, Player2 }
                            join result in History on player equals result.Winner into wins
                            select Tuple.Create(player, wins.Count());

                return stats.OrderByDescending(stat => stat.Item2);
            }
        }

        public Player Winner
        {
            get
            {
                if (IsInProgress) return null;

                var player1Score = Player1Score;
                var player2Score = Player2Score;

                if (player1Score > player2Score) return Player1;
                if (player2Score > player1Score) return Player2;

                return null;
            }
        }

        public static GameState NewGame(RuleList rulelist, Player player1, Player player2, int maxRounds = 3)
        {
            return new GameState(rulelist.Rules, player1, player2, 1, maxRounds, new List<RoundResult>());
        }

        public static GameState Increment(GameState state, RoundResult result)
        {
            if (!state.IsInProgress) throw new InvalidOperationException("Game is no longer in progress");

            var history = new List<RoundResult>(state.History.Concat(new[] { result }));

            return new GameState(state.Rules, state.Player1, state.Player2, 
                history.Count == state.MaxRounds ? state.RoundNumber : state.RoundNumber + 1, 
                state.MaxRounds, 
                history);
        }
    }

    public class GameEngine
    {
        public GameState Update(GameState game, Shape player1Shape, Shape player2Shape)
        {
            if (!game.IsInProgress) throw new InvalidOperationException("Game is no longer in progress");

            (Player winner, Rule rule) = ScoreRound(game.Rules, game.Player1, game.Player2, player1Shape, player2Shape);

            return GameState.Increment(game, new RoundResult(game.RoundNumber, player1Shape, player2Shape, winner, rule));
        }

        public static (Player, Rule) ScoreRound(IEnumerable<Rule> rules, Player player1, Player player2, Shape player1Shape, Shape player2Shape)
        {
            Rule GetWinningRule(Shape beats, Shape isBeatenBy) => rules.FirstOrDefault(rule =>
                rule.Winner == beats && rule.Loser == isBeatenBy);

            var player1WinningRule = GetWinningRule(player1Shape, player2Shape);

            if (player1WinningRule != null)
            {
                return (player1, player1WinningRule);
            }

            var player2WinningRule = GetWinningRule(player2Shape, player1Shape);

            if (player2WinningRule != null)
            {
                return (player2, player2WinningRule);
            }

            return (null, null);
        }
    }
}
