using NUnit.Framework;
using RockPaperScissors.Core;
using RockPaperScissors.Core.Rules;
using System.Collections.Generic;

namespace RockPaperScissors.Tests
{
    [TestFixture]
    public class TacticalComputerPlayerTests
    {
        private static IEnumerable<Rule> _rules = new RockPaperScissorsRules().Rules;

        [Test]
        public void ShouldPlayARandomShapeIfItIsTheFirstRoundOfTheGame()
        {
            var player1 = new TacticalComputerPlayer(1);
            var player2 = new HumanPlayer(2);

            var game = GameState.NewGame(new RockPaperScissorsRules(), player1, player2);

            Assert.That(player1.MakeSelection(game), 
                Is.EqualTo(Shape.Rock).Or.EqualTo(Shape.Paper).Or.EqualTo(Shape.Scissors));
        }

        [Test, TestCaseSource("TestCases")]
        public void ShouldPlayTheShapeThatWouldHaveBeatenItsLastChoiceIfItIsNotTheFirstRoundOfTheGame(
            Shape player1Shape, Shape player2Shape, int playerNumber, Shape expectedSelection)
        {
            var roundResults = new List<RoundResult>();
            var computerPlayer = new TacticalComputerPlayer(playerNumber);
            var humanPlayer = new HumanPlayer(playerNumber == 1 ? 2 : 1);
            var player1 = computerPlayer.Number == 1 ? (Player)computerPlayer : humanPlayer;
            var player2 = computerPlayer.Number == 2 ? (Player)computerPlayer : humanPlayer;

            var game = new GameState(new RockPaperScissorsRules().Rules, 
                player1, player2, 2, 3, 
                new List<RoundResult> { new RoundResult(1, player1Shape, player2Shape, null) });

            Assert.That(computerPlayer.MakeSelection(game), Is.EqualTo(expectedSelection));
        }

        static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(Shape.Rock, Shape.Scissors, 1, Shape.Paper);
                yield return new TestCaseData(Shape.Paper, Shape.Rock, 1, Shape.Scissors);
                yield return new TestCaseData(Shape.Scissors, Shape.Paper, 1, Shape.Rock);
                yield return new TestCaseData(Shape.Scissors, Shape.Scissors, 1, Shape.Rock);

                yield return new TestCaseData(Shape.Rock, Shape.Scissors, 2, Shape.Rock);
                yield return new TestCaseData(Shape.Paper, Shape.Rock, 2, Shape.Paper);
                yield return new TestCaseData(Shape.Scissors, Shape.Paper, 2, Shape.Scissors);
                yield return new TestCaseData(Shape.Scissors, Shape.Scissors, 2, Shape.Rock);
            }
        }
    }
}
