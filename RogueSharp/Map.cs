using System;
using System.Collections.Generic;
using System.Text;
using RogueSharp.MapCreation;

namespace RogueSharp
{
   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability, transparency and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public class Map : Map<Cell>, IMap
   {
      /// <summary>
      /// Constructor creates a new uninitialized Map
      /// </summary>
      public Map()
      {
      }

      /// <summary>
      /// Constructor creates a new Map and immediately initializes it
      /// </summary>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public Map( int width, int height )
      : base( width, height )
      {
      }

      /// <summary>
      /// Static factory method which creates a new Map using the specified IMapCreationStrategy
      /// </summary>
      /// <remarks>
      /// Several classes implementing IMapCreationStrategy are included with RogueSharp but they are very basic
      /// It recommended that you create your own class implementing this interface to create more interesting Maps
      /// </remarks>
      /// <param name="mapCreationStrategy">A class that implements IMapCreationStrategy and has CreateMap method which defines algorithms for creating interesting Maps</param>
      /// <returns>Map created by calling CreateMap from the specified IMapCreationStrategy</returns>
      /// <exception cref="ArgumentNullException">Thrown on null map creation strategy</exception>
      public static TMap Create<TMap>( IMapCreationStrategy<TMap> mapCreationStrategy ) where TMap : IMap<Cell>
      {
         if ( mapCreationStrategy == null )
         {
            throw new ArgumentNullException( nameof( mapCreationStrategy ), "Map creation strategy cannot be null" );
         }

         return mapCreationStrategy.CreateMap();
      }

      /// <summary>
      /// Static factory method which creates a new Map using the specified IMapCreationStrategy
      /// </summary>
      /// <remarks>
      /// Several classes implementing IMapCreationStrategy are included with RogueSharp but they are very basic
      /// It recommended that you create your own class implementing this interface to create more interesting Maps
      /// </remarks>
      /// <param name="mapCreationStrategy">A class that implements IMapCreationStrategy and has CreateMap method which defines algorithms for creating interesting Maps</param>
      /// <returns>Map created by calling CreateMap from the specified IMapCreationStrategy</returns>
      /// <exception cref="ArgumentNullException">Thrown on null map creation strategy</exception>
      public static TMap Create<TMap, TCell>( IMapCreationStrategy<TMap, TCell> mapCreationStrategy ) where TMap : IMap<TCell> where TCell : ICell
      {
         if ( mapCreationStrategy == null )
         {
            throw new ArgumentNullException( nameof( mapCreationStrategy ), "Map creation strategy cannot be null" );
         }

         return mapCreationStrategy.CreateMap();
      }
   }

   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability, field-of-view and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public class Map<TCell> : IMap<TCell> where TCell : ICell
   {
      private TCell[,] _cells;

      /// <summary>
      /// Constructor creates a new uninitialized Map
      /// </summary>
      public Map()
      {
      }

      /// <summary>
      /// Constructor creates a new Map and immediately initializes it
      /// </summary>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public Map( int width, int height )
      {
         Init( width, height );
      }

      /// <summary>
      /// This Indexer allows direct access to Cells given x and y index
      /// </summary>
      /// <param name="x">X index of the Cell to get</param>
      /// <param name="y">Y index of the Cell to get</param>
      /// <returns>Cell at the specified index</returns>
      public TCell this[int x, int y]
      {
         get => _cells[x, y];
         set => _cells[x, y] = value;
      }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Width of 10 will have Cells with X locations of 0 through 9
      /// Cells with an X value of 0 will be the leftmost Cells
      /// </remarks>
      public int Width
      {
         get; private set;
      }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Height of 20 will have Cells with Y locations of 0 through 19
      /// Cells with an Y value of 0 will be the topmost Cells
      /// </remarks>
      public int Height
      {
         get; private set;
      }

      /// <summary>
      /// Create a new map with the properties of all Cells set to false
      /// </summary>
      /// <remarks>
      /// This is basically a solid stone map that would then need to be modified to have interesting features. Override to initialize other internal state
      /// </remarks>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public virtual void Initialize( int width, int height )
      {
         Init( width, height );
      }

      private void Init( int width, int height )
      {
         Width = width;
         Height = height;
         _cells = new TCell[width, height];
         for ( int x = 0; x < width; x++ )
         {
            for ( int y = 0; y < height; y++ )
            {
               _cells[x, y] = Activator.CreateInstance<TCell>();
               _cells[x, y].X = x;
               _cells[x, y].Y = y;
            }
         }
      }

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
      public bool IsTransparent( int x, int y )
      {
         return _cells[x, y].IsTransparent;
      }

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
      public bool IsWalkable( int x, int y )
      {
         return _cells[x, y].IsWalkable;
      }

      /// <summary>
      /// Set the properties of an unexplored Cell to the specified values
      /// </summary>
      /// <param name="x">X location of the Cell to set properties on, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to set properties on, starting with 0 as the top</param>
      /// <param name="isTransparent">True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">True if a character could walk across the Cell normally. False otherwise</param>
      public virtual void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable )
      {
         _cells[x, y].IsTransparent = isTransparent;
         _cells[x, y].IsWalkable = isWalkable;
      }

