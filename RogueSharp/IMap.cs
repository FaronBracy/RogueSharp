using System;
using System.Collections.Generic;

namespace RogueSharp
{
   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability, field-of-view and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public interface IMap
   {
      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Width of 10 will have Cells with X locations of 0 through 9
      /// Cells with an X value of 0 will be the leftmost Cells
      /// </remarks>
      int Width { get; }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Height of 20 will have Cells with Y locations of 0 through 19
      /// Cells with an Y value of 0 will be the topmost Cells
      /// </remarks>
      int Height { get; }

      /// <summary>
      /// Create a new map with the properties of all Cells set to false
      /// </summary>
      /// <remarks>
      /// This is basically a solid stone map that would then need to be modified to have interesting features
      /// </remarks>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      void Initialize( int width, int height );

      /// <summary>
      /// Get the transparency of the Cell i.e. if line of sight would be blocked by this Cell
      /// </summary>
      /// <example>      
      /// A Cell representing an empty stone floor would be transparent 
      /// A Cell representing a glass wall could be transparent (even though it may not be walkable)
      /// A Cell representing a solid stone wall would not be transparent
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if line-of-sight is not blocked by this Cell, false otherwise</returns>
      bool IsTransparent( int x, int y );

      /// <summary>
      /// Get the walkability of the Cell i.e. if a character could normally move across the Cell without difficulty
      /// </summary>
      /// <example>      
      /// A Cell representing an empty stone floor would be walkable
      /// A Cell representing a glass wall may not be walkable (even though it could be transparent)
      /// A Cell representing a solid stone wall would not be walkable
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if a character could move across this Cell, false otherwise</returns>
      bool IsWalkable( int x, int y );

      /// <summary>
      /// Check if the Cell is in the currently computed field-of-view
      /// For newly initialized maps a field-of-view will not exist so all Cells will return false
      /// Field-of-view must first be calculated by calling ComputeFov and/or AppendFov
      /// </summary>
      /// <remarks>
      /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius
      /// </remarks>
      /// <example>
      /// Field-of-view c be used to simulate a character holding a light source and exploring a Map representing a dark cavern
      /// Any Cells within the FOV would be what the character could see from their current location and lighting conditions
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if the Cell is in the currently computed field-of-view, false otherwise</returns>
      bool IsInFov( int x, int y );

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
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if the Cell has been flagged as being explored by the player, false otherwise</returns>
      bool IsExplored( int x, int y );

      /// <summary>
      /// Set the properties of a Cell to the specified values
      /// </summary>
      /// <remarks>
      /// IsInFov cannot be set through this method as it is always calculated by calling ComputeFov and/or AppendFov
      /// </remarks>
      /// <param name="x">X location of the Cell to set properties on, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to set properties on, starting with 0 as the top</param>
      /// <param name="isTransparent">True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">True if a character could walk across the Cell normally. False otherwise</param>
      /// <param name="isExplored">Optional parameter defaults to false if not provided. True if the Cell has ever been in the field-of-view of the player. False otherwise</param>
      void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored );

