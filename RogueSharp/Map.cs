using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public class Map : IMap
   {
      private FieldOfView _fieldOfView;
      private bool[,] _isTransparent;
      private bool[,] _isWalkable;
      private bool[,] _isExplored;
      /// <summary>
      /// 
      /// </summary>
      public Map()
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="width"></param>
      /// <param name="height"></param>
      public Map( int width, int height )
      {
         Initialize( width, height );
      }
      /// <summary>
      /// 
      /// </summary>
      public int Width { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public int Height { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="width"></param>
      /// <param name="height"></param>
      public void Initialize( int width, int height )
      {
         Width = width;
         Height = height;
         _isTransparent = new bool[width, height];
         _isWalkable = new bool[width, height];
         _isExplored = new bool[width, height];
         _fieldOfView = new FieldOfView( this );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public bool IsTransparent( int x, int y )
      {
         return _isTransparent[x, y];
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public bool IsWalkable( int x, int y )
      {
         return _isWalkable[x, y];
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public bool IsInFov( int x, int y )
      {
         return _fieldOfView.IsInFov( x, y );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public bool IsExplored( int x, int y )
      {
         return _isExplored[x, y];
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="isTransparent"></param>
      /// <param name="isWalkable"></param>
      /// <param name="isExplored"></param>
      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored = false )
      {
         _isTransparent[x, y] = isTransparent;
         _isWalkable[x, y] = isWalkable;
         _isExplored[x, y] = isExplored;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="isTransparent"></param>
      /// <param name="isWalkable"></param>
      public void Clear( bool isTransparent = false, bool isWalkable = false )
      {
         foreach ( Cell cell in GetAllCells() )
         {
            SetCellProperties( cell.X, cell.Y, isTransparent, isWalkable );
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public IMap Clone()
      {
         var map = new Map( Width, Height );
         foreach ( Cell cell in GetAllCells() )
         {
            map.SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, cell.IsExplored );
         }
         return map;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceMap"></param>
      /// <param name="left"></param>
      /// <param name="top"></param>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="radius"></param>
      /// <param name="lightWalls"></param>
      public void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         _fieldOfView.ComputeFov( xOrigin, yOrigin, radius, lightWalls );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="radius"></param>
      /// <param name="lightWalls"></param>
      public void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         _fieldOfView.AppendFov( xOrigin, yOrigin, radius, lightWalls );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="xDestination"></param>
      /// <param name="yDestination"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="radius"></param>
      /// <returns></returns>
      public IEnumerable<Cell> GetCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         var discovered = new HashSet<int>();

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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="distance"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="radius"></param>
      /// <returns></returns>
      public IEnumerable<Cell> GetBorderCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         var discovered = new HashSet<int>();

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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="xOrigin"></param>
      /// <param name="yOrigin"></param>
      /// <param name="distance"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="rowNumbers"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="columnNumbers"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public Cell GetCell( int x, int y )
      {
         return new Cell( x, y, _isTransparent[x, y], _isWalkable[x, y], _fieldOfView.IsInFov( x, y ), _isExplored[x, y] );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="useFov"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="state"></param>
      public void Restore( MapState state )
      {
         var inFov = new HashSet<int>();

         Initialize( state.Width, state.Height );
         foreach ( Cell cell in GetAllCells() )
         {
            MapState.CellProperties cellProperties = state.Cells[cell.Y * Width + cell.X];
            if ( cellProperties.HasFlag( MapState.CellProperties.Visible ) )
            {
               inFov.Add( IndexFor( cell.X, cell.Y ) );
            }
            _isTransparent[cell.X, cell.Y] = cellProperties.HasFlag( MapState.CellProperties.Transparent );
            _isWalkable[cell.X, cell.Y] = cellProperties.HasFlag( MapState.CellProperties.Walkable );
         }

         _fieldOfView = new FieldOfView( this, inFov );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="mapCreationStrategy"></param>
      /// <returns></returns>
      public static Map Create( IMapCreationStrategy<Map> mapCreationStrategy )
      {
         return mapCreationStrategy.CreateMap();
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public Cell CellFor( int index )
      {
         int x = index % Width;
         int y = index / Width;

         return GetCell( x, y );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      public int IndexFor( int x, int y )
      {
         return ( y * Width ) + x;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="cell"></param>
      /// <returns></returns>
      public int IndexFor( Cell cell )
      {
         return ( cell.Y * Width ) + cell.X;
      }
      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, out Cell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( IndexFor( cell ) );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return ToString( false );
      }
   }
}