namespace RogueSharp.DiceNotation
{
   /// <summary>
   /// The DiceParser interface can be implemented to parse a string into a DiceExpression
   /// </summary>
   public interface IDiceParser
   {
      /// <summary>
      /// Create a new DiceExpression by parsing the specified string
      /// </summary>
      /// <param name="expression">A dice notation string expression. Ex. 3d6+3</param>
      /// <returns>A DiceExpression parsed from the specified string</returns>
      DiceExpression Parse( string expression );
   }
}