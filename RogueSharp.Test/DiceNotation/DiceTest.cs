using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.DiceNotation;

namespace RogueSharp.Test.DiceNotation
{
   [TestClass]
   public class DiceTest
   {
      [TestMethod]
      public void Roll_3D6_ResultBetween3And18()
      {
         int result = Dice.Roll( "3d6" );

         Assert.IsTrue( result >= 3 && result <= 18 );
      }

      [TestMethod]
      public void Roll_1D10Plus5_ResultBetween6And15()
      {
         int result = Dice.Roll( "1d10+5" );

         Assert.IsTrue( result >= 6 && result <= 15 );
      }

      [TestMethod]
      public void Roll_1D10Plus2d6Minus5_ResultBetweenNegative2And17()
      {
         int result = Dice.Roll( "1d10+2d6-5" );

         Assert.IsTrue( result >= -2 && result <= 17 );
      }
   }
}
