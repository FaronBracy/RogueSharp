using System;
using System.Collections.Generic;
using System.Linq;
using RogueSharp.Algorithms;

namespace RogueSharp
{
   /// <summary>
   /// A class which can be used to find shortest path from a source to a destination in a Map
   /// </summary>
   public class PathFinder
   {
      private readonly EdgeWeightedDigraph _graph;
      private readonly IMap _map;

      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown on null map</exception>
      public PathFinder( IMap map )
      {
         if ( map == null )
         {
            throw new ArgumentNullException( "map", "Map cannot be null" );
         }

         _map = map;
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( ICell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = IndexFor( cell );
               foreach ( ICell neighbor in _map.GetBorderCellsInRadius( cell.X, cell.Y, 1 ) )
               {
                  if ( neighbor.IsWalkable )
                  {
                     int w = IndexFor( neighbor );
                     _graph.AddEdge( new DirectedEdge( v, w, 1.0 ) );
                     _graph.AddEdge( new DirectedEdge( w, v, 1.0 ) );
                  }
               }
            }
         }
      }

      /// <summary>
      /// Returns an ordered IEnumerable of Cells representing the shortest path from a specified source Cell to a destination Cell
      /// </summary>
      /// <param name="source">The Cell which is at the start of the path</param>
      /// <param name="destination">The Cell which is at the end of the path</param>
      /// <exception cref="ArgumentNullException">Thrown when source or destination is null</exception>
      /// <exception cref="PathNotFoundException">Thrown when there is not a path from the source to the destination</exception>
      /// <returns>Returns an ordered IEnumerable of Cells representing the shortest path from a specified source Cell to a destination Cell</returns>
      public Path ShortestPath( ICell source, ICell destination )
      {
         if ( source == null )
         {
            throw new ArgumentNullException( "source" );
         }

         if ( destination == null )
         {
            throw new ArgumentNullException( "destination" );
         }

         var cells = ShortestPathCells( source, destination ).ToList();
         if ( cells[0] == null )
         {
            throw new PathNotFoundException( string.Format( "Path from ({0}, {1}) to ({2}, {3}) not found", source.X, source.Y, destination.X, destination.Y ) );
         }
         return new Path( cells );
      }

      private IEnumerable<ICell> ShortestPathCells( ICell source, ICell destination )
      {
         var dsp = new DijkstraShortestPath( _graph, IndexFor( source ) );
         IEnumerable<DirectedEdge> path = dsp.PathTo( IndexFor( destination ) );
         if ( path == null )
         {
            yield return null;
         }
         else
         {
            foreach ( DirectedEdge edge in path )
            {
               yield return CellFor( edge.To );
            }
         }
      }

      private int IndexFor( ICell cell )
      {
         return ( cell.Y * _map.Width ) + cell.X;
      }

      private ICell CellFor( int index )
      {
         int x = index % _map.Width;
         int y = index / _map.Width;

         return _map.GetCell( x, y );
      }
   }
}