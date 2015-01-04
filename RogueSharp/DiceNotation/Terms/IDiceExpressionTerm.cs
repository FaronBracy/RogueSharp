using System.Collections.Generic;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation.Terms
{
   public interface IDiceExpressionTerm
   {
      IEnumerable<TermResult> GetResults( IRandom random );
   }
}