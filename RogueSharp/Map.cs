using System;
using System.Collections.Generic;
using System.Text;
using RogueSharp.MapCreation;

namespace RogueSharp
{
   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability, field-of-view and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public class Map : IMap
   {
      private FieldOfView _fieldOfView;
      private bool[,] _isTransparent;
      private bool[,] _isWalkable;
      private bool[,] _isExplored;

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
         Initialize( width, height );
      }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Width of 10 will have Cells with X locations of 0 through 9
      /// Cells with an X value of 0 will be the leftmost Cells
      /// </remarks>
      public int Width { get; private set; }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Height of 20 will have Cells with Y locations of 0 through 19
      /// Cells with an Y value of 0 will be the topmost Cells
      /// </remarks>
      public int Height { get; private set; }

      /// <summary>
      /// Create a new map with the properties of all Cells set to false
      /// </summary>
      /// <remarks>
      /// This is basically a solid stone map that would then need to be modified to have interesting features
      /// </remarks>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
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
         return _isTransparent[x, y];
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
         return _isWalkable[x, y];
      }

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
      /// <param name="x">X location of the Cell to check starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to check, starting with 0 as the top</param>
      /// <returns>True if the Cell is in the currently computed field-of-view, false otherwise</returns>
      public bool IsInFov( int x, int y )
      {
         return _fieldOfView.IsInFov( x, y );
      }

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
      public bool IsExplored( int x, int y )
      {
         return _isExplored[x, y];
      }

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
      /// <param name="isExplored">True if the Cell has ever been in the field-of-view of the player. False otherwise</param>
      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable, bool isExplored )
      {
         _isTransparent[x, y] = isTransparent;
         _isWalkable[x, y] = isWalkable;
         _isExplored[x, y] = isExplored;
      }

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
      public void SetCellProperties( int x, int y, bool isTransparent, bool isWalkable )
      {
         SetCellProperties( x, y, isTransparent, isWalkable, false );
      }

      /// <summary>
      /// Sets the properties of all Cells in the Map to be transparent and walkable
      /// </summary>
      public void Clear()
      {
         Clear( true, true );
      }

      /// <summary>
      /// Sets the properties of all Cells in the Map to the specified values
      /// </summary>
      /// <param name="isTransparent">Optional parameter defaults to true if not provided. True if line-of-sight is not blocked by this Cell. False otherwise</param>
      /// <param name="isWalkable">Optional parameter defaults to true if not provided. True if a character could walk across the Cell normally. False otherwise</param>
      public void Clear( bool isTransparent, bool isWalkable )
      {
         foreach ( ICell cell in GetAllCells() )
         {
            SetCellProperties( cell.X, cell.Y, isTransparent, isWalkable );
         }
      }

      /// <summary>
      /// Create and return a deep copy of an existing Map
      /// </summary>
      /// <returns>IMap deep copy of the original Map</returns>
      public IMap Clone()
      {
         var map = new Map( Width, Height );
         foreach ( ICell cell in GetAllCells() )
         {
            map.SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, cell.IsExplored );
         }
         return map;
      }

      /// <summary>
      /// Copies the Cell properties of a smaller source Map into this destination Map at location (0,0)
      /// </summary>
      /// <param name="sourceMap">An IMap which must be of smaller size and able to fit in this destination Map at the specified location</param>
      public void Copy( IMap sourceMap )
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
      public void Copy( IMap sourceMap, int left, int top )
      {
         if ( sourceMap == null )
         {
            throw new ArgumentNullException( "sourceMap", "Source map cannot be null" );
         }

         if ( sourceMap.Width + left > Width )
         {
            throw new ArgumentException( "Source map 'width' + 'left' cannot be larger than the destination map width" );
         }
         if ( sourceMap.Height + top > Height )
         {
            throw new ArgumentException( "Source map 'height' + 'top' cannot be larger than the destination map height" );
         }
         foreach ( ICell cell in sourceMap.GetAllCells() )
         {
            SetCellProperties( cell.X + left, cell.Y + top, cell.IsTransparent, cell.IsWalkable, cell.IsExplored );
         }
      }

      /// <summary>
      /// Performs a field-of-view calculation with the specified parameters.
      /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius.
      /// Any existing field-of-view calculations will be overwritten when calling this method.
      /// </summary>
      /// <param name="xOrigin">X location of the Cell to perform the field-of-view calculation with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Cell to perform the field-of-view calculation with 0 as the top</param>
      /// <param name="radius">The number of Cells in which the field-of-view extends from the origin Cell. Think of this as the intensity of the light source.</param>
      /// <param name="lightWalls">True if walls should be included in the field-of-view when they are within the radius of the light source. False excludes walls even when they are within range.</param>
      public void ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         _fieldOfView.ComputeFov( xOrigin, yOrigin, radius, lightWalls );
      }

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
      public void AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         _fieldOfView.AppendFov( xOrigin, yOrigin, radius, lightWalls );
      }

      /// <summary>
      /// Get an IEnumerable of all Cells in the Map
      /// </summary>
      /// <returns>IEnumerable of all Cells in the Map</returns>
      public IEnumerable<ICell> GetAllCells()
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
      public IEnumerable<ICell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination )
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
      /// Get an IEnumerable of Cells in a circular Radius around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the Origin Cell</param>
      /// <returns>IEnumerable of Cells in a circular Radius around the Origin Cell</returns>
      public IEnumerable<ICell> GetCellsInRadius( int xOrigin, int yOrigin, int radius )
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
               ICell cell;
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
      /// Get an IEnumerable of Cells in a square area around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the Origin Cell</param>
      /// <returns>IEnumerable of Cells in a square area around the Origin Cell</returns>
      public IEnumerable<ICell> GetCellsInArea( int xOrigin, int yOrigin, int distance )
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
      /// Get an IEnumerable of the outermost border Cells in a circular Radius around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="radius">The radius from the Origin Cell in which the border Cells lie</param>
      /// <returns>IEnumerable of the outermost border Cells in a circular Radius around the Origin Cell</returns>
      public IEnumerable<ICell> GetBorderCellsInRadius( int xOrigin, int yOrigin, int radius )
      {
         var discovered = new HashSet<int>();

         int xMin = Math.Max( 0, xOrigin - radius );
         int xMax = Math.Min( Width - 1, xOrigin + radius );
         int yMin = Math.Max( 0, yOrigin - radius );
         int yMax = Math.Min( Height - 1, yOrigin + radius );

         ICell cell;
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
      /// Get an IEnumerable of the outermost border Cells in a square around the Origin Cell
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell with 0 as the top</param>
      /// <param name="distance">The distance from the Origin Cell in which the border Cells lie</param>
      /// <returns> IEnumerable of the outermost border Cells in a square around the Origin Cell</returns>
      public IEnumerable<ICell> GetBorderCellsInArea( int xOrigin, int yOrigin, int distance )
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
      /// Get an IEnumerable of all the Cells in the specified row numbers
      /// </summary>
      /// <param name="rowNumbers">Integer array of row numbers with 0 as the top</param>
      /// <returns>IEnumerable of all the Cells in the specified row numbers</returns>
      public IEnumerable<ICell> GetCellsInRows( params int[] rowNumbers )
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
      public IEnumerable<ICell> GetCellsInColumns( params int[] columnNumbers )
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
      /// Get a Cell at the specified location
      /// </summary>
      /// <param name="x">X location of the Cell to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to get, starting with 0 as the top</param>
      /// <returns>Cell at the specified location</returns>
      public ICell GetCell( int x, int y )
      {
         return new Cell( x, y, _isTransparent[x, y], _isWalkable[x, y], _fieldOfView.IsInFov( x, y ), _isExplored[x, y] );
      }

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
      public string ToString( bool useFov )
      {
         var mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( ICell iCell in GetAllCells() )
         {
            Cell cell = ( Cell ) iCell;
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
      /// Get a MapState POCO which represents this Map and can be easily serialized
      /// Use Restore with the MapState to get back a full Map
      /// </summary>
      /// <returns>Mapstate POCO (Plain Old C# Object) which represents this Map and can be easily serialized</returns>
      public MapState Save()
      {
         var mapState = new MapState();
         mapState.Width = Width;
         mapState.Height = Height;
         mapState.Cells = new MapState.CellProperties[Width * Height];
         foreach ( ICell cell in GetAllCells() )
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
      /// Restore the state of this Map from the specified MapState
      /// </summary>
      /// <param name="state">Mapstate POCO (Plain Old C# Object) which represents this Map and can be easily serialized and deserialized</param>
      /// <exception cref="ArgumentNullException">Thrown on null map state</exception>
      public void Restore( MapState state )
      {
         if ( state == null )
         {
            throw new ArgumentNullException( "state", "Map state cannot be null" );
         }

         var inFov = new HashSet<int>();

         Initialize( state.Width, state.Height );
         foreach ( ICell cell in GetAllCells() )
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
      /// Static factory method which creates a new Map using the specified IMapCreationStrategy
      /// </summary>
      /// <remarks>
      /// Several classes implementing IMapCreationStrategy are included with RogueSharp but they are very basic
      /// It recommended that you create your own class implementing this interface to create more interesting Maps
      /// </remarks>
      /// <param name="mapCreationStrategy">A class that implements IMapCreationStrategy and has CreateMap method which defines algorithms for creating interesting Maps</param>
      /// <returns>Map created by calling CreateMap from the specified IMapCreationStrategy</returns>
      /// <exception cref="ArgumentNullException">Thrown on null map creation strategy</exception>
      public static Map Create( IMapCreationStrategy<Map> mapCreationStrategy )
      {
         if ( mapCreationStrategy == null )
         {
            throw new ArgumentNullException( "mapCreationStrategy", "Map creation strategy cannot be null" );
         }

         return mapCreationStrategy.CreateMap();
      }

      /// <summary>
      /// Get the Cell at the specified single dimensional array index using the formulas: x = index % Width; y = index / Width;
      /// </summary>
      /// <param name="index">The single dimensional array index for the Cell that we want to get</param>
      /// <returns>Cell at the specified single dimensional array index</returns>
      public ICell CellFor( int index )
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
      public int IndexFor( ICell cell )
      {
         if ( cell == null )
         {
            throw new ArgumentNullException( "cell", "Cell cannot be null" );
         }

         return ( cell.Y * Width ) + cell.X;
      }

      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, out ICell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( IndexFor( cell ) );
      }

      /// <summary>
      /// Provides a simple visual representation of the map using the following symbols:
      /// - `.`: `Cell` is transparent and walkable
      /// - `s`: `Cell` is walkable (but not transparent)
      /// - `o`: `Cell` is transparent (but not walkable)
      /// - `#`: `Cell` is not transparent or walkable
      /// </summary>
      /// <remarks>
      /// This call ignores field-of-view. If field-of-view is important use the ToString overload with a "true" parameter
      /// </remarks>
      /// <returns>A string representation of the map using special symbols to denote Cell properties</returns>
      public override string ToString()
      {
         return ToString( false );
      }
   }
}