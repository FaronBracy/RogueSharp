using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.DiceNotation;
using RogueSharp.DiceNotation.Exceptions;
using RogueSharp.DiceNotation.Rollers;
using RogueSharp.DiceNotation.Terms;

namespace RogueSharp.Test.DiceNotation
{
   [TestClass]
   public class DiceTermTest
   {
      [TestMethod]
      public void SetsConstructorValues()
      {
         const int multiplicity = 3;
         const int sides = 4;
         const int scalar = 5;

         var diceTerm = new DiceTerm( multiplicity, sides, scalar );

         Assert.AreEqual( multiplicity, diceTerm.Multiplicity );
         Assert.AreEqual( sides, diceTerm.Sides );
         Assert.AreEqual( scalar, diceTerm.Scalar );
      }

      [TestMethod]
      [ExpectedException(typeof (ImpossibleDieException))]
      public void ExceptionIsThrownWhenConstructingImpossibleDice()
      {
         const int invalidNumberOfSides = -1;

         var dieWithInvalidSides = new DiceTerm( 1, invalidNumberOfSides, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( InvalidChooseException ) )]
      public void ExceptionIsThrownWhenChoosingMoreDiceThanRolled()
      {
         const int multiplicity = 1;
         const int choose = multiplicity + 1;

         var chooseMoreDiceThanRolled = new DiceTerm( multiplicity, 6, choose, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( InvalidChooseException ) )]
      public void ExceptionIsThrownWhenChoosingLessThanNoDice()
      {
         const int multiplicity = 1;
         const int choose = -1;

         var chooseLessThanAnyDice = new DiceTerm( multiplicity, 6, choose, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( InvalidMultiplicityException ) )]
      public void ExceptionIsThrownWhenConstructingDiceWithInvalidMultiplicity()
      {
         const int invalidMultiplicity = -1;

         var rollLessThanNoDice = new DiceTerm( invalidMultiplicity, 6, 1 );
      }

      [TestMethod]
      public void ReturnsMultiplicityResultsWhenNoChooseSpecified()
      {
         const int multiplicity = 6;
         const int sides = 6;

         var diceTerm = new DiceTerm( multiplicity, sides, 1 );

         var dieRoller = new MaxDieRoller();

         IEnumerable<TermResult> results = diceTerm.GetResults( dieRoller );

         Assert.AreEqual( multiplicity, results.Count() );
      }

      [TestMethod]
      public void StringRepresentationIsCorrectForScalarOfOne()
      {
         const int multiplicity = 3;
         const int sides = 6;

         var diceTerm = new DiceTerm( multiplicity, sides, 1 );

         string stringRepresentation = diceTerm.ToString();

         Assert.AreEqual( "3d6", stringRepresentation );
      }

      [TestMethod]
      public void StringRepresentationIsCorrectForScalarOfTwo()
      {
         const int multiplicity = 3;
         const int sides = 6;
         const int scalar = 2;

         var diceTerm = new DiceTerm( multiplicity, sides, scalar );

         string stringRepresentation = diceTerm.ToString();

         Assert.AreEqual( "2*3d6", stringRepresentation );
      }

      [TestMethod]
      public void StringRepresentationIsCorrectForChoose()
      {
         var diceTerm = new DiceTerm( 4, 6, 3, 1 );

         string stringRepresentation = diceTerm.ToString();

         Assert.AreEqual( "4d6k3", stringRepresentation );
      }
   }
}