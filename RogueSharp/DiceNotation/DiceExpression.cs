using System.Collections.Generic;
using System.Linq;
using RogueSharp.DiceNotation.Terms;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   public class DiceExpression
   {
      private readonly IList<IDiceExpressionTerm> _terms;

      public DiceExpression()
         : this( new IDiceExpressionTerm[] { } )
      { }

      private DiceExpression( IEnumerable<IDiceExpressionTerm> diceTerms )
      {
         _terms = diceTerms.ToList();
      }

      public DiceExpression Die( int sides, int scalar )
      {
         return Dice( 1, sides, scalar );
      }

      public DiceExpression Die( int sides )
      {
         return Dice( 1, sides );
      }

      public DiceExpression Dice( int multiplicity, int sides, int scalar = 1, int? choose = null )
      {
         _terms.Add( new DiceTerm( multiplicity, sides, choose ?? multiplicity, scalar ) );
         return this;
      }

      public DiceExpression Constant( int constant )
      {
         _terms.Add( new ConstantTerm( constant ) );
         return this;
      }

      public DiceResult Roll( IRandom random )
      {
         IEnumerable<TermResult> termResults = _terms.SelectMany( t => t.GetResults( random ) ).ToList();
         return new DiceResult( termResults, random );
      }

      public override string ToString()
      {
         return string.Join( " + ", _terms );
      }
   }
}