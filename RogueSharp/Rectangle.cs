using System;

namespace RogueSharp
{
   /// <summary>
   /// A struct that defines a rectangle
   /// </summary>
   public struct Rectangle : IEquatable<Rectangle>
   {
      private static readonly Rectangle _emptyRectangle = new Rectangle();

      /// <summary>
      /// Specifies the Height of the Rectangle
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// Specifies the Width of the Rectangle
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// Specifies the x-coordinate of the Rectangle with 0 being to the left
      /// and increasing as the Rectangle is moved to the right
      /// </summary>
      public int X { get; set; }

      /// <summary>
      /// Specifies the y-coordinate of the Rectangle with 0 being at the top 
      /// and increasing as the Rectangle is moved downwards
      /// </summary>
      public int Y { get; set; }


      #region Constructors

      /// <summary>
      /// Creates a new instance of <see cref="Rectangle"/> struct, with the specified
      /// position, width, and height.
      /// </summary>
      /// <param name="x">The x coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
      /// <param name="y">The y coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
      /// <param name="width">The width of the created <see cref="Rectangle"/>.</param>
      /// <param name="height">The height of the created <see cref="Rectangle"/>.</param>
      public Rectangle( int x, int y, int width, int height ) 
         : this()
      {
         this.X = x;
         this.Y = y;
         this.Width = width;
         this.Height = height;
      }

      /// <summary>
      /// Creates a new instance of <see cref="Rectangle"/> struct, with the specified
      /// location and size.
      /// </summary>
      /// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="Rectangle"/>.</param>
      /// <param name="size">The width and height of the created <see cref="Rectangle"/>.</param>
      public Rectangle( Point location, Point size ) 
         : this()
      {
         this.X = location.X;
         this.Y = location.Y;
         this.Width = size.X;
         this.Height = size.Y;
      }

      #endregion

      /// <summary>
      /// Returns a Rectangle with all of its values set to zero
      /// </summary>
      public static Rectangle Empty
      {
         get
         {
            return _emptyRectangle;
         }
      }

      /// <summary>
      /// Returns the x-coordinate of the left side of the rectangle
      /// </summary>
      public int Left
      {
         get
         {
            return X;
         }
      }

      /// <summary>
      /// Returns the x-coordinate of the right side of the rectangle
      /// </summary>
      public int Right
      {
         get
         {
            return ( X + Width );
         }
      }

      /// <summary>
      /// Returns the y-coordinate of the top of the rectangle
      /// </summary>
      public int Top
      {
         get
         {
            return Y;
         }
      }

      /// <summary>
      /// Returns the y-coordinate of the bottom of the rectangle
      /// </summary>
      public int Bottom
      {
         get
         {
            return ( Y + Height );
         }
      }

      /// <summary>
      /// Gets or sets the Point representing the upper-left value of the Rectangle
      /// </summary>
      public Point Location
      {
         get
         {
            return new Point( X, Y );
         }
         set
         {
            X = value.X;
            Y = value.Y;
         }
      }

      /// <summary>
      /// Returns the Point that specifies the center of the rectangle
      /// </summary>
      public Point Center
      {
         get
         {
            return new Point( X + ( Width / 2 ), Y + ( Height / 2 ) );
         }
      }

