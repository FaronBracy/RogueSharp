using RogueSharp.Random;

namespace RogueSharp.DiceNotation
{
   /// <summary>
   /// The Dice class is a static class that has convenience methods for parsing and rolling dice
   /// </summary>
   public static class Dice
   {
      private static readonly IDiceParser _diceParser = new DiceParser();
      /// <summary>
      /// Parse the specified string into a DiceExpression
      /// </summary>
      /// <param name="expression">The string dice expression to parse. Ex. 3d6+4</param>
      /// <returns>A DiceExpression representing the parsed string</returns>
      public static DiceExpression Parse( string expression )
      {
         return _diceParser.Parse( expression );
      }
      /// <summary>
      /// A convenience method for parsing a dice expression from a string, rolling the dice, and returning the total.
      /// </summary>
      /// <param name="expression">The string dice expression to parse. Ex. 3d6+4</param>
      /// <param name="random">Optional parameter that defaults to DotNetRandom. If a different IRandom is provided that RNG will be used to perform the Roll instead.</param>
      /// <returns>An integer result of the sum of the dice rolled including constants and scalars in the expression</returns>
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