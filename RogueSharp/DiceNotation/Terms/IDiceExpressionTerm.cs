using System.Collections.Generic;
using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.DiceNotation.Terms
{
    public interface IDiceExpressionTerm
    {
        IEnumerable<TermResult> GetResults(IDieRoller dieRoller);
    }
}