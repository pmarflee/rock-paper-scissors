using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors.Core.Rules
{
    public abstract class RuleList
    {
        protected RuleList(IEnumerable<Rule> rules)
        {
            Rules = rules;
        }

        public IEnumerable<Rule> Rules { get; }
    }

    public class RockPaperScissorsRules : RuleList
    {
        public RockPaperScissorsRules() : base(new Rule[]
            {
                new Rule(Shape.Rock, Shape.Scissors, "Crushes"),
                new Rule(Shape.Paper, Shape.Rock, "Covers"),
                new Rule(Shape.Scissors, Shape.Paper, "Cuts")
            }) { }
    }

    public class RockPaperScissorsLizardSpockRules : RuleList
    {
        public RockPaperScissorsLizardSpockRules() : base(
            new RockPaperScissorsRules().Rules.Concat(
            new[]
            {
                new Rule(Shape.Lizard, Shape.Spock, "Poisons"),
                new Rule(Shape.Lizard, Shape.Paper, "Eats"),
                new Rule(Shape.Rock, Shape.Lizard, "Crushes"),
                new Rule(Shape.Paper, Shape.Spock, "Disproves"),
                new Rule(Shape.Scissors, Shape.Lizard, "Decapitates"),
                new Rule(Shape.Spock, Shape.Scissors, "Smashes"),
                new Rule(Shape.Spock, Shape.Rock, "Vapourises")
            }).ToList()) { }
    }
}
