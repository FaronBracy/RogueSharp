using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   public static class Dice
   {
      private static readonly IDiceParser DiceParser = new DiceParser();

      public static DiceExpression Parse( string expression )
      {
         return DiceParser.Parse( expression );
      }

      public static int Roll( string expression, IRandom random = null )
      {
         if ( random == null )
         {
            random = Singleton.DefaultRandom;
         }
         return Parse( expression ).Roll( random ).Value;
      }
   }
}