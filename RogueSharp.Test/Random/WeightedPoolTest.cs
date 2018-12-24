using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class WeightedPoolTest
   {
      [TestMethod]
      public void Constructor_WhenIRandomArgumentIsNull_WillThrowArgumentNullException()
      {
         Assert.ThrowsException<ArgumentNullException>( () => new WeightedPool<int>( null ) );
      }

      [TestMethod]
      public void Add_WhenItemArgumentIsNull_WillThrowArgumentNullException()
      {
         WeightedPool<string> pool = new WeightedPool<string>();

         Assert.ThrowsException<ArgumentNullException>( () => pool.Add( null, 1 ) );
      }

      [TestMethod]
      public void Add_WhenWeightIs0_WillThrowArgumentException()
      {
         WeightedPool<int> pool = new WeightedPool<int>();

         Assert.ThrowsException<ArgumentException>( () => pool.Add( 12, 0 ) );
      }

      [TestMethod]
      public void Add_WhenWeightIsNegative_WillThrowArgumentException()
      {
         WeightedPool<int> pool = new WeightedPool<int>();

         Assert.ThrowsException<ArgumentException>( () => pool.Add( 12, -5 ) );
      }

      [TestMethod]
      public void Count_WhenTwoItemsAddedToEmptyPool_WillBe2()
      {
         WeightedPool<string> pool = new WeightedPool<string>();
         pool.Add( "Thing 1", 1 );
         pool.Add( "Thing 2", 1 );

         Assert.AreEqual( 2, pool.Count );
      }

      [TestMethod]
      public void Select_WhenPoolHas0Items_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Select() );
      }

      [TestMethod]
      public void Draw_WhenPoolHas0Items_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Draw() );
      }

      [TestMethod]
      public void Clear_WhenPoolHas2Items_WillHaveCount0()
      {
         WeightedPool<string> pool = new WeightedPool<string>();
         pool.Add( "Thing 1", 1 );
         pool.Add( "Thing 2", 1 );

         pool.Clear();

         Assert.AreEqual( 0, pool.Count );
      }
   }
}
