using System;

namespace RogueSharp.DiceNotation.Exceptions
{
    public class InvalidChooseException : Exception
    {
        public InvalidChooseException(int choose, int multiplicity) : base(string.Format("Cannot choose {0} dice, only {1} were rolled", choose, multiplicity))
        { }

        public InvalidChooseException(int choose) : base("Cannot choose {0} of the dice; it is less than 0")
        { }
    }
}