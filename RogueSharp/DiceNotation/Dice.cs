namespace RogueSharp.DiceNotation
{
    public static class Dice
    {
        private static readonly IDiceParser DiceParser = new DiceParser();

        public static DiceExpression Parse(string expression)
        {
            return DiceParser.Parse(expression);
        }
    }
}