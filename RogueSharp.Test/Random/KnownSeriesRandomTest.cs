using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class KnownSeriesRandomTest
   {
      [TestMethod]
      public void Next_Call3TimesOnSeriesOf2Numbers_WrapsAroundAndReturnsFirstNumberAgain()
      {
         IRandom random = new KnownSeriesRandom( 1, 2 );

         Assert.AreEqual( 1, random.Next( 2 ) );
         Assert.AreEqual( 2, random.Next( 2 ) );
         Assert.AreEqual( 1, random.Next( 2 ) );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
      public void Next_SeriesWith10AndMaxValueOnly5_ThrowsArgumentOutOfRangeException()
      {
         IRandom random = new KnownSeriesRandom( 10 );

         random.Next( 5 );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentOutOfRangeException ) )]
      public void Next_SeriesWith10AndMinValueOf11_ThrowsArgumentOutOfRangeException()
      {
         IRandom random = new KnownSeriesRandom( 10 );

         random.Next( 11, 20 );
      }

      [TestMethod]
      public void Restore_AfterGeneratingThreeNumbers_RegeneratesSameThreeNumbers()
      {
         IRandom random = new KnownSeriesRandom( 1, 2, 3, 4, 5, 6 );
         for ( int i = 0; i < 25; i++ )
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
