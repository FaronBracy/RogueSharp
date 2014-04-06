using System;

namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public class Point : IEquatable<Point>
   {
      private static readonly Point _zeroPoint = new Point();
      /// <summary>
      /// 
      /// </summary>
      public int X { get; set; }
      /// <summary>
      /// 
      /// </summary>
      public int Y { get; set; }
      /// <summary>
      /// 
      /// </summary>
      public Point()
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      public Point( int x, int y )
      {
         X = x;
         Y = y;
      }
      /// <summary>
      /// 
      /// </summary>
      public static Point Zero
      {
         get
         {
            return _zeroPoint;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="other"></param>
      /// <returns></returns>
      public bool Equals( Point other )
      {
         return ( ( X == other.X ) && ( Y == other.Y ) );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static bool operator ==( Point a, Point b )
      {
         return a.Equals( b );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static bool operator !=( Point a, Point b )
      {
         return !a.Equals( b );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals( object obj )
      {
         return ( obj is Point ) ? Equals( (Point) obj ) : false;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return X ^ Y;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return string.Format( "{{X:{0} Y:{1}}}", X, Y );
      }
   }
}