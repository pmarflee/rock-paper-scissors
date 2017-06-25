using RockPaperScissors.Core;
using RockPaperScissors.Core.Rules;
using System.Linq;

namespace RockPaperScissors.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Invalid number of arguments.  Use /? for help.");

                return;
            }

            if (args[0] == "/?")
            {
                System.Console.WriteLine("This program needs to be run with 4 parameters");
                System.Console.WriteLine("First: the maximum number of rounds");
                System.Console.WriteLine("Second: player 1 type.  Valid values are 'H' (Human), 'R' (Computer - Random), or 'T' (Computer - Tactical)");
                System.Console.WriteLine("Third: player 2 type.  Valid values are 'H' (Human), 'R' (Computer - Random), or 'T' (Computer - Tactical)");
                System.Console.WriteLine("Fourth: rules type.  Valid values are 'D' (Default) or 'E' (Extended)");

                return;
            }

            if (args.Length < 4)
            {
                System.Console.WriteLine("Invalid number of arguments");

                return;
            }

            if (!int.TryParse(args[0], out var rounds))
            {
                System.Console.WriteLine("Please specify number of rounds");

                return;
            }

            if (!TryCreatePlayer(args[1], 1, out var player1))
            {
                System.Console.WriteLine("Invalid type for player 1.  Should be 'H', 'R', or 'T'.");

                return;
            }

            if (!TryCreatePlayer(args[2], 2, out var player2))
            {
                System.Console.WriteLine("Invalid type for player 1.  Should be 'H', 'R', or 'T'.");

                return;
            }

            if (!TryCreateGameRules(args[3], out var rules))
            {
                System.Console.WriteLine("Invalid type for game rules.  Should be 'D' (Default), or 'E' (Extended).");

                return;
            }

            var game = GameState.NewGame(rules, player1, player2, rounds);
            var engine = new GameEngine();

            while (game.IsInProgress)
            {
                var player1Shape = GetInputForPlayer(game, player1);

                System.Console.WriteLine($"Player 1 selected {player1Shape}");

                var player2Shape = GetInputForPlayer(game, player2);

                System.Console.WriteLine($"Player 2 selected {player2Shape}");

                game = engine.Update(game, player1Shape, player2Shape);

                var roundsRemaining = game.IsInProgress ? (game.MaxRounds - game.RoundNumber + 1) : 0;

                System.Console.WriteLine(game.History.Last().Description);
                System.Console.WriteLine($"Player 1 score: {game.Player1Score} Player 2 score: {game.Player2Score} Rounds remaining: {roundsRemaining}");
            }

            if (game.Winner != null)
            {
                System.Console.WriteLine($"Game over.  Player {game.Winner.Number} was the winner.");
            }
            else
            {
                System.Console.WriteLine("Game over.  Draw.");
            }
        }

        static bool TryCreatePlayer(string type, int number, out Player player)
        {
            switch (type)
            {
                case "H":
                    player = new HumanPlayer(number);
                    break;
                case "R":
                    player = new RandomComputerPlayer(number);
                    break;
                case "T":
                    player = new TacticalComputerPlayer(number);
                    break;
                default:
                    player = null;
                    break;
            }

            return player != null;
        }

        static bool TryCreateGameRules(string type, out RuleList rules)
        {
            switch (type)
            {
                case "D":
                    rules = new RockPaperScissorsRules();
                    break;
                case "E":
                    rules = new RockPaperScissorsLizardSpockRules();
                    break;
                default:
                    rules = null;
                    break;
            }

            return rules != null;
        }

        private static Shape GetInputForPlayer(GameState game, Player player)
        {
            return player.Type == PlayerType.Human
                ? HandleHumanPlayerInput(game, player)
                : player.MakeSelection(game);
        }

        private static Shape HandleHumanPlayerInput(GameState game, Player player)
        {
            System.ConsoleKeyInfo key;
            var attempts = 0;
            Shape shape;

            do
            {
                attempts++;
                if (attempts > 1)
                {
                    System.Console.WriteLine("Invalid selection");
                }
                System.Console.WriteLine($"Round {game.RoundNumber}.  Player {player.Number} select shape: ");
                key = System.Console.ReadKey();
            } while (!TryParseInput(game, key.KeyChar, out shape));

            System.Console.WriteLine();

            return shape;
        }

        private static bool TryParseInput(GameState game, char key, out Shape shape)
        {
            var validShapes = game.Rules.Select(rule => rule.Winner).Distinct();
            
            switch (char.ToUpperInvariant(key))
            {
                case 'R':
                    shape = Shape.Rock;
                    break;
                case 'P':
                    shape = Shape.Paper;
                    break;
                case 'S':
                    shape = Shape.Scissors;
                    break;
                case 'L':
                    shape = Shape.Lizard;
                    break;
                case 'K':
                    shape = Shape.Spock;
                    break;
                default:
                    shape = Shape.None;
                    break;
            }

            return shape != Shape.None;
        }
    }
}
