using System;
using System.Collections.Generic;

namespace RogueSharp
{
   public interface IMap
   {
      int Width { get; }
      int Height { get; }

      void Initialize( int width, int height );
      bool IsTransparent( int x, int y );
      bool IsWalkable( int x, int y );
      bool IsInFov( int x, int y );
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