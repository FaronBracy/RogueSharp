using System;

namespace RogueSharp.DiceNotation.Exceptions
{
    public class InvalidMultiplicityException : Exception
    {
        public InvalidMultiplicityException(int multiplicity) : base(string.Format("Cannot roll {0} dice; this quantity is less than 0", multiplicity))
        { }
    }
}