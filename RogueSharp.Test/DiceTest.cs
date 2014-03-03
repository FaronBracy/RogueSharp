using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test
{
   [TestClass]
   public class DiceTest
   {
      [TestMethod]
      public void Roll_20D6_EachDieBetween1And6()
      {
         List<IDie> diceCollection = new List<IDie>();
         for ( int i = 0; i < 20; i++ )
         {
            IDie die = new Die( new DotNetRandom(), 6 );
            diceCollection.Add( die );
         }
         IDice dice = new Dice( diceCollection );

         foreach ( int result in dice.Roll() )
         {
            Assert.IsTrue( result >= 1 && result <= 6, string.Format( "Expected result between 1 and 6 but was: {0}", result.ToString() ) );
         }
      }

      [TestMethod]
      public void AddDie_3DiceToStart_4DiceRolled()
      {
         List<IDie> diceCollection = new List<IDie>();
         for ( int i = 0; i < 3; i++ )
         {
            IDie die = new Die( new DotNetRandom(), 6 );
            diceCollection.Add( die );
         }
         IDice dice = new Dice( diceCollection );

         dice.AddDie( new Die( new DotNetRandom(), 6 ) );

         var results = dice.Roll();
         Assert.AreEqual( 4, results.Count );
      }


      [TestMethod]
      public void RemoveDie_3DiceToStart_2DiceRolled()
      {
         IDie die1 = new Die( new DotNetRandom(), 6 );
         IDie die2 = new Die( new DotNetRandom(), 6 );
         IDie die3 = new Die( new DotNetRandom(), 6 );
         IDice dice = new Dice( new Collection<IDie> { die1, die2, die3 } );

         dice.RemoveDie( die1 );

         var results = dice.Roll();
         Assert.AreEqual( 2, results.Count );
      }
   }
}
