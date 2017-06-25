using NUnit.Framework;
using RockPaperScissors.Core;
using RockPaperScissors.Core.Rules;
using System.Collections.Generic;
namespace RockPaperScissors.Tests
{
    [TestFixture]
    public class GameEngineTests
    {
        private static Player _player1 = new HumanPlayer(1);
        private static Player _player2 = new HumanPlayer(2);

        private static GameEngine _engine = new GameEngine();

        [Test, TestCaseSource("RoundScoringTestCases")]
        public void TestRoundScoring(IEnumerable<Rule> rules, Shape player1Shape, Shape player2Shape, Player expectedWinner)
        {
            Assert.AreEqual(expectedWinner, GameEngine.ScoreRound(rules, _player1, _player2, player1Shape, player2Shape).Item1);
        }

        static IEnumerable<TestCaseData> RoundScoringTestCases
        {
            get
            {
                var rockPaperScissorsRules = new RockPaperScissorsRules().Rules;

                yield return new TestCaseData(rockPaperScissorsRules, Shape.Rock, Shape.Scissors, _player1);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Paper, Shape.Rock, _player1);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Scissors, Shape.Paper, _player1);

                yield return new TestCaseData(rockPaperScissorsRules, Shape.Scissors, Shape.Rock, _player2);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Rock, Shape.Paper, _player2);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Paper, Shape.Scissors, _player2);

                yield return new TestCaseData(rockPaperScissorsRules, Shape.Rock, Shape.Rock, null);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Paper, Shape.Paper, null);
                yield return new TestCaseData(rockPaperScissorsRules, Shape.Scissors, Shape.Scissors, null);

                var rockPaperScissorsLizardSpockRules = new RockPaperScissorsLizardSpockRules().Rules;

                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Lizard, Shape.Spock, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Lizard, Shape.Paper, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Rock, Shape.Lizard, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Paper, Shape.Spock, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Scissors, Shape.Lizard, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Spock, Shape.Scissors, _player1);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Spock, Shape.Rock, _player1);

                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Spock, Shape.Lizard, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Paper, Shape.Lizard, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Lizard, Shape.Rock, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Spock, Shape.Paper, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Lizard, Shape.Scissors, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Scissors, Shape.Spock, _player2);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Rock, Shape.Spock, _player2);

                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Lizard, Shape.Lizard, null);
                yield return new TestCaseData(rockPaperScissorsLizardSpockRules, Shape.Spock, Shape.Spock, null);
            }
        }

        [Test, TestCaseSource("RoundNumberTestCases")]
        public void TestUpdateRoundNumber(GameState game, Shape player1Shape, Shape player2Shape, int expected)
        {
            var newGame =_engine.Update(game, player1Shape, player2Shape);

            Assert.AreEqual(expected, newGame.RoundNumber);
        }

        static IEnumerable<TestCaseData> RoundNumberTestCases
        {
            get
            {
                var rules = new RockPaperScissorsRules();

                yield return new TestCaseData(
                    GameState.NewGame(new RockPaperScissorsRules(), _player1, _player2),
                    Shape.Rock, Shape.Scissors,
                    2);

                yield return new TestCaseData(
                    new GameState(new RockPaperScissorsRules().Rules, _player1, _player2, 2, 3,
                    new List<RoundResult>
                    {
                        new RoundResult(1, Shape.Rock, Shape.Scissors, _player1)
                    }),
                    Shape.Rock, Shape.Scissors,
                    3);

                yield return new TestCaseData(
                    new GameState(new RockPaperScissorsRules().Rules, _player1, _player2, 3, 3,
                    new List<RoundResult>
                    {
                        new RoundResult(1, Shape.Rock, Shape.Scissors, _player1),
                        new RoundResult(2, Shape.Scissors, Shape.Rock, _player2)
                    }),
                    Shape.Rock, Shape.Scissors,
                    3);
            }
        }

        [Test, TestCaseSource("HistoryTestCases")]
        public void TestUpdateHistory(GameState game, Shape player1Shape, Shape player2Shape, IReadOnlyList<RoundResult> expected)
        {
            var newGame = _engine.Update(game, player1Shape, player2Shape);

            Assert.AreEqual(expected, newGame.History);
        }

        static IEnumerable<TestCaseData> HistoryTestCases
        {
            get
            {
                var rules = new RockPaperScissorsRules();

                yield return new TestCaseData(
                    GameState.NewGame(rules, _player1, _player2),
                    Shape.Rock, Shape.Scissors,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) });

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) }),
                    Shape.Rock, Shape.Scissors,
                    new List<RoundResult>
                    {
                        new RoundResult(1, Shape.Rock, Shape.Scissors, _player1),
                        new RoundResult(2, Shape.Rock, Shape.Scissors, _player1),
                    });
            }
        }

        [Test, TestCaseSource("InProgressTestCases")]
        public void TestUpdateHistory(GameState game, Shape player1Shape, Shape player2Shape, bool expected)
        {
            var newGame = _engine.Update(game, player1Shape, player2Shape);

            Assert.AreEqual(expected, newGame.IsInProgress);
        }

        static IEnumerable<TestCaseData> InProgressTestCases
        {
            get
            {
                var rules = new RockPaperScissorsRules();

                yield return new TestCaseData(
                    GameState.NewGame(rules, _player1, _player2),
                    Shape.Rock, Shape.Scissors,
                    true);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) }),
                    Shape.Rock, Shape.Scissors,
                    false);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) }),
                    Shape.Scissors, Shape.Rock,
                    true);
            }
        }

        [Test, TestCaseSource("WinnerTestCases")]
        public void TestUpdateHistory(GameState game, Shape player1Shape, Shape player2Shape, Player expected)
        {
            var newGame = _engine.Update(game, player1Shape, player2Shape);

            Assert.AreEqual(expected, newGame.Winner);
        }

        static IEnumerable<TestCaseData> WinnerTestCases
        {
            get
            {
                var rules = new RockPaperScissorsRules();

                yield return new TestCaseData(
                    GameState.NewGame(rules, _player1, _player2),
                    Shape.Rock, Shape.Scissors,
                    null);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) }),
                    Shape.Rock, Shape.Scissors,
                    _player1);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Scissors, Shape.Rock, _player2) }),
                    Shape.Scissors, Shape.Rock,
                    _player2);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult> { new RoundResult(1, Shape.Rock, Shape.Scissors, _player1) }),
                    Shape.Scissors, Shape.Rock,
                    null);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 3, 3,
                    new List<RoundResult>
                    {
                        new RoundResult(1, Shape.Rock, Shape.Rock, null),
                        new RoundResult(2, Shape.Rock, Shape.Rock, null)
                    }),
                    Shape.Rock, Shape.Rock,
                    null);

                yield return new TestCaseData(
                    new GameState(rules.Rules, _player1, _player2, 2, 3,
                    new List<RoundResult>
                    {
                        new RoundResult(1, Shape.Rock, Shape.Paper, _player2)
                    }),
                    Shape.Rock, Shape.Rock,
                    null);
            }
        }
    }
}
