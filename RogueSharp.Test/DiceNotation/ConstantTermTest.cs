using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.DiceNotation.Terms;

namespace RogueSharp.Test.DiceNotation
{
   [TestClass]
   public class ConstantTermTest
   {
      [TestMethod]
      public void ToString_ConstantTerm_ReturnsValueOfConstantOnly()
      {
         const int constant = 5;

         var constantTerm = new ConstantTerm( constant );

         Assert.AreEqual( constant.ToString(), constantTerm.ToString() );
      }
   }
}