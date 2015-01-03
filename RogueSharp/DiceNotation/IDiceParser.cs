namespace RogueSharp.DiceNotation
{
    public interface IDiceParser
    {
        DiceExpression Parse(string expression);
    }
}