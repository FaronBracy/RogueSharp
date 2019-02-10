using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class StringGeneratorTest
   {
      [TestMethod]
      public void AddWordPool_WhenWordPoolIsNull_ThrowsArgumentNullException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentNullException>( () => stringGenerator.AddWordPool( "color", null ) );
      }
   }
}
