namespace RogueSharp
{
   /// <summary>
   /// A class that defines a square on a Map with all of its associated properties
   /// </summary>
   public class Cell : ICell
   {
      /// <summary>
      /// Construct a new uninitialized Cell
      /// </summary>
      public Cell()
      {
      }
      
      /// <summary>
      /// Construct a new unexplored Cell located at the specified x and y location with the specified properties
      /// </summary>
      /// <param name="x">X location of the Cell starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell starting with 0 as the top</param>
      /// <param name="isTransparent">Is there a clear line-of-sight through this Cell</param>
      /// <param name="isWalkable">Could a character could normally walk across the Cell without difficulty</param>
      public Cell( int x, int y, bool isTransparent, bool isWalkable )
      {
         X = x;
         Y = y;
         IsTransparent = isTransparent;
         IsWalkable = isWalkable;
      }

      /// <summary>
      /// Gets the X location of the Cell starting with 0 as the farthest left
      /// </summary>
      public int X { get; set; }

      /// <summary>
      /// Y location of the Cell starting with 0 as the top
      /// </summary>
      public int Y { get; set; }

      /// <summary>
      /// Get the transparency of the Cell i.e. if line of sight would be blocked by this Cell
      /// </summary>
      /// <example>
      /// A Cell representing an empty stone floor would be transparent
      /// A Cell representing a glass wall could be transparent (even though it may not be walkable)
      /// A Cell representing a solid stone wall would not be transparent
      /// </example>
      public bool IsTransparent { get; set; }

      /// <summary>
      /// Get the walkability of the Cell i.e. if a character could normally move across the Cell without difficulty
      /// </summary>
      /// <example>
      /// A Cell representing an empty stone floor would be walkable
      /// A Cell representing a glass wall may not be walkable (even though it could be transparent)
      /// A Cell representing a solid stone wall would not be walkable
      /// </example>
      public bool IsWalkable { get; set; }
      
      /// <summary>
      /// Provides a simple visual representation of the Cell using the following symbols:
      /// - `.`: `Cell` is transparent and walkable
      /// - `s`: `Cell` is walkable (but not transparent)
      /// - `o`: `Cell` is transparent (but not walkable)
      /// - `#`: `Cell` is not transparent or walkable
      /// </summary>
      /// <returns>A string representation of the Cell using special symbols to denote Cell properties</returns>
      public override string ToString()
      {
         if ( IsWalkable )
         {
            if ( IsTransparent )
            {
               return ".";
            }

            return "s";
         }

         if ( IsTransparent )
         {
            return "o";
         }

         return "#";
      }

      /// <summary>
      /// Determines whether two Cell instances are equal
      /// </summary>
      /// <param name="other">The Cell to compare this instance to</param>
      /// <returns>True if the instances are equal; False otherwise</returns>
      public virtual bool Equals( ICell other )
      {
         if ( ReferenceEquals( null, other ) )
         {
            return false;
         }
         if ( ReferenceEquals( this, other ) )
         {
            return true;
         }
         return X == other.X && Y == other.Y && IsTransparent == other.IsTransparent && IsWalkable == other.IsWalkable;
      }

      /// <summary>
      /// Determines whether two Cell instances are equal
      /// </summary>
      /// <param name="obj">The Object to compare this instance to</param>
      /// <returns>True if the instances are equal; False otherwise</returns>
      public override bool Equals( object obj )
      {
         if ( ReferenceEquals( null, obj ) )
         {
            return false;
         }
         if ( ReferenceEquals( this, obj ) )
         {
            return true;
         }
         if ( obj.GetType() != GetType() )
         {
            return false;
         }
         return Equals( (Cell) obj );
      }

      /// <summary>
      /// Determines whether two Cell instances are equal
      /// </summary>
      /// <param name="left">Cell on the left side of the equal sign</param>
      /// <param name="right">Cell on the right side of the equal sign</param>
      /// <returns>True if a and b are equal; False otherwise</returns>
      public static bool operator ==( Cell left, Cell right )
      {
         return Equals( left, right );
      }

      /// <summary>
      /// Determines whether two Cell instances are not equal
      /// </summary>
      /// <param name="left">Cell on the left side of the equal sign</param>
      /// <param name="right">Cell on the right side of the equal sign</param>
      /// <returns>True if a and b are not equal; False otherwise</returns>
      public static bool operator !=( Cell left, Cell right )
      {
         return !Equals( left, right );
      }

      /// <summary>
      /// Gets the hash code for this object which can help for quick checks of equality
      /// or when inserting this Cell into a hash-based collection such as a Dictionary or Hashtable 
      /// </summary>
      /// <returns>An integer hash used to identify this Cell</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = X;
            hashCode = ( hashCode * 397 ) ^ Y;
            hashCode = ( hashCode * 397 ) ^ IsTransparent.GetHashCode();
            hashCode = ( hashCode * 397 ) ^ IsWalkable.GetHashCode();
            return hashCode;
         }
      }
   }
}