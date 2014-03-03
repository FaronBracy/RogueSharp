using System;

namespace RogueSharp
{
   public class Rectangle : IEquatable<Rectangle>
   {
      private static readonly Rectangle _emptyRectangle = new Rectangle();
      public int Height { get; set; }
      public int Width { get; set; }

      public int X { get; set; }
      public int Y { get; set; }

      public Rectangle()
      {
      }

      public Rectangle( int x, int y, int width, int height )
      {
         X = x;
         Y = y;
         Width = width;
         Height = height;
      }

      public static Rectangle Empty
      {
         get
         {
            return _emptyRectangle;
         }
      }

      public int Left
      {
         get
         {
            return X;
         }
      }

      public int Right
      {
         get
         {
            return ( X + Width );
         }
      }

      public int Top
      {
         get
         {
            return Y;
         }
      }

      public int Bottom
      {
         get
         {
            return ( Y + Height );
         }
      }

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

      public Point Center
      {
         get
         {
            return new Point( X + ( Width / 2 ), Y + ( Height / 2 ) );
         }
      }

      public bool IsEmpty
      {
         get
         {
            return ( ( ( ( Width == 0 ) && ( Height == 0 ) ) && ( X == 0 ) ) && ( Y == 0 ) );
         }
      }

      public bool Equals( Rectangle other )
      {
         return this == other;
      }

      public static bool operator ==( Rectangle a, Rectangle b )
      {
         return ( ( a.X == b.X ) && ( a.Y == b.Y ) && ( a.Width == b.Width ) && ( a.Height == b.Height ) );
      }

      public bool Contains( int x, int y )
      {
         return ( ( ( ( X <= x ) && ( x < ( X + Width ) ) ) && ( Y <= y ) ) && ( y < ( Y + Height ) ) );
      }

      public bool Contains( Point value )
      {
         return ( ( ( ( X <= value.X ) && ( value.X < ( X + Width ) ) ) && ( Y <= value.Y ) ) && ( value.Y < ( Y + Height ) ) );
      }

      public bool Contains( Rectangle value )
      {
         return ( ( ( ( X <= value.X ) && ( ( value.X + value.Width ) <= ( X + Width ) ) ) && ( Y <= value.Y ) )
                  && ( ( value.Y + value.Height ) <= ( Y + Height ) ) );
      }

      public static bool operator !=( Rectangle a, Rectangle b )
      {
         return !( a == b );
      }

      public void Offset( Point offset )
      {
         X += offset.X;
         Y += offset.Y;
      }

      public void Offset( int offsetX, int offsetY )
      {
         X += offsetX;
         Y += offsetY;
      }

      public void Inflate( int horizontalValue, int verticalValue )
      {
         X -= horizontalValue;
         Y -= verticalValue;
         Width += horizontalValue * 2;
         Height += verticalValue * 2;
      }

      public override bool Equals( object obj )
      {
         return ( obj is Rectangle ) ? this == ( (Rectangle) obj ) : false;
      }

      public override string ToString()
      {
         return string.Format( "{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height );
      }

      public override int GetHashCode()
      {
         return ( X ^ Y ^ Width ^ Height );
      }

      public bool Intersects( Rectangle value )
      {
         return value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
      }

      public void Intersects( ref Rectangle value, out bool result )
      {
         result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
      }

      public static Rectangle Intersect( Rectangle value1, Rectangle value2 )
      {
         Rectangle rectangle;
         Intersect( ref value1, ref value2, out rectangle );
         return rectangle;
      }

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

      public static Rectangle Union( Rectangle value1, Rectangle value2 )
      {
         int x = Math.Min( value1.X, value2.X );
         int y = Math.Min( value1.Y, value2.Y );
         return new Rectangle( x, y, Math.Max( value1.Right, value2.Right ) - x, Math.Max( value1.Bottom, value2.Bottom ) - y );
      }

      public static void Union( ref Rectangle value1, ref Rectangle value2, out Rectangle result )
      {
         result = new Rectangle();
         result.X = Math.Min( value1.X, value2.X );
         result.Y = Math.Min( value1.Y, value2.Y );
         result.Width = Math.Max( value1.Right, value2.Right ) - result.X;
         result.Height = Math.Max( value1.Bottom, value2.Bottom ) - result.Y;
      }
   }
}