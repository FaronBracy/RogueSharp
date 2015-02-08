using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   public static class DiceExtensions
   {
      public static DiceResult Roll( this DiceExpression diceExpression )
      {
         return diceExpression.Roll( Singleton.DefaultRandom );
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