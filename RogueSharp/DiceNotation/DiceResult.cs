using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   /// <summary>
   /// The DiceResult class represents the result of rolling a DiceExpression
   /// </summary>
   public class DiceResult
   {
      /// <summary>
      /// The random number generator used to get this result
      /// </summary>
      public IRandom RandomUsed { get; private set; }

      /// <summary>
      /// A Collection of TermResults that represents one result for each DiceTerm in the DiceExpression
      /// </summary>
      public ReadOnlyCollection<TermResult> Results { get; private set; }

      /// <summary>
      /// The total result of the the roll
      /// </summary>
      public int Value { get; private set; }

      /// <summary>
      /// Construct a new DiceResult from the specified values
      /// </summary>
      /// <param name="results">An IEnumerable of TermResult that represents one result for each DiceTerm in the DiceExpression</param>
      /// <param name="randomUsed">The random number generator used to get this result</param>
      public DiceResult( IEnumerable<TermResult> results, IRandom randomUsed )
      {
         RandomUsed = randomUsed;
         Results = new ReadOnlyCollection<TermResult>( results.ToList() );
         Value = results.Sum( r => r.Value * r.Scalar );
      }
   }
}