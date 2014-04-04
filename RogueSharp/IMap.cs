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
      /// <param name="width">How many cells wide the Map will be</param>
      /// <param name="height">How many cells tall the Map will be</param>
      void Initialize( int width, int height );
      /// <summary>
      /// Get the transparency of the Cell i.e. if line of sight would be blocked by this Cell
      /// </summary>
      /// <example>      
      /// A cell representing an empty stone floor would be transparent 
      /// A cell representing a glass wall could be transparent (even though it may not be walkable)
      /// A cell representing a solid stone wall would not be transparent
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if line-of-sight is not blocked by this cell, false otherwise</returns>
      bool IsTransparent( int x, int y );
      /// <summary>
      /// Get the walkability of the Cell i.e. if a character could normally move across the cell without difficulty
      /// </summary>
      /// <example>      
      /// A cell representing an empty stone floor would be walkable
      /// A cell representing a glass wall may not be walkable (even though it could be transparent)
      /// A cell representing a solid stone wall would not be walkable
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if a character could move across this cell, false otherwise</returns>
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
      /// Any cells within the FOV would be what the character could see from their current location and lighting conditions
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if the Cell is in the currently computed field-of-view, false otherwise</returns>
      bool IsInFov( int x, int y );
      /// <summary>
      /// Check if the Cell is flagged as ever having been explored by the player
      /// </summary>
      /// <remarks>
      /// The explored property of a cell can be used to track if the cell has ever been in the field-of-view of a character controlled by the player
      /// This property will not automatically be updated based on FOV calcuations or any other built-in functions of the RogueSharp library.
      /// </remarks>
      /// <example>
      /// As the player moves characters around a Map, Cells will enter and exit the currently computed field-of-view
      /// This property can be used to keep track of those Cells that have been "seen" and could be used to show fog-of-war type effects when rendering the map
      /// </example>
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if the Cell has been flagged as being explored by the player, false otherwise</returns>
      bool IsExplored( int x, int y );
      void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored = false );
      void Clear( bool isTransparent = false, bool isWalkable = false );
      IMap Clone();
      void Copy( IMap sourceMap, int left = 0, int top = 0 );
      void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls );
      void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls );
      IEnumerable<Cell> GetAllCells();
      IEnumerable<Cell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination );
      IEnumerable<Cell> GetCellsInRadius( int xOrigin, int yOrigin, int radius );
      IEnumerable<Cell> GetCellsInArea( int xOrigin, int yOrigin, int distance );
      IEnumerable<Cell> GetBorderCellsInRadius( int xOrigin, int yOrigin, int radius );
      IEnumerable<Cell> GetBorderCellsInArea( int xOrigin, int yOrigin, int distance );
      IEnumerable<Cell> GetCellsInRows( params int[] rowNumbers );
      IEnumerable<Cell> GetCellsInColumns( params int[] columnNumbers );
      Cell GetCell( int x, int y );
      string ToString( bool useFov );
      MapState Save();
      void Restore( MapState state );
      Cell CellFor( int index );
      int IndexFor( int x, int y );
      int IndexFor( Cell cell );
   }

   public class MapState
   {
      [Flags]
      public enum CellProperties
      {
         None = 0,
         Walkable = 1,
         Transparent = 2,
         Visible = 4,
         Explored = 8
      }

      public int Width { get; set; }
      public int Height { get; set; }
      public CellProperties[] Cells { get; set; }
   }
}