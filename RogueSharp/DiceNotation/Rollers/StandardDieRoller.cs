namespace RogueSharp.DiceNotation.Rollers
{
    public class StandardDieRoller : IDieRoller
    {
        private static readonly System.Random Random = new System.Random();
        
        private readonly System.Random _random;

        public StandardDieRoller() : this(Random)
        { }

        public StandardDieRoller(System.Random random)
        {
            _random = random;
        }

        public int RollDie(int sides)
        {
            return _random.Next(0, sides) + 1;
        }
    }
}