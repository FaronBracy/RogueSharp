using System;
using System.Collections.Generic;
using System.Text;
using libtcod;
using RogueSharp.MapCreation;

namespace RogueSharp.PerformanceTest
{
   public class LibtcodMap : IMap
   {
      public TCODMap TCODMap { get; private set; }

      public int Height
      {
         get { return TCODMap.getHeight(); }
      }

      public int Width
      {
         get { return TCODMap.getWidth(); }
      }

      public LibtcodMap() { }

      public LibtcodMap( int width, int height )
      {
         Initialize( width, height );
      }

      public LibtcodMap( TCODMap map )
      {
         TCODMap = map;
      }

      public static LibtcodMap Create( IMapCreationStrategy<LibtcodMap> mapCreationStrategy )
      {
         return mapCreationStrategy.CreateMap();
      }

      public void Initialize( int width, int height )
      {
         TCODMap = new TCODMap( width, height );
      }

      public bool IsTransparent( int x, int y )
      {
         return TCODMap.isTransparent( x, y );
      }

      public bool IsWalkable( int x, int y )
      {
         return TCODMap.isWalkable( x, y );
      }

      public bool IsInFov( int x, int y )
      {
         return TCODMap.isInFov( x, y );
      }

      public bool IsExplored( int x, int y )
      {
         throw new NotImplementedException();
      }

      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored )
      {
         TCODMap.setProperties( x, y, isTransparent, isWalkable );
      }

      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable )
      {
         TCODMap.setProperties( x, y, isTransparent, isWalkable );
      }

      public void Clear()
      {
         throw new NotImplementedException();
      }

      public void Clear( bool isTransparent = false, bool isWalkable = false )
      {
         TCODMap.clear( isTransparent, isWalkable );
      }

      public IMap Clone()
      {
         TCODMap map = new TCODMap( Width, Height );
         map.copy( TCODMap );
         return new LibtcodMap( map );
      }

      public void Copy( IMap sourceMap )
      {
         throw new NotImplementedException();
      }

      public void Copy( IMap sourceMap, int left = 0, int top = 0 )
      {
         if ( sourceMap.Width + left > Width )
         {
            throw new ArgumentException( "Source map 'width' + 'left' cannot be larger than the destination map width", "destinationMap" );
         }
         if ( sourceMap.Height + top > Height )
         {
            throw new ArgumentException( "Source map 'height' + 'top' cannot be larger than the destination map height", "destinationMap" );
         }
         if ( sourceMap is LibtcodMap && left == 0 && top == 0 )
         {
            LibtcodMap mapToCopyFrom = sourceMap as LibtcodMap;
            TCODMap.copy( mapToCopyFrom.TCODMap );
         }
         else
         {
            foreach ( Cell cell in sourceMap.GetAllCells() )
            {
               SetCellProperties( cell.X + left, cell.Y + top, cell.IsTransparent, cell.IsWalkable );
            }
         }
      }

      public void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         TCODMap.computeFov( xOrigin, yOrigin, radius, lightWalls );
      }

      public void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         throw new NotImplementedException( "Not implemented for Libtcod Maps" );
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

      public IEnumerable<Cell> GetCellsAlongLine( int xStart, int yStart, int xEnd, int yEnd )
      {
         TCODLine.init( xStart, yStart, xEnd, yEnd );
         int x = xStart;
         int y = yStart;
         do
         {
            yield return GetCell( x, y );
         } while ( !TCODLine.step( ref x, ref y ) );

         yield break;
      }

      public IEnumerable<Cell> GetCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         HashSet<Cell> discovered = new HashSet<Cell>();

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
                  yield return cell;
               if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Max( yMin, yOrigin - radius + j ), out cell ) )
                  yield return cell;
               if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Min( yMax, yOrigin + radius - j ), out cell ) )
                  yield return cell;
               if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Max( yMin, yOrigin - radius + j ), out cell ) )
                  yield return cell;
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
         HashSet<Cell> discovered = new HashSet<Cell>();

         int xMin = Math.Max( 0, xOrigin - radius );
         int xMax = Math.Min( Width - 1, xOrigin + radius );
         int yMin = Math.Max( 0, yOrigin - radius );
         int yMax = Math.Min( Height - 1, yOrigin + radius );

         Cell cell;
         if ( AddToHashSet( discovered, xOrigin, yMin, out cell ) )
            yield return cell;
         if ( AddToHashSet( discovered, xOrigin, yMax, out cell ) )
            yield return cell;
         for ( int i = 1; i <= radius; i++ )
         {
            if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Min( yMax, yOrigin + radius - i ), out cell ) )
               yield return cell;
            if ( AddToHashSet( discovered, Math.Max( xMin, xOrigin - i ), Math.Max( yMin, yOrigin - radius + i ), out cell ) )
               yield return cell;
            if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Min( yMax, yOrigin + radius - i ), out cell ) )
               yield return cell;
            if ( AddToHashSet( discovered, Math.Min( xMax, xOrigin + i ), Math.Max( yMin, yOrigin - radius + i ), out cell ) )
               yield return cell;
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

      private bool AddToHashSet( HashSet<Cell> hashSet, int x, int y, out Cell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( cell );
      }

      public Cell GetCell( int x, int y )
      {
         return new Cell( x, y, TCODMap.isTransparent( x, y ), TCODMap.isWalkable( x, y ), TCODMap.isInFov( x, y ) );
      }

      public override string ToString()
      {
         return ToString( false );
      }

      public string ToString( bool useFov )
      {
         StringBuilder mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( Cell cell in GetAllCells() )
         {
            if ( cell.Y != lastY )
            {
               lastY = cell.Y;
               mapRepresentation.Append( Environment.NewLine );
            }
            mapRepresentation.Append( cell.ToString( useFov ) );
         };
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      public MapState Save()
      {
         throw new NotImplementedException();
      }

      public void Restore( MapState state )
      {
         throw new NotImplementedException();
      }

      public Cell CellFor( int index )
      {
         int x = index % Width;
         int y = index / Width;

         return GetCell( x, y );
      }

      public int IndexFor( int x, int y )
      {
         return ( y * Width ) + x;
      }

      public int IndexFor( Cell cell )
      {
         return ( cell.Y * Width ) + cell.X;
      }
   }
}
