using System;
using System.Collections.Generic;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The AStarShortestPath class represents a data type for finding the shortest path between two Cells on a Map
   /// </summary>
   public class AStarShortestPath<TCell> where TCell : ICell
   {
      private readonly double? _diagonalCost;

      /// <summary>
      /// Construct a new class for computing the shortest path between two Cells on a Map using the A* algorithm
      /// Using this constructor will not allow diagonal movement. Use the overloaded constructor with diagonalCost if diagonal movement is allowed.
      /// </summary>
      public AStarShortestPath()
      {
      }
      
      /// <summary>
      /// Construct a new class for computing the shortest path between two Cells on a Map using the A* algorithm
      /// </summary>
      /// <param name="diagonalCost">
      /// The cost of diagonal movement compared to horizontal or vertical movement.
      /// Use 1.0 if you want the same cost for all movements.
      /// On a standard cartesian map, it should be sqrt(2) (1.41)
      /// </param>
      public AStarShortestPath( double diagonalCost )
      {
         _diagonalCost = diagonalCost;
      }

      /// <summary>
      /// Returns an List of Cells representing a shortest path from the specified source to the specified destination
      /// </summary>
      /// <param name="source">The source Cell to find a shortest path from</param>
      /// <param name="destination">The destination Cell to find a shortest path to</param>
      /// <param name="map">The Map on which to find the shortest path between Cells</param>
      /// <returns>List of Cells representing a shortest path from the specified source to the specified destination. If no path is found null will be returned</returns>
      public List<TCell> FindPath( TCell source, TCell destination, IMap<TCell> map )
      {
         // OPEN = the set of nodes to be evaluated
         IndexMinPriorityQueue<PathNode> openNodes = new IndexMinPriorityQueue<PathNode>( map.Height * map.Width );
         // CLOSED = the set of nodes already evaluated
         bool[] isNodeClosed = new bool[map.Height * map.Width];

         // add the start node to OPEN
         openNodes.Insert( map.IndexFor( source ), new PathNode
         {
            DistanceFromStart = 0,
            HeuristicDistanceFromEnd = CalculateDistance( source, destination, _diagonalCost ),
            X = source.X,
            Y = source.Y,
            Parent = null
         } );

         PathNode currentNode;
         // loop
         while ( true )
         {
            // current = node in OPEN with the lowest f_cost
            if ( openNodes.Size < 1 )
            {
               return null;
            }
            currentNode = openNodes.MinKey();
            // remove current from OPEN
            int currentIndex = openNodes.DeleteMin();
            // add current to CLOSED
            isNodeClosed[currentIndex] = true;

            ICell currentCell = map.CellFor( currentIndex );
            // if current is the target node the path has been found
            if ( currentCell.Equals( destination ) )
            {
               break;
            }

            // foreach neighbor of the current node
            bool includeDiagonals = _diagonalCost.HasValue;
            foreach ( TCell neighbor in map.GetAdjacentCells( currentCell.X, currentCell.Y, includeDiagonals ) )
            {
               int neighborIndex = map.IndexFor( neighbor );
               // if neighbor is not walkable or neighbor is in CLOSED
               if ( neighbor.IsWalkable == false || isNodeClosed[neighborIndex] )
               {
                  // skip to the next neighbor
                  continue;
               }

               bool isNeighborInOpen = openNodes.Contains( neighborIndex );

               // if neighbor is in OPEN
               if ( isNeighborInOpen )
               {
                  // if new path to neighbor is shorter
                  PathNode neighborNode = openNodes.KeyAt( neighborIndex );
                  double newDistance = currentNode.DistanceFromStart + 1;
                  if ( newDistance < neighborNode.DistanceFromStart )
                  {
                     // update neighbor distance
                     neighborNode.DistanceFromStart = newDistance;
                     // set parent of neighbor to current
                     neighborNode.Parent = currentNode;
                  }
               }
               else // if neighbor is not in OPEN
               {
                  // set f_cost of neighbor
                  // set parent of neighbor to current
                  PathNode neighborNode = new PathNode
                  {
                     DistanceFromStart = currentNode.DistanceFromStart + 1,
                     HeuristicDistanceFromEnd = CalculateDistance( source, destination, _diagonalCost ),
                     X = neighbor.X,
                     Y = neighbor.Y,
                     Parent = currentNode
                  };
                  // add neighbor to OPEN
                  openNodes.Insert( neighborIndex, neighborNode );
               }
            }
         }

         List<TCell> path = new List<TCell>();
         path.Add( map.GetCell( currentNode.X, currentNode.Y ) );
         while ( currentNode.Parent != null )
         {
            currentNode = currentNode.Parent;
            path.Add( map.GetCell( currentNode.X, currentNode.Y ) );
         }

         path.Reverse();
         return path;
      }

      private static double CalculateDistance( TCell source, TCell destination )
      {
         int dx = Math.Abs( source.X - destination.X );
         int dy = Math.Abs( source.Y - destination.Y );

         return dx + dy;
      }

      private static double CalculateDistance( TCell source, TCell destination, double? diagonalCost )
      {
         if ( !diagonalCost.HasValue )
         {
            return CalculateDistance( source, destination );
         }
         int dx = Math.Abs( source.X - destination.X );
         int dy = Math.Abs( source.Y - destination.Y );

         int dMin = Math.Min( dx, dy );
         int dMax = Math.Max( dx, dy );

         return ( dMin * diagonalCost.Value ) + ( dMax - dMin );
      }

      private class PathNode : IComparable<PathNode>
      {
         public int X
         {
            get;
            set;
         }

         public int Y
         {
            get;
            set;
         }

         // G cost = distance from starting node
         public double DistanceFromStart
         {
            get;
            set;
         }

         // H cost = (heuristic) distance from end node
         public double HeuristicDistanceFromEnd
         {
            get;
            set;
         }

         public PathNode Parent
         {
            get;
            set;
         }

         // F cost = G cost + H cost
         public double Cost => DistanceFromStart + HeuristicDistanceFromEnd;

         public int CompareTo( PathNode other )
         {
            if ( ReferenceEquals( this, other ) )
            {
               return 0;
            }

            if ( ReferenceEquals( null, other ) )
            {
               return 1;
            }

            return Cost.CompareTo( other.Cost );
         }
      }
   }
}
