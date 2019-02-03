namespace RogueSharp
{
   /// <summary>
   /// A class that defines a square on a Map with all of its associated properties
   /// </summary>
   public class Cell : ICell
   {
      /// <summary>
      /// Construct a new Cell located at the specified x and y location with the specified properties
      /// </summary>
      /// <param name="x">X location of the Cell starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell starting with 0 as the top</param>
      /// <param name="isTransparent">Is there a clear line-of-sight through this Cell</param>
      /// <param name="isWalkable">Could a character could normally walk across the Cell without difficulty</param>
      /// <param name="isInFov">Is the Cell currently in the currently observable field-of-view</param>
      /// <param name="isExplored">Has this Cell ever been explored by the player</param>
      public Cell( int x, int y, bool isTransparent, bool isWalkable, bool isInFov, bool isExplored )
      {
         X = x;
         Y = y;
         IsTransparent = isTransparent;
         IsWalkable = isWalkable;
         IsInFov = isInFov;
         IsExplored = isExplored;
      }

      /// <summary>
      /// Construct a new unexplored Cell located at the specified x and y location with the specified properties
      /// </summary>
      /// <param name="x">X location of the Cell starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell starting with 0 as the top</param>
      /// <param name="isTransparent">Is there a clear line-of-sight through this Cell</param>
      /// <param name="isWalkable">Could a character could normally walk across the Cell without difficulty</param>
      /// <param name="isInFov">Is the Cell currently in the currently observable field-of-view</param>
      public Cell( int x, int y, bool isTransparent, bool isWalkable, bool isInFov )
      {
         X = x;
         Y = y;
         IsTransparent = isTransparent;
         IsWalkable = isWalkable;
         IsInFov = isInFov;
         IsExplored = false;
      }

      /// <summary>
      /// Gets the X location of the Cell starting with 0 as the farthest left
      /// </summary>
      public int X { get; private set; }

      /// <summary>
      /// Y location of the Cell starting with 0 as the top
      /// </summary>
      public int Y { get; private set; }

      /// <summary>
      /// Get the transparency of the Cell i.e. if line of sight would be blocked by this Cell
      /// </summary>
      /// <example>      
      /// A Cell representing an empty stone floor would be transparent 
      /// A Cell representing a glass wall could be transparent (even though it may not be walkable)
      /// A Cell representing a solid stone wall would not be transparent
      /// </example>
      public bool IsTransparent { get; private set; }

      /// <summary>
      /// Get the walkability of the Cell i.e. if a character could normally move across the Cell without difficulty
      /// </summary>
      /// <example>      
      /// A Cell representing an empty stone floor would be walkable
      /// A Cell representing a glass wall may not be walkable (even though it could be transparent)
      /// A Cell representing a solid stone wall would not be walkable
      /// </example>
      public bool IsWalkable { get; private set; }

      /// <summary>
      /// Check if the Cell is in the currently computed field-of-view
      /// For newly initialized maps a field-of-view will not exist so all Cells will return false
      /// Field-of-view must first be calculated by calling ComputeFov and/or AppendFov
      /// </summary>
      /// <remarks>
      /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius
      /// </remarks>
      /// <example>
      /// Field-of-view can be used to simulate a character holding a light source and exploring a Map representing a dark cavern
      /// Any Cells within the FOV would be what the character could see from their current location and lighting conditions
      /// </example>
      public bool IsInFov { get; private set; }

      /// <summary>
      /// Check if the Cell is flagged as ever having been explored by the player
      /// </summary>
      /// <remarks>
      /// The explored property of a Cell can be used to track if the Cell has ever been in the field-of-view of a character controlled by the player
      /// This property will not automatically be updated based on FOV calculations or any other built-in functions of the RogueSharp library.
      /// </remarks>
      /// <example>
      /// As the player moves characters around a Map, Cells will enter and exit the currently computed field-of-view
      /// This property can be used to keep track of those Cells that have been "seen" and could be used to show fog-of-war type effects when rendering the map
      /// </example>
      public bool IsExplored { get; private set; }

      /// <summary>
      /// Provides a simple visual representation of the Cell using the following symbols:
      /// - `.`: `Cell` is transparent and walkable
      /// - `s`: `Cell` is walkable (but not transparent)
      /// - `o`: `Cell` is transparent (but not walkable)
      /// - `#`: `Cell` is not transparent or walkable
      /// </summary>
      /// <remarks>
      /// This call ignores field-of-view. If field-of-view is important use the ToString overload with a "true" parameter
      /// </remarks>
      /// <returns>A string representation of the Cell using special symbols to denote Cell properties</returns>
      public override string ToString()
      {
         return ToString( false );
      }

      /// <summary>
      /// Provides a simple visual representation of the Cell using the following symbols:
      /// - `%`: `Cell` is not in field-of-view
      /// - `.`: `Cell` is transparent, walkable, and in field-of-view
      /// - `s`: `Cell` is walkable and in field-of-view (but not transparent)
      /// - `o`: `Cell` is transparent and in field-of-view (but not walkable)
      /// - `#`: `Cell` is in field-of-view (but not transparent or walkable)
      /// </summary>
      /// <param name="useFov">True if field-of-view calculations will be used when creating the string representation of the Cell. False otherwise</param>
      /// <returns>A string representation of the Cell using special symbols to denote Cell properties</returns>
      public string ToString( bool useFov )
      {
         if ( useFov && !IsInFov )
         {
            return "%";
         }
         if ( IsWalkable )
         {
            if ( IsTransparent )
            {
               return ".";
            }
            else
            {
               return "s";
            }
         }
         else
         {
            if ( IsTransparent )
            {
               return "o";
            }
            else
            {
               return "#";
            }
         }
      }

      /// <summary>
      /// Determines whether two Cell instances are equal
      /// </summary>
      /// <param name="other">The Cell to compare this instance to</param>
      /// <returns>True if the instances are equal; False otherwise</returns>
      public bool Equals( ICell other )
      {
         if ( ReferenceEquals( null, other ) )
         {
            return false;
         }
         if ( ReferenceEquals( this, other ) )
         {
            return true;
         }
         return X == other.X && Y == other.Y && IsTransparent == other.IsTransparent && IsWalkable == other.IsWalkable && IsInFov == other.IsInFov && IsExplored == other.IsExplored;
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
            hashCode = ( hashCode * 397 ) ^ IsInFov.GetHashCode();
            hashCode = ( hashCode * 397 ) ^ IsExplored.GetHashCode();
            return hashCode;
         }
      }
   }
}