      /// <summary>
      /// Returns a value that indicates whether the Rectangle is empty
      /// true if the Rectangle is empty; otherwise false
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return ( ( ( ( Width == 0 ) && ( Height == 0 ) ) && ( X == 0 ) ) && ( Y == 0 ) );
         }
      }

      /// <summary>
      /// Determines whether two Rectangle instances are equal
      /// </summary>
      /// <param name="other">The Rectangle to compare this instance to</param>
      /// <returns>True if the instances are equal; False otherwise</returns>
      public bool Equals( Rectangle other )
      {
         return this == other;
      }

      /// <summary>
      /// Compares two rectangles for equality
      /// </summary>
      /// <param name="a">Rectangle on the left side of the equals sign</param>
      /// <param name="b">Rectangle on the right side of the equals sign</param>
      /// <returns>True if the rectangles are equal; False otherwise</returns>
      public static bool operator ==( Rectangle a, Rectangle b )
      {
         return ( ( a.X == b.X ) && ( a.Y == b.Y ) && ( a.Width == b.Width ) && ( a.Height == b.Height ) );
      }

      /// <summary>
      /// Determines whether this Rectangle contains a specified point represented by its x and y-coordinates
      /// </summary>
      /// <param name="x">The x-coordinate of the specified point</param>
      /// <param name="y">The y-coordinate of the specified point</param>
      /// <returns>True if the specified point is contained within this Rectangle; False otherwise</returns>
      public bool Contains( int x, int y )
      {
         return ( ( ( ( X <= x ) && ( x < ( X + Width ) ) ) && ( Y <= y ) ) && ( y < ( Y + Height ) ) );
      }

      /// <summary>
      /// Determines whether this Rectangle contains a specified Point
      /// </summary>
      /// <param name="value">The Point to evaluate</param>
      /// <returns>True if the specified Point is contained within this Rectangle; False otherwise</returns>
      public bool Contains( Point value )
      {
         return ( ( ( ( X <= value.X ) && ( value.X < ( X + Width ) ) ) && ( Y <= value.Y ) ) && ( value.Y < ( Y + Height ) ) );
      }

      /// <summary>
      /// Determines whether this Rectangle entirely contains the specified Rectangle
      /// </summary>
      /// <param name="value">The Rectangle to evaluate</param>
      /// <returns>True if this Rectangle entirely contains the specified Rectangle; False otherwise</returns>
      public bool Contains( Rectangle value )
      {
         return ( ( ( ( X <= value.X ) && ( ( value.X + value.Width ) <= ( X + Width ) ) ) && ( Y <= value.Y ) )
                  && ( ( value.Y + value.Height ) <= ( Y + Height ) ) );
      }

      /// <summary>
      /// Compares two rectangles for inequality
      /// </summary>
      /// <param name="a">Rectangle on the left side of the equals sign</param>
      /// <param name="b">Rectangle on the right side of the equals sign</param>
      /// <returns>True if the rectangles are not equal; False otherwise</returns>
      public static bool operator !=( Rectangle a, Rectangle b )
      {
         return !( a == b );
      }

      /// <summary>
      /// Changes the position of the Rectangles by the values of the specified Point
      /// </summary>
      /// <param name="offsetPoint">The values to adjust the position of the Rectangle by</param>
      public void Offset( Point offsetPoint )
      {
         X += offsetPoint.X;
         Y += offsetPoint.Y;
      }

      /// <summary>
      /// Changes the position of the Rectangle by the specified x and y offsets
      /// </summary>
      /// <param name="offsetX">Change in the x-position</param>
      /// <param name="offsetY">Change in the y-position</param>
      public void Offset( int offsetX, int offsetY )
      {
         X += offsetX;
         Y += offsetY;
      }

      /// <summary>
      /// Pushes the edges of the Rectangle out by the specified horizontal and vertical values
      /// </summary>
      /// <param name="horizontalValue">Value to push the sides out by</param>
      /// <param name="verticalValue">Value to push the top and bottom out by</param>
      /// <exception cref="OverflowException">Thrown if the new width or height exceed Int32.MaxValue, or new X or Y are smaller than int32.MinValue</exception>
      public void Inflate( int horizontalValue, int verticalValue )
      {
         X -= horizontalValue;
         Y -= verticalValue;
         Width += horizontalValue * 2;
         Height += verticalValue * 2;
      }

      /// <summary>
      /// Compares whether current instance is equal to specified <see cref="Object"/>.
      /// </summary>
      /// <param name="obj">The <see cref="Object"/> to compare.</param>
      /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
      public override bool Equals( object obj )
      {
         return ( obj is Rectangle ) && this == ( (Rectangle) obj );
      }

      /// <summary>
      /// Returns a string that represents the current Rectangle
      /// </summary>
      /// <returns>A string that represents the current Rectangle</returns>
      public override string ToString()
      {
         return string.Format( "{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height );
      }

      /// <summary>
      /// Gets the hash code for this object which can help for quick checks of equality
      /// or when inserting this Rectangle into a hash-based collection such as a Dictionary or Hashtable 
      /// </summary>
      /// <returns>An integer hash used to identify this Rectangle</returns>
      public override int GetHashCode()
      {
         return ( X ^ Y ^ Width ^ Height );
      }

      /// <summary>
      /// Determines whether this Rectangle intersects with the specified Rectangle
      /// </summary>
      /// <param name="value">The Rectangle to evaluate</param>
      /// <returns>True if the specified Rectangle intersects with this one; False otherwise</returns>
      public bool Intersects( Rectangle value )
      {
         return value.Left < Right &&
                Left < value.Right &&
                value.Top < Bottom &&
                Top < value.Bottom;
      }

      /// <summary>
      /// Determines whether this Rectangle intersects with the specified Rectangle
      /// </summary>
      /// <param name="value">The Rectangle to evaluate</param>
      /// <param name="result">True if the specified Rectangle intersects with this one; False otherwise</param>
      public void Intersects( ref Rectangle value, out bool result )
      {
         result = value.Left < Right &&
                  Left < value.Right &&
                  value.Top < Bottom &&
                  Top < value.Bottom;
      }

      /// <summary>
      /// Creates a Rectangle defining the area where one Rectangle overlaps with another Rectangle
      /// </summary>
      /// <param name="value1">The first Rectangle to compare</param>
      /// <param name="value2">The second Rectangle to compare</param>
      /// <returns>The area where the two specified Rectangles overlap. If the two Rectangles do not overlap the resulting Rectangle will be Empty</returns>
      public static Rectangle Intersect( Rectangle value1, Rectangle value2 )
      {
         Rectangle rectangle;
         Intersect( ref value1, ref value2, out rectangle );
         return rectangle;
      }

      /// <summary>
      /// Creates a Rectangle defining the area where one Rectangle overlaps with another Rectangle
      /// </summary>
      /// <param name="value1">The first Rectangle to compare</param>
      /// <param name="value2">The second Rectangle to compare</param>
      /// <param name="result">The area where the two specified Rectangles overlap. If the two Rectangles do not overlap the resulting Rectangle will be Empty</param>
      public static void Intersect( ref Rectangle value1, ref Rectangle value2, out Rectangle result )
      {
         if ( value1.Intersects( value2 ) )
         {
            int rightSide = Math.Min( value1.X + value1.Width, value2.X + value2.Width );
            int leftSide = Math.Max( value1.X, value2.X );
            int topSide = Math.Max( value1.Y, value2.Y );
            int bottomSide = Math.Min( value1.Y + value1.Height, value2.Y + value2.Height );
            result = new Rectangle( leftSide, topSide, rightSide - leftSide, bottomSide - topSide );
         }
         else
         {
            result = new Rectangle( 0, 0, 0, 0 );
         }
      }

      /// <summary>
      /// Creates a new Rectangle that exactly contains the specified two Rectangles
      /// </summary>
      /// <param name="value1">The first Rectangle to contain</param>
      /// <param name="value2">The second Rectangle to contain</param>
      /// <returns>A new Rectangle that exactly contains the specified two Rectangles</returns>
      public static Rectangle Union( Rectangle value1, Rectangle value2 )
      {
         int x = Math.Min( value1.X, value2.X );
         int y = Math.Min( value1.Y, value2.Y );
         return new Rectangle( x, y, Math.Max( value1.Right, value2.Right ) - x, Math.Max( value1.Bottom, value2.Bottom ) - y );
      }

      /// <summary>
      /// Creates a new <see cref="Rectangle"/> that completely contains two other rectangles.
      /// </summary>
      /// <param name="value1">The first <see cref="Rectangle"/>.</param>
      /// <param name="value2">The second <see cref="Rectangle"/>.</param>
      /// <param name="result">The union of the two rectangles as an output parameter.</param>
      public static void Union( ref Rectangle value1, ref Rectangle value2, out Rectangle result )
      {
         result = Union( value1, value2 );
      }
   }
}