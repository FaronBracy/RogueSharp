using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RogueSharp
{
   /// <summary>
   /// A class for performing field-of-view calculations to determine what is observable in a Map from a given Cell within a given light radius
   /// </summary>
   /// <seealso href="https://sites.google.com/site/jicenospam/visibilitydetermination">Based on the visibility determination algorithm described here</seealso>
   public class FieldOfView
   {
      private readonly IMap _map;
      private readonly HashSet<int> _inFov;

      /// <summary>
      /// Constructs a new FieldOfView class for the specified Map
      /// </summary>
      /// <param name="map">The Map that this FieldOfView class will use to perform its field-of-view calculations</param>
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

      /// <summary>
      /// Create and return a deep copy of an existing FieldOfView class
      /// </summary>
      /// <returns>A deep copy of an existing FieldOfViewClass</returns>
      public FieldOfView Clone()
      {
         var inFovCopy = new HashSet<int>();
         foreach ( int i in _inFov )
         {
            inFovCopy.Add( i );
         }
         return new FieldOfView( _map, inFovCopy );
      }

      /// <summary>
      /// Check if the Cell is in the currently computed field-of-view
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
         return _inFov.Contains( _map.IndexFor( x, y ) );
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
      /// <returns>List of Cells representing what is observable in the Map based on the specified parameters</returns>
      public ReadOnlyCollection<ICell> ComputeFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         ClearFov();
         return AppendFov( xOrigin, yOrigin, radius, lightWalls );
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
      /// <returns>List of Cells representing what is observable in the Map based on the specified parameters</returns>
      public ReadOnlyCollection<ICell> AppendFov( int xOrigin, int yOrigin, int radius, bool lightWalls )
      {
         foreach ( ICell borderCell in _map.GetBorderCellsInSquare( xOrigin, yOrigin, radius ) )
         {
            foreach ( ICell cell in _map.GetCellsAlongLine( xOrigin, yOrigin, borderCell.X, borderCell.Y ) )
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
            // Post processing step created based on the algorithm at this website:
            // https://sites.google.com/site/jicenospam/visibilitydetermination
            foreach ( ICell cell in _map.GetCellsInSquare( xOrigin, yOrigin, radius ) )
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

      private ReadOnlyCollection<ICell> CellsInFov()
      {
         var cells = new List<ICell>();
         foreach ( int index in _inFov )
         {
            cells.Add( _map.CellFor( index ) );
         }
         return new ReadOnlyCollection<ICell>( cells );
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
