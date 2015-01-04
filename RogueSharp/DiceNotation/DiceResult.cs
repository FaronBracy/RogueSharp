using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   public class DiceResult
   {
      public IRandom RandomUsed { get; private set; }
      public ReadOnlyCollection<TermResult> Results { get; private set; }
      public int Value { get; private set; }

      public DiceResult( IEnumerable<TermResult> results, IRandom randomUsed )
      {
         RandomUsed = randomUsed;
         Results = new ReadOnlyCollection<TermResult>( results.ToList() );
         Value = results.Sum( r => r.Value * r.Scalar );
      }
   }
}