using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RogueSharp.Test
{
   [TestClass]
   public class RectangleTest
   {
      [TestMethod]
      public void Constructor_WithXAndYAndWitdthAndHeightValues_AllValuesAreSet()
      {
         int x = 10;
         int y = 5;
         int width = 15;
         int height = 8;

         Rectangle rectangle = new Rectangle( x, y, width, height );

         Assert.AreEqual( x, rectangle.X );
         Assert.AreEqual( y, rectangle.Y );
         Assert.AreEqual( width, rectangle.Width );
         Assert.AreEqual( height, rectangle.Height );
      }

      [TestMethod]
      public void Constructor_WithLocationAndSize_AllValuesAreSet()
      {
         int x = 10;
         int y = 5;
         int width = 15;
         int height = 8;
         Point location = new Point( x, y );
         Point size = new Point( width, height );

         Rectangle rectangle = new Rectangle( location, size );

         Assert.AreEqual( x, rectangle.X );
         Assert.AreEqual( y, rectangle.Y );
         Assert.AreEqual( width, rectangle.Width );
         Assert.AreEqual( height, rectangle.Height );
      }

      [TestMethod]
      public void Empty_Get_ReturnsRectangleWithAllValuesSetToZero()
      {
         Rectangle rectangle = Rectangle.Empty;

         Assert.AreEqual( 0, rectangle.X );
         Assert.AreEqual( 0, rectangle.Y );
         Assert.AreEqual( 0, rectangle.Width );
         Assert.AreEqual( 0, rectangle.Height );
      }

      [TestMethod]
      public void Left_WhenRectangleAtX0Y1_Returns0()
      {
         int x = 0;

         Rectangle rectangle = new Rectangle( x, 1, 10, 10 );

         Assert.AreEqual( x, rectangle.Left );
      }

      [TestMethod]
      public void Right_WhenRectangleAtX0Y1Width5_Returns5()
      {
         int x = 0;
         int width = 5;

         Rectangle rectangle = new Rectangle( x, 1, width, 10 );

         Assert.AreEqual( width, rectangle.Right );
      }

      [TestMethod]
      public void Top_WhenRectangleAtX1Y0_Returns0()
      {
         int y = 0;

         Rectangle rectangle = new Rectangle( 1, y, 10, 10 );

         Assert.AreEqual( y, rectangle.Top );
      }

      [TestMethod]
      public void Bottom_WhenRectangleAtX1Y0Height5_Returns5()
      {
         int y = 0;
         int height = 5;

         Rectangle rectangle = new Rectangle( 1, y, 10, height );

         Assert.AreEqual( height, rectangle.Bottom );
      }

      [TestMethod]
      public void Location_GetWhenRectangleAtX10Y15_ReturnsPointX10Y15()
      {
         Point location = new Point( 10, 15 );

         Rectangle rectangle = new Rectangle( location, new Point( 3, 5 ) );

         Assert.AreEqual( location, rectangle.Location );
      }

      [TestMethod]
      public void Location_SetToX10Y15WhenRectangleIsEmpty_XAndYValuesAreSet()
      {
         int x = 10;
         int y = 15;
         Rectangle rectangle = Rectangle.Empty;

         rectangle.Location = new Point( x, y );
         
         Assert.AreEqual( x, rectangle.X );
         Assert.AreEqual( y, rectangle.Y );
      }

      [TestMethod]
      public void Center_WhenRectangleAtX0Y0Width10Height10_ReturnsPointX5Y5()
      {
         Point center = new Point( 5, 5 );

         Rectangle rectangle = new Rectangle( 0, 0, 10, 10 );

         Assert.AreEqual( center, rectangle.Center );
      }

      [TestMethod]
      public void IsEmpty_WhenAllRectangleValuesAre0_ReturnsTrue()
      {
         Rectangle rectangle = new Rectangle( 0, 0, 0, 0 );

         Assert.IsTrue( rectangle.IsEmpty );
         Assert.AreEqual( Rectangle.Empty, rectangle );
      }
   }
}
