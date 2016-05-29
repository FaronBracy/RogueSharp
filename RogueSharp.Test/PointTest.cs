using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RogueSharp.Test
{
   [TestClass]
   public class PointTest
   {
      [TestMethod]
      public void Constructor_WithXAndYValues_BothValuesAreSet()
      {
         int x = 10;
         int y = 5;

         Point point = new Point( x, y );

         Assert.AreEqual( x, point.X );
         Assert.AreEqual( y, point.Y );
      }

      [TestMethod]
      public void Constructor_WithOneValue_BothXAndYAreSetToSameValue()
      {
         int value = 10;

         Point point = new Point( value );

         Assert.AreEqual( value, point.X );
         Assert.AreEqual( value, point.Y );
      }

      [TestMethod]
      public void Zero_StaticFactoryMethod_ReturnsNewPointWithXAndYValuesOfZero()
      {
         Point point = Point.Zero;

         Assert.AreEqual( 0, point.X );
         Assert.AreEqual( 0, point.Y );
      }

      [TestMethod]
      public void OperatorPlus_TwoPoints_ReturnsNewPointWithAddedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = point1 + point2;

         Assert.AreEqual( 10, point3.X );
         Assert.AreEqual( 20, point3.Y );
      }

      [TestMethod]
      public void OperatorMinus_TwoPoints_ReturnsNewPointWithSubtractedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = point1 - point2;

         Assert.AreEqual( 0, point3.X );
         Assert.AreEqual( 10, point3.Y );
      }

      [TestMethod]
      public void OperatorStar_TwoPoints_ReturnsNewPointWithMultipliedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = point1 * point2;

         Assert.AreEqual( 25, point3.X );
         Assert.AreEqual( 75, point3.Y );
      }

      [TestMethod]
      public void OperatorSlash_TwoPoints_ReturnsNewPointWithDividedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = point1 / point2;

         Assert.AreEqual( 1, point3.X );
         Assert.AreEqual( 3, point3.Y );
      }

      [TestMethod]
      public void OperatorEqualsEquals_TwoPointsWithSameXAndYValues_ReturnsTrue()
      {
         Point point1 = new Point( 5, 5 );
         Point point2 = new Point( 5, 5 );

         Assert.IsTrue( point1 == point2 );
      }

      [TestMethod]
      public void OperatorEqualsEquals_TwoPointsWithDifferentXAndYValues_ReturnsFalse()
      {
         Point point1 = new Point( 10, 10 );
         Point point2 = new Point( 5, 5 );

         Assert.IsFalse( point1 == point2 );
      }

      [TestMethod]
      public void OperatorEqualsEquals_TwoPointsWithDifferentXValues_ReturnsFalse()
      {
         Point point1 = new Point( 10, 5 );
         Point point2 = new Point( 5, 5 );

         Assert.IsFalse( point1 == point2 );
      }

      [TestMethod]
      public void OperatorEqualsEquals_TwoPointsWithDifferentYValues_ReturnsFalse()
      {
         Point point1 = new Point( 5, 10 );
         Point point2 = new Point( 5, 5 );

         Assert.IsFalse( point1 == point2 );
      }

      [TestMethod]
      public void OperatorNotEquals_TwoPointsWithSameXAndYValues_ReturnsFalse()
      {
         Point point1 = new Point( 5, 5 );
         Point point2 = new Point( 5, 5 );

         Assert.IsFalse( point1 != point2 );
      }

      [TestMethod]
      public void OperatorNotEquals_TwoPointsWithDifferentXAndYValues_ReturnsTrue()
      {
         Point point1 = new Point( 10, 10 );
         Point point2 = new Point( 5, 5 );

         Assert.IsTrue( point1 != point2 );
      }

      [TestMethod]
      public void OperatorNotEquals_TwoPointsWithDifferentXValues_ReturnsTrue()
      {
         Point point1 = new Point( 10, 5 );
         Point point2 = new Point( 5, 5 );

         Assert.IsTrue( point1 != point2 );
      }

      [TestMethod]
      public void OperatorNotEquals_TwoPointsWithDifferentYValues_ReturnsTrue()
      {
         Point point1 = new Point( 5, 10 );
         Point point2 = new Point( 5, 5 );

         Assert.IsTrue( point1 != point2 );
      }

      [TestMethod]
      public void Add_TwoPoints_ReturnsNewPointWithAddedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = Point.Add( point1, point2 );

         Assert.AreEqual( 10, point3.X );
         Assert.AreEqual( 20, point3.Y );
      }

      [TestMethod]
      public void Subtract_TwoPoints_ReturnsNewPointWithSubtractedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = Point.Subtract( point1, point2 );

         Assert.AreEqual( 0, point3.X );
         Assert.AreEqual( 10, point3.Y );
      }

      [TestMethod]
      public void Multiply_TwoPoints_ReturnsNewPointWithMultipliedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = Point.Multiply( point1, point2 );

         Assert.AreEqual( 25, point3.X );
         Assert.AreEqual( 75, point3.Y );
      }

      [TestMethod]
      public void Divide_TwoPoints_ReturnsNewPointWithDividedXAndYValues()
      {
         Point point1 = new Point( 5, 15 );
         Point point2 = new Point( 5, 5 );

         Point point3 = Point.Divide( point1, point2 );

         Assert.AreEqual( 1, point3.X );
         Assert.AreEqual( 3, point3.Y );
      }

      [TestMethod]
      public void Equals_TwoPointsWithSameXAndYValues_ReturnsTrue()
      {
         Point point1 = new Point( 5, 5 );
         Point point2 = new Point( 5, 5 );

         Assert.IsTrue( point1.Equals( point2 ) );
      }

      [TestMethod]
      public void Equals_TwoPointsWithDifferentXAndYValues_ReturnsFalse()
      {
         Point point1 = new Point( 10, 10 );
         Point point2 = new Point( 5, 5 );

         Assert.IsFalse( point1.Equals( point2 ) );
      }

      [TestMethod]
      public void Equals_TwoPointsOneCastAsObjectWithSameXAndYValuesOneCastAsObject_ReturnsTrue()
      {
         Point point = new Point( 5, 5 );
         object obj = new Point( 5, 5 );

         Assert.IsTrue( point.Equals( obj ) );
      }

      [TestMethod]
      public void Equals_TwoPointsOneCastAsObjectWithDifferentXAndYValues_ReturnsFalse()
      {
         Point point = new Point( 10, 10 );
         object obj = new Point( 5, 5 );

         Assert.IsFalse( point.Equals( obj ) );
      }

      [TestMethod]
      public void ToString_PointX5Y10_ExpectedString()
      {
         string expected = "{X:10 Y:5}";
         Point point = new Point( 10, 5 );

         Assert.AreEqual( expected, point.ToString() );
      }

      [TestMethod]
      public void Distance_TwoPointsInHypotenuseOfPythagoreanTriangle_Returns5()
      {
         float expectedDistance = 5.0f;
         Point point1 = new Point( 0, 3 );
         Point point2 = new Point( 4, 0 );

         float distance = Point.Distance( point1, point2 );

         Assert.AreEqual( expectedDistance, distance );
      }

      [TestMethod]
      public void Distance_TwoPoints_ExpectedFloat()
      {
         float expectedDistance = 7.615773f;
         Point point1 = new Point( 3, 7 );
         Point point2 = new Point( -4, 4 );

         float distance = Point.Distance( point1, point2 );

         Assert.AreEqual( expectedDistance, distance );
      }

      [TestMethod]
      public void Negate_Point_ReturnsNewPointWithExpectedVectionInversion()
      {
         Point expectedPoint = new Point( 3, -7 );
         Point point1 = new Point( -3, 7 );

         Point negatedPoint = Point.Negate( point1 );

         Assert.AreEqual( expectedPoint, negatedPoint );
      }
   }
}
