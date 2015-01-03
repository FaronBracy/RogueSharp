using System.Collections.Generic;
using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.DiceNotation.Terms
{
    public class ConstantTerm : IDiceExpressionTerm
    {
        private readonly int _constant;

        public ConstantTerm(int constant)
        {
            _constant = constant;
        }

        public IEnumerable<TermResult> GetResults(IDieRoller dieRoller)
        {
            return new[] {new TermResult {Scalar = 1, Value = _constant, Type = "constant"}};
        }

        public override string ToString()
        {
            return _constant.ToString();
        }
    }
}