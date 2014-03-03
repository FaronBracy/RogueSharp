using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSharp
{
   public class Map : IMap
   {
      private bool[,] _isInFov;
      private bool[,] _isTransparent;
      private bool[,] _isWalkable;
      private bool[,] _isExplored;

      public Map()
      {
      }

      public Map( int width, int height )
      {
         Initialize( width, height );
      }

      public int Width { get; private set; }
      public int Height { get; private set; }

      public void Initialize( int width, int height )
      {
         Width = width;
         Height = height;
         _isTransparent = new bool[width, height];
         _isWalkable = new bool[width, height];
         _isInFov = new bool[width, height];
         _isExplored = new bool[width, height];
      }

      public bool IsTransparent( int x, int y )
      {
         return _isTransparent[x, y];
      }

      public bool IsWalkable( int x, int y )
      {
         return _isWalkable[x, y];
      }

      public bool IsInFov( int x, int y )
      {
         return _isInFov[x, y];
      }

      public bool IsExplored( int x, int y )
      {
         return _isExplored[x, y];
      }

      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored = false )
      {
         _isTransparent[x, y] = isTransparent;
         _isWalkable[x, y] = isWalkable;
         _isExplored[x, y] = isExplored;
      }

      public void Clear( bool isTransparent = false, bool isWalkable = false )
      {
         foreach ( Cell cell in GetAllCells() )
         {
            SetCellProperties( cell.X, cell.Y, isTransparent, isWalkable );
         }
      }

      public IMap Clone()
      {
         var map = new Map( Width, Height );
         foreach ( Cell cell in GetAllCells() )
         {
            map.SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, cell.IsInFov, cell.IsExplored );
         }
         return map;
      }

      public void Copy( IMap sourceMap, int left, int top )
      {
         if ( sourceMap.Width + left > Width )
         {
            throw new ArgumentException( "Source map 'width' + 'left' cannot be larger than the destination map width", "destinationMap" );
         }
         if ( sourceMap.Height + top > Height )
         {
            throw new ArgumentException( "Source map 'height' + 'top' cannot be larger than the destination map height", "destinationMap" );
         }
         foreach ( Cell cell in sourceMap.GetAllCells() )
         {
            SetCellProperties( cell.X + left, cell.Y + top, cell.IsTransparent, cell.IsWalkable, cell.IsExplored );
         }
      }

      public void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         ClearFov();
         AppendFov( xOrigin, yOrigin, radius, lightWalls );
      }

      public void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         foreach ( Cell borderCell in GetBorderCellsInArea( xOrigin, yOrigin, radius ) )
         {
            foreach ( Cell cell in GetCellsAlongLine( xOrigin, yOrigin, borderCell.X, borderCell.Y ) )
            {
               if ( ( Math.Abs( cell.X - xOrigin ) + Math.Abs( cell.Y - yOrigin ) ) > radius )
               {
                  break;
               }
               if ( cell.IsTransparent )
               {
                  _isInFov[cell.X, cell.Y] = true;
               }
               else
               {
                  if ( lightWalls )
                  {
                     _isInFov[cell.X, cell.Y] = true;
                  }
                  break;
               }
            }
         }

         if ( lightWalls )
         {
            // Post processing step taken from algorithm at this website:
            // https://sites.google.com/site/jicenospam/visibilitydetermination
            foreach ( Cell cell in GetCellsInArea( xOrigin, yOrigin, radius ) )
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
      }

      public IEnumerable<Cell> GetAllCells()
      {
         for ( int y = 0; y < Height; y++ )
         {
            for ( int x = 0; x < Width; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      public IEnumerable<Cell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination )
      {
         int dx = Math.Abs( xDestination - xOrigin );
         int dy = Math.Abs( yDestination - yOrigin );

         int sx = xOrigin < xDestination ? 1 : -1;
         int sy = yOrigin < yDestination ? 1 : -1;
         int err = dx - dy;

         while ( true )
         {
            yield return GetCell( xOrigin, yOrigin );
            if ( xOrigin == xDestination && yOrigin == yDestination )
            {
               break;
            }
            int e2 = 2 * err;
            if ( e2 > -dy )
            {
               err = err - dy;
               xOrigin = xOrigin + sx;
            }
            if ( e2 < dx )
            {
               err = err + dx;
               yOrigin = yOrigin + sy;
            }
         }
      }

      public IEnumerable<Cell> GetCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         var discovered = new HashSet<Cell>();

         int xMin = Math.Max( 0, xOrigin - radius );
         int xMax = Math.Min( Width - 1, xOrigin + radius );
         int yMin = Math.Max( 0, yOrigin - radius );
         int yMax = Math.Min( Height - 1, yOrigin + radius );

         for ( int i = 0; i <= radius; i++ )
         {
            for ( int j = radius; j >= 0 + i; j-- )
            {
               Cell cell;
               if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Min( yMax, yOrigin + radius - j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Max( yMin, yOrigin - radius + j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Min( yMax, yOrigin + radius - j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Max( yMin, yOrigin - radius + j ), out cell ) )
               {
                  yield return cell;
               }
            }
         }
      }

      public IEnumerable<Cell> GetCellsInArea( int xOrigin, int yOrigin, int distance )
      {
         int xMin = Math.Max( 0, xOrigin - distance );
         int xMax = Math.Min( Width - 1, xOrigin + distance );
         int yMin = Math.Max( 0, yOrigin - distance );
         int yMax = Math.Min( Height - 1, yOrigin + distance );

         for ( int y = yMin; y <= yMax; y++ )
         {
            for ( int x = xMin; x <= xMax; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      public IEnumerable<Cell> GetBorderCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         var discovered = new HashSet<Cell>();

         int xMin = Math.Max( 0, xOrigin - radius );
         int xMax = Math.Min( Width - 1, xOrigin + radius );
         int yMin = Math.Max( 0, yOrigin - radius );
         int yMax = Math.Min( Height - 1, yOrigin + radius );

         Cell cell;
         if ( AddToHashSet( discovered, xOrigin, yMin, out cell ) )
         {
            yield return cell;
         }
         if ( AddToHashSet( discovered, xOrigin, yMax, out cell ) )
         {
            yield return cell;
         }
         for ( int i = 1; i <= radius; i++ )
         {
            if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Min( yMax, yOrigin + radius - i ), out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Max( yMin, yOrigin - radius + i ), out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Min( yMax, yOrigin + radius - i ), out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Max( yMin, yOrigin - radius + i ), out cell ) )
            {
               yield return cell;
            }
         }
      }

      public IEnumerable<Cell> GetBorderCellsInArea( int xOrigin, int yOrigin, int distance )
      {
         int xMin = Math.Max( 0, xOrigin - distance );
         int xMax = Math.Min( Width - 1, xOrigin + distance );
         int yMin = Math.Max( 0, yOrigin - distance );
         int yMax = Math.Min( Height - 1, yOrigin + distance );

         for ( int x = xMin; x <= xMax; x++ )
         {
            yield return GetCell( x, yMin );
            yield return GetCell( x, yMax );
         }
         for ( int y = yMin; y <= yMax; y++ )
         {
            yield return GetCell( xMin, y );
            yield return GetCell( xMax, y );
         }
      }

      public IEnumerable<Cell> GetCellsInRows( params int[] rowNumbers )
      {
         foreach ( int y in rowNumbers )
         {
            for ( int x = 0; x < Width; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      public IEnumerable<Cell> GetCellsInColumns( params int[] columnNumbers )
      {
         foreach ( int x in columnNumbers )
         {
            for ( int y = 0; y < Height; y++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      public Cell GetCell( int x, int y )
      {
         return new Cell( x, y, _isTransparent[x, y], _isWalkable[x, y], _isInFov[x, y], _isExplored[x, y] );
      }

      public string ToString( bool useFov )
      {
         var mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( Cell cell in GetAllCells() )
         {
            if ( cell.Y != lastY )
            {
               lastY = cell.Y;
               mapRepresentation.Append( Environment.NewLine );
            }
            mapRepresentation.Append( cell.ToString( useFov ) );
         }
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      public MapState Save()
      {
         var mapState = new MapState();
         mapState.Width = Width;
         mapState.Height = Height;
         mapState.Cells = new MapState.CellProperties[Width * Height];
         foreach ( Cell cell in GetAllCells() )
         {
            var cellProperties = MapState.CellProperties.None;
            if ( cell.IsInFov )
            {
               cellProperties |= MapState.CellProperties.Visible;
            }
            if ( cell.IsTransparent )
            {
               cellProperties |= MapState.CellProperties.Transparent;
            }
            if ( cell.IsWalkable )
            {
               cellProperties |= MapState.CellProperties.Walkable;
            }
            mapState.Cells[cell.Y * Width + cell.X] = cellProperties;
         }
         return mapState;
      }

      public void Restore( MapState state )
      {
         Initialize( state.Width, state.Height );
         foreach ( Cell cell in GetAllCells() )
         {
            MapState.CellProperties cellProperties = state.Cells[cell.Y * Width + cell.X];
            _isInFov[cell.X, cell.Y] = cellProperties.HasFlag( MapState.CellProperties.Visible );
            _isTransparent[cell.X, cell.Y] = cellProperties.HasFlag( MapState.CellProperties.Transparent );
            _isWalkable[cell.X, cell.Y] = cellProperties.HasFlag( MapState.CellProperties.Walkable );
         }
      }

      public static Map Create( IMapCreationStrategy<Map> mapCreationStrategy )
      {
         return mapCreationStrategy.CreateMap();
      }

      internal void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isInFov, bool isExplored )
      {
         _isTransparent[x, y] = isTransparent;
         _isWalkable[x, y] = isWalkable;
         _isInFov[x, y] = isInFov;
         _isExplored[x, y] = isExplored;
      }

      internal void ClearFov()
      {
         foreach ( Cell cell in GetAllCells() )
         {
            _isInFov[cell.X, cell.Y] = false;
         }
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
         if ( !_isInFov[x, y] && !_isTransparent[x, y] )
         {
            if ( ( _isTransparent[x1, y1] && _isInFov[x1, y1] ) || ( _isTransparent[x2, y2] && _isInFov[x2, y2] )
                 || ( _isTransparent[x2, y1] && _isInFov[x2, y1] ) )
            {
               _isInFov[x, y] = true;
            }
         }
      }

      private bool AddToHashSet( HashSet<Cell> hashSet, int x, int y, out Cell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( cell );
      }

      public override string ToString()
      {
         return ToString( false );
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