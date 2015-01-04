using System.Collections.Generic;
using System.Linq;
using RogueSharp.DiceNotation.Exceptions;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation.Terms
{
   public class DiceTerm : IDiceExpressionTerm
   {
      public int Multiplicity { get; private set; }
      public int Sides { get; private set; }
      public int Scalar { get; private set; }
      protected int Choose { get; private set; }

      public DiceTerm( int multiplicity, int sides, int scalar )
         : this( multiplicity, sides, multiplicity, scalar )
      { }

      public DiceTerm( int multiplicity, int sides, int choose, int scalar )
      {
         if ( sides <= 0 )
         {
            throw new ImpossibleDieException( string.Format( "Cannot construct a die with {0} sides", sides ) );
         }
         if ( multiplicity < 0 )
         {
            throw new InvalidMultiplicityException( string.Format( "Cannot roll {0} dice; this quantity is less than 0", multiplicity ) );
         }
         if ( choose < 0 )
         {
            throw new InvalidChooseException( "Cannot choose {0} of the dice; it is less than 0" );
         }
         if ( choose > multiplicity )
         {
            throw new InvalidChooseException( string.Format( "Cannot choose {0} dice, only {1} were rolled", choose, multiplicity ) );
         }

         Sides = sides;
         Multiplicity = multiplicity;
         Scalar = scalar;
         Choose = choose;
      }

      public IEnumerable<TermResult> GetResults( IRandom random )
      {
         IEnumerable<TermResult> results =
             from i in Enumerable.Range( 0, Multiplicity )
             select new TermResult {
                Scalar = Scalar,
                Value = random.Next( 1, Sides ),
                Type = "d" + Sides
             };
         return results.OrderByDescending( d => d.Value ).Take( Choose );
      }

      public override string ToString()
      {
         string choose = Choose == Multiplicity ? "" : "k" + Choose;
         return Scalar == 1
             ? string.Format( "{0}d{1}{2}", Multiplicity, Sides, choose )
             : string.Format( "{0}*{1}d{2}{3}", Scalar, Multiplicity, Sides, choose );
      }
   }
}