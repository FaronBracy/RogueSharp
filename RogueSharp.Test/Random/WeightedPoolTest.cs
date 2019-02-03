using System;
using System.Linq;
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
      public void Add_WhenTotalWeightGoesOverMaximumIntValue_WillThrowOverflowException()
      {
         WeightedPool<int> pool = new WeightedPool<int>();
         pool.Add( 1, int.MaxValue - 10 );
         pool.Add( 2, 10 );

         Assert.ThrowsException<OverflowException>( () => pool.Add( 3, 1 ) );
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
      public void Choose_WhenCloneFuncWasNotDefined_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom );
         pool.Add( 1, 1 );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Choose() );
      }

      [TestMethod]
      public void Choose_WhenPoolHas0Items_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom, x => x );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Choose() );
      }

      [TestMethod]
      public void Choose_WhenPoolHas1Item_WillGetCloneOfItemWithDifferentReference()
      {
         WeightedPool<PlayingCard> pool = new WeightedPool<PlayingCard>( Singleton.DefaultRandom, PlayingCard.Clone );
         PlayingCard kingOfHearts = new PlayingCard
         {
            DisplayName = "King of Hearts", FaceValue = 13, Suit = PlayingCard.Suits.Hearts
         };
         pool.Add( kingOfHearts, 1 );

         PlayingCard selectedCard = pool.Choose();

         Assert.AreNotEqual( kingOfHearts, selectedCard );
         Assert.AreEqual( kingOfHearts.FaceValue, selectedCard.FaceValue );
         Assert.AreEqual( kingOfHearts.DisplayName, selectedCard.DisplayName );
         Assert.AreEqual( kingOfHearts.Suit, selectedCard.Suit );
         Assert.AreEqual( 1, pool.Count );
      }

      [TestMethod]
      public void Draw_WhenPoolHas0Items_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Draw() );
      }

      [TestMethod]
      public void Draw_CalledTwiceWhenPoolHas1Item_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( Singleton.DefaultRandom );
         pool.Add( 1, 12 );
         int drawnItem = pool.Draw();

         Assert.AreEqual( 1, drawnItem );
         Assert.ThrowsException<InvalidOperationException>( () => pool.Draw() );
      }

      [TestMethod]
      public void Draw_WhenUsingRandomBiggerThanTotalWeight_WillThrowInvalidOperationException()
      {
         WeightedPool<int> pool = new WeightedPool<int>( new BadRandom( 13 ) );
         pool.Add( 1, 12 );

         Assert.ThrowsException<InvalidOperationException>( () => pool.Draw() );
      }

      [TestMethod]
      public void Draw_Called7TimesOnPoolOf7Items_WillDrawAllOfThemAndCountWillBe0()
      {
         WeightedPool<string> pool = new WeightedPool<string>( Singleton.DefaultRandom );
         pool.Add( "White", 5 );
         pool.Add( "Blue", 5 );
         pool.Add( "Black", 5 );
         pool.Add( "Red", 5 );
         pool.Add( "Green", 5 );
         pool.Add( "Artifact", 3 );
         pool.Add( "DualColor", 1 );

         string[] drawn = new string[7];
         drawn[0] = pool.Draw();
         drawn[1] = pool.Draw();
         drawn[2] = pool.Draw();
         drawn[3] = pool.Draw();
         drawn[4] = pool.Draw();
         drawn[5] = pool.Draw();
         drawn[6] = pool.Draw();

         Assert.IsTrue( drawn.Contains( "White" ) );
         Assert.IsTrue( drawn.Contains( "Blue" ) );
         Assert.IsTrue( drawn.Contains( "Black" ) );
         Assert.IsTrue( drawn.Contains( "Red" ) );
         Assert.IsTrue( drawn.Contains( "Green" ) );
         Assert.IsTrue( drawn.Contains( "Artifact" ) );
         Assert.IsTrue( drawn.Contains( "DualColor" ) );
         Assert.AreEqual( 0, pool.Count );
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

      private class PlayingCard
      {
         public enum Suits
         {
            None = 0,
            Hearts = 1,
            Spades = 2,
            Diamonds = 3,
            Clubs = 4
         }

         public string DisplayName
         {
            get;
            set;
         }

         public Suits Suit
         {
            get;
            set;
         }

         public int FaceValue
         {
            get;
            set;
         }

         public static PlayingCard Clone( PlayingCard other )
         {
            return new PlayingCard {
               DisplayName = other.DisplayName,
               Suit = other.Suit,
               FaceValue = other.FaceValue
            };
         }
      }

      private class BadRandom : IRandom
      {
         private readonly int _alwaysReturnThisValue;
         public BadRandom( int alwaysReturnThisValue )
         {
            _alwaysReturnThisValue = alwaysReturnThisValue;
         }

         public int Next( int maxValue )
         {
            throw new NotImplementedException();
         }

         public int Next( int minValue, int maxValue )
         {
            return _alwaysReturnThisValue;
         }

         public RandomState Save()
         {
            throw new NotImplementedException();
         }

         public void Restore( RandomState state )
         {
            throw new NotImplementedException();
         }
      }
   }
}
