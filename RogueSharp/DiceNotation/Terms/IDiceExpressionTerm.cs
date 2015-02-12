using System.Collections.Generic;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation.Terms
{
   /// <summary>
   /// The IDiceExpressionTerm interface can be implemented to create a new term for a dice expression
   /// </summary>
   public interface IDiceExpressionTerm
   {
      /// <summary>
      /// Gets the TermResults for the implementation
      /// </summary>
      /// <param name="random">Optional parameter that defaults to null. It is recommended that if this is null then a default RNG is used such as DotNetRandom</param>
      /// <returns>An IEnumerable of TermResult which will have one item per result</returns>
      IEnumerable<TermResult> GetResults( IRandom random = null );
   }
}