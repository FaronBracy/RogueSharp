namespace RogueSharp.DiceNotation.Rollers
{
    public class MaxDieRoller : IDieRoller
    {
        public int RollDie(int sides)
        {
            return sides;
        }
    }
}