using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   public static class DiceExtensions
   {
      private static readonly IRandom _random = new DotNetRandom();

      public static DiceResult Roll( this DiceExpression diceExpression )
      {
         return diceExpression.Roll( _random );
      }

      public static DiceResult MinRoll( this DiceExpression diceExpression )
      {
         return diceExpression.Roll( new MinRandom() );
      }

      public static DiceResult MaxRoll( this DiceExpression diceExpression )
      {
         return diceExpression.Roll( new MaxRandom() );
      }
   }
}