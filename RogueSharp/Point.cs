using System;

namespace RogueSharp
{
   public class Point : IEquatable<Point>
   {
      private static readonly Point _zeroPoint = new Point();
      public int X { get; set; }
      public int Y { get; set; }

      public Point()
      {
      }

      public Point( int x, int y )
      {
         X = x;
         Y = y;
      }

      public static Point Zero
      {
         get
         {
            return _zeroPoint;
         }
      }

      public bool Equals( Point other )
      {
         return ( ( X == other.X ) && ( Y == other.Y ) );
      }

      public static bool operator ==( Point a, Point b )
      {
         return a.Equals( b );
      }

      public static bool operator !=( Point a, Point b )
      {
         return !a.Equals( b );
      }

      public override bool Equals( object obj )
      {
         return ( obj is Point ) ? Equals( (Point) obj ) : false;
      }

      public override int GetHashCode()
      {
         return X ^ Y;
      }

      public override string ToString()
      {
         return string.Format( "{{X:{0} Y:{1}}}", X, Y );
      }
   }
}