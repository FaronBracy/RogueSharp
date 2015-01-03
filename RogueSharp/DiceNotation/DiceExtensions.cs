using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.DiceNotation
{
    public static class DiceExtensions
    {
        private static readonly IDieRoller DieRoller = new StandardDieRoller(new System.Random());

        public static DiceResult Roll(this DiceExpression diceExpression)
        {
            return diceExpression.Roll(DieRoller);
        }

        public static DiceResult MinRoll(this DiceExpression diceExpression)
        {
            return diceExpression.Roll(new MinDieRoller());
        }

        public static DiceResult MaxRoll(this DiceExpression diceExpression)
        {
            return diceExpression.Roll(new MaxDieRoller());
        }
    }
}