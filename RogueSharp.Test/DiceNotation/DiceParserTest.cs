using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.DiceNotation;

namespace RogueSharp.Test.DiceNotation
{
   [TestClass]
   public class DiceParserTest
   {
      private readonly DiceParser _diceParser;

      public DiceParserTest()
      {
         _diceParser = new DiceParser();
      }

      [TestMethod]
      public void HandlesOneDieTerm()
      {
         DiceExpression diceExpression = _diceParser.Parse( "3d6" );
         Assert.AreEqual( "3d6", diceExpression.ToString() );
      }

      [TestMethod]
      public void HandlesDieTermPlusConstant()
      {
         DiceExpression diceExpression = _diceParser.Parse( "3d6+5" );
         Assert.AreEqual( "3d6 + 5", diceExpression.ToString() );
      }

      [TestMethod]
      public void HandlesDieTermWithChooseAndScalar()
      {
         const string expression = "2 + 3*4d6k3";
         DiceExpression diceExpression = _diceParser.Parse( expression );
         Assert.AreEqual( expression, diceExpression.ToString() );
      }

      [TestMethod]
      public void HandlesDieWithImplicitMultiplicityOf1()
      {
         const string expression = "2 + 2*d6";
         const string expectedExpression = "2 + 2*1d6";

         DiceExpression diceExpression = _diceParser.Parse( expression );
         Assert.AreEqual( expectedExpression, diceExpression.ToString() );
      }

      [TestMethod]
      public void HandlesNegativeScalar()
      {
         const string expression = "2 + -2*1d6";

         DiceExpression diceExpression = _diceParser.Parse( expression );
         Assert.AreEqual( expression, diceExpression.ToString() );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentException ) )]
      public void RejectsExpressionWithInvalidCharacters()
      {
         const string expression = "2d6/2";
         _diceParser.Parse( expression );
      }
   }
}