using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.DiceNotation;
using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.Test.DiceNotation
{
   [TestClass]
   public class DiceExpressionTest
   {
      [TestMethod]
      public void ContainsAndReturnsCorrectNumberOfValues()
      {
         DiceExpression diceExpression = new DiceExpression()
             .Constant( 5 )
             .Die( 8 )
             .Dice( 4, 6, choose: 3 );

         DiceResult result = diceExpression.Roll( new StandardDieRoller( new System.Random() ) );

         const int expectedNumberOfTerms = 1 + 1 + 3;

         Assert.AreEqual( expectedNumberOfTerms, result.Results.Count );
      }

      [TestMethod]
      public void ToStringReturnsTermsSeparatedByPlus()
      {
         DiceExpression diceExpression = new DiceExpression().Constant( 10 ).Die( 8, -1 );

         string toString = diceExpression.ToString();

         Assert.AreEqual( "10 + -1*1d8", toString );
      }
   }
}