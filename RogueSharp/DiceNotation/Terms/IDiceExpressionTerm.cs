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
      /// <param name="random">IRandom RNG used to perform the Roll.</param>
      /// <returns>An IEnumerable of TermResult which will have one item per result</returns>
      IEnumerable<TermResult> GetResults( IRandom random );

      /// <summary>
      /// Gets the TermResults for the implementation
      /// </summary>
      /// <returns>An IEnumerable of TermResult which will have one item per result</returns>
      /// <remarks>Uses DotNetRandom as its RNG</remarks>
      IEnumerable<TermResult> GetResults();
   }
}