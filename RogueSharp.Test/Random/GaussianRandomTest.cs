using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class GaussianRandomTest
   {
      [TestMethod]
      public void Next_Min5Max20_GeneratesNumbersBetween5And20Inclusive()
      {
         IRandom random = new GaussianRandom();

         for ( int i = 0; i < 100; i++ )
         {
            int value = random.Next( 5, 20 );
            Assert.IsTrue( 5 <= value && 20 >= value );
         }
      }

      [TestMethod]
      public void Next_Max6_GeneratesNumbersBetween0And6Inclusive()
      {
         IRandom random = new GaussianRandom();

         for ( int i = 0; i < 100; i++ )
         {
            int value = random.Next( 6 );
            Assert.IsTrue( 0 <= value && 6 >= value );
         }
      }

      [TestMethod]
      public void Restore_AfterGeneratingThreeNumbers_RegeneratesSameThreeNumbers()
      {
         IRandom random = new GaussianRandom();
         for ( int i = 0; i < 4; i++ )
         {
            random.Next( 6 );
         }
         RandomState state = random.Save();
         int first = random.Next( 6 );
         int second = random.Next( 6 );
         int third = random.Next( 6 );

         random.Restore( state );

         Assert.AreEqual( first, random.Next( 6 ) );
         Assert.AreEqual( second, random.Next( 6 ) );
         Assert.AreEqual( third, random.Next( 6 ) );
      }
   }
}
