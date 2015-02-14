using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class DotNetRandomTest
   {
      [TestMethod]
      public void Restore_AfterGeneratingThreeNumbers_RegeneratesSameThreeNumbers()
      {
         IRandom random = new DotNetRandom();
         for ( int i = 0; i < 25; i++ )
         {
            random.Next( 1000 );
         }
         RandomState state = random.Save();
         int first = random.Next( 1000 );
         int second = random.Next( 1000 );
         int third = random.Next( 1000 );

         random.Restore( state );

         Assert.AreEqual( first, random.Next( 1000 ) );
         Assert.AreEqual( second, random.Next( 1000 ) );
         Assert.AreEqual( third, random.Next( 1000 ) );
      }
   }
}
