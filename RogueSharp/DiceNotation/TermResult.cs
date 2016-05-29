namespace RogueSharp.DiceNotation
{
   /// <summary>
   /// The TermResult class represents a single result of one of the terms in a DiceExpression
   /// </summary>
   public class TermResult
   {
      /// <summary>
      /// An integer value that could be used to multiply the result of this term by
      /// </summary>
      public int Scalar { get; set; }

      /// <summary>
      /// The integer total for this term
      /// </summary>
      public int Value { get; set; }

      /// <summary>
      /// A string representing the type of this Term. Possible values are "constant" or "d(sides)"
      /// In 1d6 + 5, the 1d6 term is of type "d6" and the 5 term is of type "constant"
      /// </summary>
      public string TermType { get; set; }
   }
}