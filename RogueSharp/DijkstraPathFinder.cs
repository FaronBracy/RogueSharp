using System;
using System.Collections.Generic;
using System.Linq;
using RogueSharp.Algorithms;

namespace RogueSharp
{
   /// <summary>
   /// A class which can be used to find shortest path from a source to a destination in a Map.
   /// This is only more efficient than the standard pathfinder when attempting to find multiple paths from a single source node. 
   /// </summary>
   public class DijkstraPathFinder : DijkstraPathFinder<Cell>
   {
      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map that will not consider diagonal movements to be valid.
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public DijkstraPathFinder( IMap<Cell> map )
         : base( map )
      {
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
      public DijkstraPathFinder( IMap<Cell> map, double diagonalCost )
         : base( map, diagonalCost )
      {
      }
   }

   /// <summary>
   /// A class which can be used to find shortest path from a source to a destination in a Map
   /// This is only more efficient than the standard pathfinder when attempting to find multiple paths from a single source node. 
   /// </summary>
   public class DijkstraPathFinder<TCell> where TCell : ICell
   {
      private readonly EdgeWeightedDigraph _graph;
      private readonly IMap<TCell> _map;
      private int? _sourceIndex = null;
      private DijkstraShortestPath _dijkstraShortestPath = null;

      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map that will not consider diagonal movements to be valid.
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public DijkstraPathFinder( IMap<TCell> map )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( TCell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = _map.IndexFor( cell );
               foreach ( TCell neighbor in _map.GetAdjacentCells( cell.X, cell.Y ) )
               {
                  if ( neighbor.IsWalkable )
                  {
                     int w = _map.IndexFor( neighbor );
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
      public DijkstraPathFinder( IMap<TCell> map, double diagonalCost )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( TCell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = _map.IndexFor( cell );
               foreach ( TCell neighbor in _map.GetAdjacentCells( cell.X, cell.Y, true ) )
               {
                  if ( neighbor.IsWalkable )
                  {
                     int w = _map.IndexFor( neighbor );
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
      public Path ShortestPath( TCell source, TCell destination )
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
      public Path TryFindShortestPath( TCell source, TCell destination )
      {
         if ( source == null )
         {
            throw new ArgumentNullException( nameof( source ) );
         }

         if ( destination == null )
         {
            throw new ArgumentNullException( nameof( destination ) );
         }

         List<ICell> cells = ShortestPathCells( source, destination ).ToList();
         if ( cells[0] == null )
         {
            return null;
         }
         return new Path( cells );
      }

      private IEnumerable<ICell> ShortestPathCells( TCell source, TCell destination )
      {
         IEnumerable<DirectedEdge> path;
         int sourceIndex = _map.IndexFor( source );
         if ( _sourceIndex.HasValue && _sourceIndex == sourceIndex && _dijkstraShortestPath != null )
         {
            path = _dijkstraShortestPath.PathTo( _map.IndexFor( destination ) );
         }
         else
         {
            _sourceIndex = sourceIndex;
            _dijkstraShortestPath = new DijkstraShortestPath( _graph, sourceIndex );
            path = _dijkstraShortestPath.PathTo( _map.IndexFor( destination ) );
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
               yield return _map.CellFor( edge.To );
            }
         }
      }
   }
}