      /// <summary>
      /// Set the properties of an unexplored Cell to the specified values
      /// </summary>
      /// <remarks>
      /// IsInFov cannot be set through this method as it is always calculated by calling ComputeFov and/or AppendFov
      /// </remarks>
      /// <param name="x">X location of the Cell to set properties on, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to set properties on, starting with 0 as the top</param>
      /// <param name="isTransparent">True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">True if a character could walk across the Cell normally. False otherwise</param>
      void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable );

      /// <summary>
      /// Sets the properties of all Cells in the Map to be transparent and walkable
      /// </summary>
      void Clear();

      /// <summary>
      /// Sets the properties of all Cells in the Map to the specified values
      /// </summary>
      /// <param name="isTransparent">Optional parameter defaults to false if not provided. True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">Optional parameter defaults to false if not provided. True if a character could walk across the Cell normally. False otherwise</param>
      void Clear( bool isTransparent, bool isWalkable );

      /// <summary>
      /// Create and return a deep copy of an existing Map
      /// </summary>
      /// <returns>IMap deep copy of the original Map</returns>
      IMap Clone();

      /// <summary>
      /// Copies the Cell properties of a smaller source Map into this destination Map at location (0,0)
      /// </summary>
      /// <param name="sourceMap">An IMap which must be of smaller size and able to fit in this destination Map at the specified location</param>
      void Copy( IMap sourceMap );

      /// <summary>
      /// Copies the Cell properties of a smaller source Map into this destination Map at the specified location
      /// </summary>
      /// <param name="sourceMap">An IMap which must be of smaller size and able to fit in this destination Map at the specified location</param>
      /// <param name="left">Optional parameter defaults to 0 if not provided. X location of the Cell to start copying parameters to, starting with 0 as the farthest left</param>
      /// <param name="top">Optional parameter defaults to 0 if not provided. Y location of the Cell to start copying parameters to, starting with 0 as the top</param>
      void Copy( IMap sourceMap, int left, int top );

      /// <summary>
      /// Performs a field-of-view calculation with the specified parameters.
      /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius.
      /// Any existing field-of-view calculations will be overwritten when calling this method.
      /// </summary>
      /// <param name="xOrigin">X location of the Cell to perform the field-of-view calculation with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Cell to perform the field-of-view calculation with 0 as the top</param>
      /// <param name="radius">The number of Cells in which the field-of-view extends from the origin Cell. Think of this as the intensity of the light source.</param>
      /// <param name="lightWalls">True if walls should be included in the field-of-view when they are within the radius of the light source. False excludes walls even when they are within range.</param>
      void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls );

      /// <summary>
      /// Performs a field-of-view calculation with the specified parameters and appends it any existing field-of-view calculations.
      /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius.
      /// </summary>
      /// <example>
      /// When a character is holding a light source in a large area that also has several other sources of light such as torches along the walls
      /// ComputeFov could first be called for the character and then AppendFov could be called for each torch to give us the final combined FOV given all the light sources
      /// </example>
      /// <param name="xOrigin">X location of the Cell to perform the field-of-view calculation with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Cell to perform the field-of-view calculation with 0 as the top</param>
      /// <param name="radius">The number of Cells in which the field-of-view extends from the origin Cell. Think of this as the intensity of the light source.</param>
      /// <param name="lightWalls">True if walls should be included in the field-of-view when they are within the radius of the light source. False excludes walls even when they are within range.</param>
      void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls );

      /// <summary>
      /// Get an IEnumerable of all Cells in the Map
      /// </summary>
      /// <returns>IEnumerable of all Cells in the Map</returns>
      IEnumerable<ICell> GetAllCells();

      /// <summary>
      /// Get an IEnumerable of Cells in a line from the Origin Cell to the Destination Cell
      /// The resulting IEnumerable includes the Origin and Destination Cells
      /// Uses Bresenham's line algorithm to determine which Cells are in the closest approximation to a straight line between the two Cells
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell at the start of the line with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell at the start of the line with 0 as the top</param>
      /// <param name="xDestination">X location of the Destination Cell at the end of the line with 0 as the farthest left</param>
      /// <param name="yDestination">Y location of the Destination Cell at the end of the line with 0 as the top</param>
      /// <returns>IEnumerable of Cells in a line from the Origin Cell to the Destination Cell which includes the Origin and Destination Cells</returns>
      IEnumerable<ICell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination );

      /// <summary>
      /// Get an IEnumerable of Cells in a circular Radius around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the Origin Cell</param>
      /// <returns>IEnumerable of Cells in a circular Radius around the Origin Cell</returns>
      IEnumerable<ICell> GetCellsInRadius( int xOrigin, int yOrigin, int radius );

      /// <summary>
      /// Get an IEnumerable of Cells in a square area around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the Origin Cell</param>
      /// <returns>IEnumerable of Cells in a square area around the Origin Cell</returns>
      IEnumerable<ICell> GetCellsInArea( int xOrigin, int yOrigin, int distance );

      /// <summary>
      /// Get an IEnumerable of the outermost border Cells in a circular Radius around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="radius">The radius from the Origin Cell in which the border Cells lie</param>
      /// <returns>IEnumerable of the outermost border Cells in a circular Radius around the Origin Cell</returns>
      IEnumerable<ICell> GetBorderCellsInRadius( int xOrigin, int yOrigin, int radius );

      /// <summary>
      /// Get an IEnumerable of the outermost border Cells in a square around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="distance">The distance from the Origin Cell in which the border Cells lie</param>
      /// <returns> IEnumerable of the outermost border Cells in a square around the Origin Cell</returns>
      IEnumerable<ICell> GetBorderCellsInArea( int xOrigin, int yOrigin, int distance );

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified row numbers
      /// </summary>
      /// <param name="rowNumbers">Integer array of row numbers with 0 as the top</param>
      /// <returns>IEnumerable of all the Cells in the specified row numbers</returns>
      IEnumerable<ICell> GetCellsInRows( params int[] rowNumbers );

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified column numbers
      /// </summary>
      /// <param name="columnNumbers">Integer array of column numbers with 0 as the farthest left</param>
      /// <returns>IEnumerable of all the Cells in the specified column numbers</returns>
      IEnumerable<ICell> GetCellsInColumns( params int[] columnNumbers );

      /// <summary>
      /// Get a Cell at the specified location
      /// </summary>
      /// <param name="x">X location of the Cell to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to get, starting with 0 as the top</param>
      /// <returns>Cell at the specified location</returns>
      ICell GetCell( int x, int y );

      /// <summary>
      /// Provides a simple visual representation of the map using the following symbols:
      /// - `%`: `Cell` is not in field-of-view
      /// - `.`: `Cell` is transparent, walkable, and in field-of-view
      /// - `s`: `Cell` is walkable and in field-of-view (but not transparent)
      /// - `o`: `Cell` is transparent and in field-of-view (but not walkable)
      /// - `#`: `Cell` is in field-of-view (but not transparent or walkable)
      /// </summary>
      /// <param name="useFov">True if field-of-view calculations will be used when creating the string represenation of the Map. False otherwise</param>
      /// <returns>A string representation of the map using special symbols to denote Cell properties</returns>
      string ToString( bool useFov );

      /// <summary>
      /// Get a MapState POCO which represents this Map and can be easily serialized
      /// Use Restore with the MapState to get back a full Map
      /// </summary>
      /// <returns>Mapstate POCO (Plain Old C# Object) which represents this Map and can be easily serialized</returns>
      MapState Save();

      /// <summary>
      /// Restore the state of this Map from the specified MapState
      /// </summary>
      /// <param name="state">Mapstate POCO (Plain Old C# Object) which represents this Map and can be easily serialized and deserialized</param>
      void Restore( MapState state );

      /// <summary>
      /// Get the Cell at the specified single dimensional array index using the formulas: x = index % Width; y = index / Width;
      /// </summary>
      /// <param name="index">The single dimensional array index for the Cell that we want to get</param>
      /// <returns>Cell at the specified single dimensional array index</returns>
      ICell CellFor( int index );

      /// <summary>
      /// Get the single dimensional array index for a Cell at the specified location using the formula: index = ( y * Width ) + x
      /// </summary>
      /// <param name="x">X location of the Cell index to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell index to get, starting with 0 as the top</param>
      /// <returns>An index for the Cell at the specified location useful if storing Cells in a single dimensional array</returns>
      int IndexFor( int x, int y );

      /// <summary>
      /// Get the single dimensional array index for the specified Cell
      /// </summary>
      /// <param name="cell">The Cell to get the index for</param>
      /// <returns>An index for the Cell which is useful if storing Cells in a single dimensional array</returns>
      int IndexFor( ICell cell );
   }

   /// <summary>
   /// A class representing the state of a Map which can be used to Restore a Map to a previously Saved state
   /// This POCO (Plain Old C# Object) can be easily serialized and deserialized
   /// </summary>
   public class MapState
   {
      /// <summary>
      /// Flags Enumeration of the possible properties for any Cell in the Map
      /// </summary>
      [Flags]
      public enum CellProperties
      {
         /// <summary>
         /// Not set
         /// </summary>
         None = 0,
         /// <summary>
         /// A character could normally walk across the Cell without difficulty
         /// </summary>
         Walkable = 1,
         /// <summary>
         /// There is a clear line-of-sight through this Cell
         /// </summary>
         Transparent = 2,
         /// <summary>
         /// The Cell is in the currently observable field-of-view
         /// </summary>
         Visible = 4,
         /// <summary>
         /// The Cell has been in the field-of-view in the player at some point during the game
         /// </summary>
         Explored = 8
      }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      public int Width { get; set; }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      public int Height { get; set; }

      /// <summary>
      /// An array of the Flags Enumeration of CellProperties for each Cell in the Map.
      /// The index of the array corresponds to the location of the Cell within the Map using the forumla: index = ( y * Width ) + x
      /// </summary>
      public CellProperties[] Cells { get; set; }
   }
}