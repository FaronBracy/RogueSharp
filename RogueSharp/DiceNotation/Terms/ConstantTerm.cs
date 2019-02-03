using System.Collections.Generic;
using System.Globalization;
using RogueSharp.Random;

namespace RogueSharp.DiceNotation.Terms
{
   /// <summary>
   /// The ConstantTerm class represents a constant value in a DiceExpression
   /// </summary>
   /// <remarks>
   /// In the expression "2d6+5" the integer "5" is a ConstantTerm
   /// </remarks>
   public class ConstantTerm : IDiceExpressionTerm
   {
      private readonly int _constant;

      /// <summary>
      /// Construct a new instance of the ConstantTerm class using the specified integer
      /// </summary>
      /// <param name="constant">An integer representing the constant term</param>
      public ConstantTerm( int constant )
      {
         _constant = constant;
      }

      /// <summary>
      /// Gets the TermResult for this ConstantTerm which will always be a single result with a scalar of 1 and a value of the constant.
      /// </summary>
      /// <param name="random">Not used for this implementation of IDiceExpressionTerm</param>
      /// <returns>An IEnumerable of TermResult which will always have a single result with a scalar of 1 and a value of the constant</returns>
      public IEnumerable<TermResult> GetResults( IRandom random )
      {
         return GetResults();
      }

      /// <summary>
      /// Gets the TermResult for this ConstantTerm which will always be a single result with a scalar of 1 and a value of the constant.
      /// </summary>
      /// <returns>An IEnumerable of TermResult which will always have a single result with a scalar of 1 and a value of the constant</returns>
      public IEnumerable<TermResult> GetResults()
      {
         return new[] { new TermResult { Scalar = 1, Value = _constant, TermType = "constant" } };
      }

      /// <summary>
      /// Returns a string that represents this ConstantTerm
      /// </summary>
      /// <returns>A string representing this ConstantTerm</returns>
      public override string ToString()
      {
         return _constant.ToString( CultureInfo.CurrentCulture );
      }
   }
}