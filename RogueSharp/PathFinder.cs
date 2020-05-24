using System;
using System.Collections.Generic;
using RogueSharp.Algorithms;

namespace RogueSharp
{
   /// <summary>
   /// A class which can be used to find shortest path from a source to a destination in a Map
   /// </summary>
   public class PathFinder : PathFinder<Cell>
   {
      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map that will not consider diagonal movements to be valid.
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public PathFinder( IMap<Cell> map )
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
      public PathFinder( IMap<Cell> map, double diagonalCost )
         : base( map, diagonalCost )
      {
      }
   }

   /// <summary>
   /// A class which can be used to find shortest path from a source to a destination in a Map
   /// </summary>
   public class PathFinder<TCell> where TCell : ICell
   {
      private readonly double? _diagonalCost;
      private readonly IMap<TCell> _map;

      /// <summary>
      /// Constructs a new PathFinder instance for the specified Map that will not consider diagonal movements to be valid.
      /// </summary>
      /// <param name="map">The Map that this PathFinder instance will run shortest path algorithms on</param>
      /// <exception cref="ArgumentNullException">Thrown when a null map parameter is passed in</exception>
      public PathFinder( IMap<TCell> map )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
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
      public PathFinder( IMap<TCell> map, double diagonalCost )
      {
         _map = map ?? throw new ArgumentNullException( nameof( map ), "Map cannot be null" );
         _diagonalCost = diagonalCost;
      }

      /// <summary>
      /// Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell using the AStar search algorithm.
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
      /// Returns a shortest Path containing a list of Cells from a specified source Cell to a destination Cell using the AStar search algorithm.
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

         if ( !source.IsWalkable )
         {
            return null;
         }

         if ( !destination.IsWalkable )
         {
            return null;
         }

         AStarShortestPath<TCell> aStarShortestPath;
         if ( _diagonalCost.HasValue )
         {
            aStarShortestPath = new AStarShortestPath<TCell>( (int) _diagonalCost.Value );
         }
         else
         {
            aStarShortestPath = new AStarShortestPath<TCell>();
         }
         List<TCell> cells = aStarShortestPath.FindPath( (TCell) source, (TCell) destination, _map );
         if ( cells == null )
         {
            return null;
         }
         return new Path( (IEnumerable<ICell>) cells );
      }
   }
}