using System;

namespace RogueSharp.DiceNotation.Exceptions
{
    public class ImpossibleDieException : Exception
    {
        public ImpossibleDieException(int sides) : base(string.Format("Cannot construct a die with {0} sides", sides))
        { }
    }
}