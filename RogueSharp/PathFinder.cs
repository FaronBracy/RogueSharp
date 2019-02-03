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
      private int? _sourceIndex = null;
      private DijkstraShortestPath _dijkstraShortestPath = null;

      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map that will not consider diagonal movements to be valid.
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public PathFinder( IMap map )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( ICell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = IndexFor( cell );
               foreach ( ICell neighbor in _map.GetBorderCellsInDiamond( cell.X, cell.Y, 1 ) )
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
      /// Constructs a new PathFinder instance for the specified Map that will consider diagonal movement by using the specified diagonalCost
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <param name="diagonalCost">
      /// The cost of diagonal movement compared to horizontal or vertical movement. 
      /// Use 1.0 if you want the same cost for all movements.
      /// On a standard cartesian map, it should be sqrt(2) (1.41)
      /// </param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public PathFinder( IMap map, double diagonalCost )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( ICell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = IndexFor( cell );
               foreach ( ICell neighbor in _map.GetBorderCellsInSquare( cell.X, cell.Y, 1 ) )
               {
                  if ( neighbor.IsWalkable )
                  {
                     int w = IndexFor( neighbor );
                     if ( neighbor.X != cell.X && neighbor.Y != cell.Y )
                     {
                        _graph.AddEdge( new DirectedEdge( v, w, diagonalCost ) );
                        _graph.AddEdge( new DirectedEdge( w, v, diagonalCost ) );
                     }
                     else
                     {
                        _graph.AddEdge( new DirectedEdge( v, w, 1.0 ) );
                        _graph.AddEdge( new DirectedEdge( w, v, 1.0 ) );
                     }
                  }
               }
            }
         }
      }

      /// <summary>
      /// Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell
      /// </summary>
      /// <param name="source">The Cell which is at the start of the path</param>
      /// <param name="destination">The Cell which is at the end of the path</param>
      /// <exception cref="ArgumentNullException">Thrown when source or destination is null</exception>
      /// <exception cref="PathNotFoundException">Thrown when there is not a path from the source to the destination</exception>
      /// <returns>Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell</returns>
      public Path ShortestPath( ICell source, ICell destination )
      {
         Path shortestPath = TryFindShortestPath( source, destination );

         if ( shortestPath == null )
         {
            throw new PathNotFoundException( $"Path from ({source.X}, {source.Y}) to ({destination.X}, {destination.Y}) not found" );
         }

         return shortestPath;
      }

      /// <summary>
      /// Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell
      /// </summary>
      /// <param name="source">The Cell which is at the start of the path</param>
      /// <param name="destination">The Cell which is at the end of the path</param>
      /// <exception cref="ArgumentNullException">Thrown when source or destination is null</exception>
      /// <returns>Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell. If no path is found null will be returned</returns>
      public Path TryFindShortestPath( ICell source, ICell destination )
      {
         if ( source == null )
         {
            throw new ArgumentNullException( nameof( source ) );
         }

         if ( destination == null )
         {
            throw new ArgumentNullException( nameof( destination ) );
         }

         var cells = ShortestPathCells( source, destination ).ToList();
         if ( cells[0] == null )
         {
            return null;
         }
         return new Path( cells );
      }

      private IEnumerable<ICell> ShortestPathCells( ICell source, ICell destination )
      {
         IEnumerable<DirectedEdge> path;
         int sourceIndex = IndexFor( source );
         if ( _sourceIndex.HasValue && _sourceIndex == sourceIndex && _dijkstraShortestPath != null )
         {
            path = _dijkstraShortestPath.PathTo( IndexFor( destination ) );
         }
         else
         {
            _sourceIndex = sourceIndex;
            _dijkstraShortestPath = new DijkstraShortestPath( _graph, sourceIndex );
            path = _dijkstraShortestPath.PathTo( IndexFor( destination ) );
         }

         if ( path == null )
         {
            yield return null;
         }
         else
         {
            yield return source;
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