      /// <summary>
      /// Sets the properties of all Cells in the Map to be transparent and walkable
      /// </summary>
      public virtual void Clear()
      {
         Clear( true, true );
      }

      /// <summary>
      /// Sets the properties of all Cells in the Map to the specified values
      /// </summary>
      /// <param name="isTransparent">Optional parameter defaults to true if not provided. True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">Optional parameter defaults to true if not provided. True if a character could walk across the Cell normally. False otherwise</param>
      public virtual void Clear( bool isTransparent, bool isWalkable )
      {
         foreach ( TCell cell in GetAllCells() )
         {
            SetCellProperties( cell.X, cell.Y, isTransparent, isWalkable );
         }
      }

      /// <summary>
      /// Create and return a deep copy of an existing Map.
      /// Override when a derived class has additional properties to clone.
      /// </summary>
      /// <returns>T of type IMap which is a deep copy of the original Map</returns>
      public virtual TMap Clone<TMap>() where TMap : IMap<TCell>, new()
      {
         var map = new TMap();
         map.Initialize( Width, Height );
         map.Clear( true, true );

         foreach ( TCell cell in GetAllCells() )
         {
            map.SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable );
         }
         return map;
      }

      /// <summary>
      /// Copies the Cell properties of a smaller source Map into this destination Map at location (0,0)
      /// </summary>
      /// <param name="sourceMap">An IMap which must be of smaller size and able to fit in this destination Map at the specified location</param>
      public virtual void Copy( IMap<TCell> sourceMap )
      {
         Copy( sourceMap, 0, 0 );
      }

      /// <summary>
      /// Copies the Cell properties of a smaller source Map into this destination Map at the specified location
      /// </summary>
      /// <param name="sourceMap">An IMap which must be of smaller size and able to fit in this destination Map at the specified location</param>
      /// <param name="left">Optional parameter defaults to 0 if not provided. X location of the Cell to start copying parameters to, starting with 0 as the farthest left</param>
      /// <param name="top">Optional parameter defaults to 0 if not provided. Y location of the Cell to start copying parameters to, starting with 0 as the top</param>
      /// <exception cref="ArgumentNullException">Thrown on null source map</exception>
      /// <exception cref="ArgumentException">Thrown on invalid source map dimensions</exception>
      public virtual void Copy( IMap<TCell> sourceMap, int left, int top )
      {
         if ( sourceMap == null )
         {
            throw new ArgumentNullException( nameof( sourceMap ), "Source map cannot be null" );
         }

         if ( sourceMap.Width + left > Width )
         {
            throw new ArgumentException( "Source map 'width' + 'left' cannot be larger than the destination map width" );
         }
         if ( sourceMap.Height + top > Height )
         {
            throw new ArgumentException( "Source map 'height' + 'top' cannot be larger than the destination map height" );
         }
         foreach ( TCell cell in sourceMap.GetAllCells() )
         {
            SetCellProperties( cell.X + left, cell.Y + top, cell.IsTransparent, cell.IsWalkable );
         }
      }

      /// <summary>
      /// Get an IEnumerable of all Cells in the Map
      /// </summary>
      /// <returns>IEnumerable of all Cells in the Map</returns>
      public IEnumerable<TCell> GetAllCells()
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
      /// Get an IEnumerable of Cells in a line from the Origin Cell to the Destination Cell
      /// The resulting IEnumerable includes the Origin and Destination Cells
      /// Uses Bresenham's line algorithm to determine which Cells are in the closest approximation to a straight line between the two Cells
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell at the start of the line with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell at the start of the line with 0 as the top</param>
      /// <param name="xDestination">X location of the Destination Cell at the end of the line with 0 as the farthest left</param>
      /// <param name="yDestination">Y location of the Destination Cell at the end of the line with 0 as the top</param>
      /// <returns>IEnumerable of Cells in a line from the Origin Cell to the Destination Cell which includes the Origin and Destination Cells</returns>
      public IEnumerable<TCell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination )
      {
         xOrigin = ClampX( xOrigin );
         yOrigin = ClampY( yOrigin );
         xDestination = ClampX( xDestination );
         yDestination = ClampY( yDestination );

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

      private int ClampX( int x )
      {
         return ( x < 0 ) ? 0 : ( x > Width - 1 ) ? Width - 1 : x;
      }

      private int ClampY( int y )
      {
         return ( y < 0 ) ? 0 : ( y > Height - 1 ) ? Height - 1 : y;
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of Cells in a circle around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInCircle( int xCenter, int yCenter, int radius )
      {
         var discovered = new HashSet<int>();

         int d = ( 5 - ( radius * 4 ) ) / 4;
         int x = 0;
         int y = radius;

         do
         {
            foreach ( TCell cell in GetCellsAlongLine( xCenter + x, yCenter + y, xCenter - x, yCenter + y ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter - x, yCenter - y, xCenter + x, yCenter - y ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter + y, yCenter + x, xCenter - y, yCenter + x ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter + y, yCenter - x, xCenter - y, yCenter - x ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }

            if ( d < 0 )
            {
               d += ( 2 * x ) + 1;
            }
            else
            {
               d += ( 2 * ( x - y ) ) + 1;
               y--;
            }
            x++;
         } while ( x <= y );
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInDiamond( int xCenter, int yCenter, int distance )
      {
         var discovered = new HashSet<int>();

         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         for ( int i = 0; i <= distance; i++ )
         {
            for ( int j = distance; j >= 0 + i; j-- )
            {
               if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Min( yMax, yCenter + distance - j ), out TCell cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Max( yMin, yCenter - distance + j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Min( yMax, yCenter + distance - j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Max( yMin, yCenter - distance + j ), out cell ) )
               {
                  yield return cell;
               }
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of Cells in a square area around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInSquare( int xCenter, int yCenter, int distance )
      {
         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         for ( int y = yMin; y <= yMax; y++ )
         {
            for ( int x = xMin; x <= xMax; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a rectangle area
      /// </summary>
      /// <param name="top">The top row of the rectangle </param>
      /// <param name="left">The left column of the rectangle</param>
      /// <param name="width">The width of the rectangle</param>
      /// <param name="height">The height of the rectangle</param>
      /// <returns>IEnumerable of Cells in a rectangle area</returns>
      public IEnumerable<TCell> GetCellsInRectangle( int top, int left, int width, int height )
      {
         int xMin = Math.Max( 0, left );
         int xMax = Math.Min( Width, left + width );
         int yMin = Math.Max( 0, top );
         int yMax = Math.Min( Height, top + height );

         for ( int y = yMin; y < yMax; y++ )
         {
            for ( int x = xMin; x < xMax; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a circle around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInCircle( int xCenter, int yCenter, int radius )
      {
         var discovered = new HashSet<int>();

         int d = ( 5 - ( radius * 4 ) ) / 4;
         int x = 0;
         int y = radius;

         TCell centerCell = GetCell( xCenter, yCenter );

         do
         {
            if ( AddToHashSet( discovered, ClampX( xCenter + x ), ClampY( yCenter + y ), centerCell, out TCell cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + x ), ClampY( yCenter - y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - x ), ClampY( yCenter + y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - x ), ClampY( yCenter - y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + y ), ClampY( yCenter + x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + y ), ClampY( yCenter - x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - y ), ClampY( yCenter + x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - y ), ClampY( yCenter - x ), centerCell, out cell ) )
            {
               yield return cell;
            }

            if ( d < 0 )
            {
               d += ( 2 * x ) + 1;
            }
            else
            {
               d += ( 2 * ( x - y ) ) + 1;
               y--;
            }
            x++;
         } while ( x <= y );
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInDiamond( int xCenter, int yCenter, int distance )
      {
         var discovered = new HashSet<int>();

         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         TCell centerCell = GetCell( xCenter, yCenter );
         if ( AddToHashSet( discovered, xCenter, yMin, centerCell, out TCell cell ) )
         {
            yield return cell;
         }
         if ( AddToHashSet( discovered, xCenter, yMax, centerCell, out cell ) )
         {
            yield return cell;
         }
         for ( int i = 1; i <= distance; i++ )
         {
            if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Min( yMax, yCenter + distance - i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Max( yMin, yCenter - distance + i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Min( yMax, yCenter + distance - i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Max( yMin, yCenter - distance + i ), centerCell, out cell ) )
            {
               yield return cell;
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a square area around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInSquare( int xCenter, int yCenter, int distance )
      {
         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );
         List<TCell> borderCells = new List<TCell>();

         for ( int x = xMin; x <= xMax; x++ )
         {
            borderCells.Add( GetCell( x, yMin ) );
            borderCells.Add( GetCell( x, yMax ) );
         }
         for ( int y = yMin + 1; y <= yMax - 1; y++ )
         {
            borderCells.Add( GetCell( xMin, y ) );
            borderCells.Add( GetCell( xMax, y ) );
         }

         TCell centerCell = GetCell( xCenter, yCenter );
         borderCells.Remove( centerCell );

         return borderCells;
      }

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified row numbers
      /// </summary>
      /// <param name="rowNumbers">Integer array of row numbers with 0 as the top</param>
      /// <returns>IEnumerable of all the Cells in the specified row numbers</returns>
      public IEnumerable<TCell> GetCellsInRows( params int[] rowNumbers )
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
      /// Get an IEnumerable of all the Cells in the specified column numbers
      /// </summary>
      /// <param name="columnNumbers">Integer array of column numbers with 0 as the farthest left</param>
      /// <returns>IEnumerable of all the Cells in the specified column numbers</returns>
      public IEnumerable<TCell> GetCellsInColumns( params int[] columnNumbers )
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
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells do not count as adjacent.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      public IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter )
      {
         return GetAdjacentCells( xCenter, yCenter, false );
      }

      /// <summary>
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells may optionally be included.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="includeDiagonals">Should diagonal Cells count as being adjacent cells?</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      public IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter, bool includeDiagonals )
      {
         int topY = yCenter - 1;
         int bottomY = yCenter + 1;
         int leftX = xCenter - 1;
         int rightX = xCenter + 1;

         if ( topY >= 0 )
         {
            yield return GetCell( xCenter, topY );
         }

         if ( leftX >= 0 )
         {
            yield return GetCell( leftX, yCenter );
         }

         if ( bottomY < Height )
         {
            yield return GetCell( xCenter, bottomY );
         }

         if ( rightX < Width )
         {
            yield return GetCell( rightX, yCenter );
         }

         if ( includeDiagonals )
         {
            if ( rightX < Width && topY >= 0 )
            {
               yield return GetCell( rightX, topY );
            }

            if ( rightX < Width && bottomY < Height )
            {
               yield return GetCell( rightX, bottomY );
            }

            if ( leftX >= 0 && topY >= 0 )
            {
               yield return GetCell( leftX, topY );
            }

            if ( leftX >= 0 && bottomY < Height )
            {
               yield return GetCell( leftX, bottomY );
            }
         }
      }

      /// <summary>
      /// Get a Cell at the specified location
      /// </summary>
      /// <param name="x">X location of the Cell to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to get, starting with 0 as the top</param>
      /// <returns>Cell at the specified location</returns>
      public TCell GetCell( int x, int y )
      {
         return _cells[x, y];
      }

      /// <summary>
      /// Provides a simple visual representation of the map using the following symbols:
      /// - `.`: `Cell` is transparent and walkable
      /// - `s`: `Cell` is walkable (but not transparent)
      /// - `o`: `Cell` is transparent (but not walkable)
      /// - `#`: `Cell` is not transparent or walkable
      /// </summary>
      /// <returns>A string representation of the map using special symbols to denote Cell properties</returns>
      public override string ToString()
      {
         var mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( ICell iCell in GetAllCells() )
         {
            Cell cell = (Cell) iCell;
            if ( cell.Y != lastY )
            {
               lastY = cell.Y;
               mapRepresentation.Append( Environment.NewLine );
            }
            mapRepresentation.Append( cell.ToString() );
         }
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      /// <summary>
      /// Get a MapState POCO which represents this Map and can be easily serialized
      /// Use Restore with the MapState to get back a full Map
      /// </summary>
      /// <returns>MapState POCO (Plain Old C# Object) which represents this Map and can be easily serialized</returns>
      public MapState Save()
      {
         var mapState = new MapState
         {
            Width = Width,
            Height = Height,
            Cells = new MapState.CellProperties[Width * Height]
         };
         foreach ( TCell cell in GetAllCells() )
         {
            MapState.CellProperties cellProperties = MapState.CellProperties.None;
            if ( cell.IsTransparent )
            {
               cellProperties |= MapState.CellProperties.Transparent;
            }
            if ( cell.IsWalkable )
            {
               cellProperties |= MapState.CellProperties.Walkable;
            }
            mapState.Cells[( cell.Y * Width ) + cell.X] = cellProperties;
         }
         return mapState;
      }

      /// <summary>
      /// Restore the state of this Map from the specified MapState
      /// </summary>
      /// <param name="state">MapState POCO (Plain Old C# Object) which represents this Map and can be easily serialized and deserialized</param>
      /// <exception cref="ArgumentNullException">Thrown on null map state</exception>
      public void Restore( MapState state )
      {
         if ( state == null )
         {
            throw new ArgumentNullException( nameof( state ), "Map state cannot be null" );
         }

         Initialize( state.Width, state.Height );
         foreach ( TCell cell in GetAllCells() )
         {
            MapState.CellProperties cellProperties = state.Cells[( cell.Y * Width ) + cell.X];

            _cells[cell.X, cell.Y].IsTransparent = cellProperties.HasFlag( MapState.CellProperties.Transparent );
            _cells[cell.X, cell.Y].IsWalkable = cellProperties.HasFlag( MapState.CellProperties.Walkable );
         }
      }

      /// <summary>
      /// Get the Cell at the specified single dimensional array index using the formulas: x = index % Width; y = index / Width;
      /// </summary>
      /// <param name="index">The single dimensional array index for the Cell that we want to get</param>
      /// <returns>Cell at the specified single dimensional array index</returns>
      public TCell CellFor( int index )
      {
         int x = index % Width;
         int y = index / Width;

         return GetCell( x, y );
      }

      /// <summary>
      /// Get the single dimensional array index for a Cell at the specified location using the formula: index = ( y * Width ) + x
      /// </summary>
      /// <param name="x">X location of the Cell index to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell index to get, starting with 0 as the top</param>
      /// <returns>An index for the Cell at the specified location useful if storing Cells in a single dimensional array</returns>
      public int IndexFor( int x, int y )
      {
         return ( y * Width ) + x;
      }

      /// <summary>
      /// Get the single dimensional array index for the specified Cell
      /// </summary>
      /// <param name="cell">The Cell to get the index for</param>
      /// <returns>An index for the Cell which is useful if storing Cells in a single dimensional array</returns>
      /// <exception cref="ArgumentNullException">Thrown on null cell</exception>
      public int IndexFor( TCell cell )
      {
         if ( cell == null )
         {
            throw new ArgumentNullException( nameof( cell ), "Cell cannot be null" );
         }

         return ( cell.Y * Width ) + cell.X;
      }

      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, out TCell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( IndexFor( cell ) );
      }

      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, TCell centerCell, out TCell cell )
      {
         cell = GetCell( x, y );
         if ( cell.Equals( centerCell ) )
         {
            return false;
         }

         return hashSet.Add( IndexFor( cell ) );
      }

      private bool AddToHashSet( HashSet<int> hashSet, TCell cell )
      {
         return hashSet.Add( IndexFor( cell ) );
      }
   }
}