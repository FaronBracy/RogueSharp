using System;
using System.Collections.Generic;

namespace RogueSharp
{
   public class FieldOfView
   {
      private readonly IMap _map;
      private readonly HashSet<int> _inFov;

      public FieldOfView( IMap map )
      {
         _map = map;
         _inFov = new HashSet<int>();
      }

      internal FieldOfView( IMap map, HashSet<int> inFov )
      {
         _map = map;
         _inFov = inFov;
      }

      public FieldOfView Clone()
      {
         var inFovCopy = new HashSet<int>();
         foreach( int i in _inFov )
         {
            inFovCopy.Add( i );
         }
         return new FieldOfView( _map, inFovCopy );
      }

      public bool IsInFov( int x, int y )
      {
         return _inFov.Contains( _map.IndexFor( x, y ) );
      }

      public List<Cell> ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         ClearFov();
         return AppendFov( xOrigin, yOrigin, radius, lightWalls );
      }

      public List<Cell> AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         foreach ( Cell borderCell in _map.GetBorderCellsInArea( xOrigin, yOrigin, radius ) )
         {
            foreach ( Cell cell in _map.GetCellsAlongLine( xOrigin, yOrigin, borderCell.X, borderCell.Y ) )
            {
               if ( ( Math.Abs( cell.X - xOrigin ) + Math.Abs( cell.Y - yOrigin ) ) > radius )
               {
                  break;
               }
               if ( cell.IsTransparent )
               {
                  _inFov.Add( _map.IndexFor( cell ) );
               }
               else
               {
                  if ( lightWalls )
                  {
                     _inFov.Add( _map.IndexFor( cell ) );
                  }
                  break;
               }
            }
         }

         if ( lightWalls )
         {
            // Post processing step taken from algorithm at this website:
            // https://sites.google.com/site/jicenospam/visibilitydetermination
            foreach ( Cell cell in _map.GetCellsInArea( xOrigin, yOrigin, radius ) )
            {
               if ( cell.X > xOrigin )
               {
                  if ( cell.Y > yOrigin )
                  {
                     PostProcessFovQuadrant( cell.X, cell.Y, Quadrant.SE );
                  }
                  else if ( cell.Y < yOrigin )
                  {
                     PostProcessFovQuadrant( cell.X, cell.Y, Quadrant.NE );
                  }
               }
               else if ( cell.X < xOrigin )
               {
                  if ( cell.Y > yOrigin )
                  {
                     PostProcessFovQuadrant( cell.X, cell.Y, Quadrant.SW );
                  }
                  else if ( cell.Y < yOrigin )
                  {
                     PostProcessFovQuadrant( cell.X, cell.Y, Quadrant.NW );
                  }
               }
            }
         }

         return CellsInFov();
      }

      private List<Cell> CellsInFov()
      {
         var cells = new List<Cell>();
         foreach ( int index in _inFov )
         {
            cells.Add( _map.CellFor( index ) );
         }
         return cells;
      }



      private void ClearFov()
      {
         _inFov.Clear();
      }

      private void PostProcessFovQuadrant( int x, int y, Quadrant quadrant )
      {
         int x1 = x;
         int y1 = y;
         int x2 = x;
         int y2 = y;
         switch ( quadrant )
         {
            case Quadrant.NE:
            {
               y1 = y + 1;
               x2 = x - 1;
               break;
            }
            case Quadrant.SE:
            {
               y1 = y - 1;
               x2 = x - 1;
               break;
            }
            case Quadrant.SW:
            {
               y1 = y - 1;
               x2 = x + 1;
               break;
            }
            case Quadrant.NW:
            {
               y1 = y + 1;
               x2 = x + 1;
               break;
            }
         }
         if ( !IsInFov( x, y ) && !_map.IsTransparent( x, y ) )
         {
            if ( ( _map.IsTransparent( x1, y1 ) && IsInFov( x1, y1 ) ) || ( _map.IsTransparent( x2, y2 ) && IsInFov( x2, y2 ) )
                 || ( _map.IsTransparent( x2, y1 ) && IsInFov( x2, y1 ) ) )
            {
               _inFov.Add( _map.IndexFor( x, y ) );
            }
         }
      }

      private enum Quadrant
      {
         NE = 1,
         SE = 2,
         SW = 3,
         NW = 4
      }
   }
}
