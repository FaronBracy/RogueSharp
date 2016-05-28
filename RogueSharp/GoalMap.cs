using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RogueSharp
{
   /// <summary>
   /// A class for assigning weights to every cell on the Map which can then be used for finding paths or building desire-driven AI
   /// </summary>
   /// <remarks>
   /// </remarks>
   /// <seealso href="http://www.roguebasin.com/index.php?title=The_Incredible_Power_of_Dijkstra_Maps">Inspired by the article "The Incredible Power of Dijkstra Maps on roguebasin</seealso>
   public class GoalMap : IGoalMap
   {
      private const int Wall = int.MinValue;
      private readonly int[,] _cellWeights;
      private readonly List<WeightedPoint> _goals;
      private readonly HashSet<Point> _obstacles;
      private readonly IMap _map;
      private bool _isRecomputeNeeded;

      /// <summary>
      /// Constructs a new instance of a GoalMap for the specified Map
      /// </summary>
      /// <param name="map">The Map that this GoalMap will be created for</param>
      /// <exception cref="ArgumentNullException">Thrown on null map</exception>
      public GoalMap( IMap map )
      {
         if ( map == null )
         {
            throw new ArgumentNullException( "map", "Map cannot be null" );
         }

         _map = map;
         _cellWeights = new int[map.Width, map.Height];
         _goals = new List<WeightedPoint>();
         _obstacles = new HashSet<Point>();
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Add a Goal at the specified location with the specified weight
      /// </summary>
      /// <param name="x">X location of the Goal starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Goal starting with 0 as the top</param>
      /// <param name="weight">The priority of this goal with respect to other goals with lower numbers being a higher priority</param>
      public void AddGoal( int x, int y, int weight )
      {
         _goals.Add( new WeightedPoint {
            X = x,
            Y = y,
            Weight = weight
         } );
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Remove a Goal at the specified location
      /// </summary>
      /// <param name="x">X location of the Goal starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Goal starting with 0 as the top</param>
      public void RemoveGoal( int x, int y )
      {
         _goals.RemoveAll( p => p.X == x && p.Y == y );
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Remove all goals from this GoalMap
      /// </summary>
      public void ClearGoals()
      {
         _goals.Clear();
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Add an Obstacle at the specified location. Any paths found must not go through Obstacles
      /// </summary>
      /// <param name="x">X location of the Obstacle starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Obstacle starting with 0 as the top</param>
      public void AddObstacle( int x, int y )
      {
         _obstacles.Add( new Point( x, y ) );
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Add multiple obstacles from the specified enumeration of locations
      /// </summary>
      /// <param name="obstacles">An enumeration of points representing X, Y locations of Obstacles to avoid when pathfinding</param>
      public void AddObstacles( IEnumerable<Point> obstacles )
      {
         if ( obstacles == null )
         {
            return;
         }
         foreach ( Point obstacle in obstacles )
         {
            _obstacles.Add( obstacle );
         }
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Remove an Obstacle at the specified location
      /// </summary>
      /// <param name="x">X location of the Obstacle starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Obstacle starting with 0 as the top</param>
      public void RemoveObstacle( int x, int y )
      {
         _obstacles.Remove( new Point( x, y ) );
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Remove all Obstacles from this GoalMap
      /// </summary>
      public void ClearObstacles()
      {
         _obstacles.Clear();
         _isRecomputeNeeded = true;
      }

      /// <summary>
      /// Returns a Path representing an ordered list of Points from the specified location away from Goals specified in this GoalMap instance
      /// Distance to the goals and the weight of the goals are both used in determining the priority of avoiding the Goals
      /// The path must not pass through any Obstacles specified in this GoalMap instance
      /// </summary>
      /// <exmaple>
      /// In order to make the enemy AI try to flee from the player and his allies, Goals could be set on each object that the
      /// AI should stay away from. Then calling this method will find a path away from those Goals
      /// </exmaple>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A Path representing ordered List of Points from the specified location away from Goals and avoiding Obstacles</returns>
      public Path FindPathAvoidingGoals( int x, int y )
      {
         MultiplyAndRecomputeCellWeights( -1.2f );
         return FindPath( x, y );
      }

      private void MultiplyAndRecomputeCellWeights( float amount )
      {
         ComputeCellWeightsIfNeeded();

         foreach ( var goal in _goals )
         {
            _cellWeights[goal.X, goal.Y] = Wall;
         }

         for ( int y = 0; y < _map.Height; y++ )
         {
            for ( int x = 0; x < _map.Width; x++ )
            {
               if ( _cellWeights[x, y] != Wall )
               {
                  _cellWeights[x, y] = (int) ( _cellWeights[x, y] * amount );
               }
            }
         }

         bool didCellWeightsChange = true;
         while ( didCellWeightsChange )
         {
            didCellWeightsChange = false;
            for ( int y = 0; y < _map.Height; y++ )
            {
               for ( int x = 0; x < _map.Width; x++ )
               {
                  if ( _cellWeights[x, y] == Wall )
                  {
                     continue;
                  }

                  List<WeightedPoint> neighbors = GetLowestWeightNeighbors( x, y );
                  if ( neighbors != null && neighbors.Count != 0 )
                  {
                     int lowestValueFloorNeighbor = neighbors[0].Weight;
                     if ( _cellWeights[x, y] > lowestValueFloorNeighbor + 1 )
                     {
                        _cellWeights[x, y] = lowestValueFloorNeighbor + 1;
                        didCellWeightsChange = true;
                     }
                  }
               }
            }
         }

         _isRecomputeNeeded = false;
      }

      /// <summary>
      /// Returns a shortest Path representing an ordered List of Points from the specified location to the Goal determined to have the highest priority
      /// Distance to the goals and the weight of the goals are both used in determining the priority
      /// The path must not pass through any obstacles specified in this GoalMap instance
      /// </summary>
      /// <exception cref="PathNotFoundException">Thrown when there is not a path from the Source x,y to any Goal or a Goal is not set</exception>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>An ordered List of Points representing a shortest path from the specified location to the Goal determined to have the highest priority</returns>
      public Path FindPath( int x, int y )
      {
         ComputeCellWeightsIfNeeded();
         ReadOnlyCollection<Path> paths = FindPaths( x, y );
         return paths.First();
      }

      /// <summary>
      /// Returns a ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority
      /// This method is useful when there are multiple paths that would all work and we want to have some additional logic to pick one of the best paths
      /// The FindPath( int x, int y ) method in the GoalMap class uses this method and then chooses the first path.
      /// </summary>
      /// <exception cref="PathNotFoundException">Thrown when there is not a path from the Source x,y to any Goal or a Goal is not set</exception>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority</returns>
      public ReadOnlyCollection<Path> FindPaths( int x, int y )
      {
         if ( _goals.Count < 1 )
         {
            throw new PathNotFoundException( "A goal must be set to find a path" );
         }

         if ( !_map.IsWalkable( x, y ) )
         {
            throw new PathNotFoundException( string.Format( "Source ({0}, {1}) must be walkable to find a path", x, y ) );
         }

         if ( !_goals.Any( g => _map.IsWalkable( g.X, g.Y ) ) )
         {
            throw new PathNotFoundException( "A goal must be walkable to find a path" );
         }  

         ComputeCellWeightsIfNeeded();
         var pathFinder = new GoalMapPathFinder( this );
         var paths = pathFinder.FindPaths( x, y );

         if ( paths.Count <= 1 && paths[0].Length <= 1 )
         {
            throw new PathNotFoundException( string.Format( "A path from Source ({0}, {1}) to any goal was not found", x, y ) );
         }

         return paths;
      }

      private void ComputeCellWeightsIfNeeded()
      {
         if ( _isRecomputeNeeded )
         {
            ComputeCellWeights();
         }
      }

      private void ComputeCellWeights()
      {
         _isRecomputeNeeded = false;
         int totalMapCells = _map.Width * _map.Height;
         for ( int y = 0; y < _map.Height; y++ )
         {
            for ( int x = 0; x < _map.Width; x++ )
            {
               if ( _map.IsWalkable( x, y ) )
               {
                  _cellWeights[x, y] = totalMapCells;
               }
               else
               {
                  _cellWeights[x, y] = Wall;
               }
            }
         }
         foreach ( WeightedPoint goal in _goals )
         {
            _cellWeights[goal.X, goal.Y] = goal.Weight;
         }
         foreach ( Point obstacle in _obstacles )
         {
            _cellWeights[obstacle.X, obstacle.Y] = Wall;
         }
         bool didCellWeightsChange = true;
         while ( didCellWeightsChange )
         {
            didCellWeightsChange = false;
            for ( int y = 0; y < _map.Height; y++ )
            {
               for ( int x = 0; x < _map.Width; x++ )
               {
                  if ( _cellWeights[x, y] == Wall )
                  {
                     continue;
                  }

                  List<WeightedPoint> neighbors = GetLowestWeightNeighbors( x, y );
                  if ( neighbors != null && neighbors.Count != 0 )
                  {
                     int lowestValueFloorNeighbor = neighbors[0].Weight;
                     if ( _cellWeights[x, y] > lowestValueFloorNeighbor + 1 )
                     {
                        _cellWeights[x, y] = lowestValueFloorNeighbor + 1;
                        didCellWeightsChange = true;
                     }
                  }
               }
            }
         }
      }

      private List<WeightedPoint> GetNeighbors( int x, int y )
      {
         var neighbors = new List<WeightedPoint>();
         if ( y + 1 < _map.Height && _cellWeights[x, y + 1] != Wall )
         {
            // Direction = Direction.Down
            neighbors.Add( new WeightedPoint {
               X = x,
               Y = y + 1,
               Weight = _cellWeights[x, y + 1]
            } );
         }
         if ( y > 0 && _cellWeights[x, y - 1] != Wall )
         {
            // Direction = Direction.Up
            neighbors.Add( new WeightedPoint {
               X = x,
               Y = y - 1,
               Weight = _cellWeights[x, y - 1]
            } );
         }
         if ( x + 1 < _map.Width && _cellWeights[x + 1, y] != Wall )
         {
            // Direction = Direction.Right
            neighbors.Add( new WeightedPoint {
               X = x + 1,
               Y = y,
               Weight = _cellWeights[x + 1, y]
            } );
         }
         if ( x > 0 && _cellWeights[x - 1, y] != Wall )
         {
            // Direction = Direction.Up
            neighbors.Add( new WeightedPoint {
               X = x - 1,
               Y = y,
               Weight = _cellWeights[x - 1, y]
            } );
         }
         return neighbors;
      }

      private List<WeightedPoint> GetLowestWeightNeighbors( int x, int y )
      {
         List<WeightedPoint> neighbors = GetNeighbors( x, y );
         if ( neighbors.Count <= 0 )
         {
            return null;
         }
         int? targetWeight = null;
         foreach ( WeightedPoint neighbor in neighbors )
         {
            if ( targetWeight.HasValue )
            {
               if ( neighbor.Weight < targetWeight )
               {
                  targetWeight = neighbor.Weight;
               }
            }
            else
            {
               targetWeight = neighbor.Weight;
            }
         }
         if ( targetWeight >= _cellWeights[x, y] )
         {
            // There are not any neighbors that have a smaller weight than the current cell
            return null;
         }
         var lowestWeightNeighbors = new List<WeightedPoint>();
         foreach ( WeightedPoint neighbor in neighbors )
         {
            if ( targetWeight.HasValue && neighbor.Weight == targetWeight.Value )
            {
               lowestWeightNeighbors.Add( neighbor );
            }
         }
         return lowestWeightNeighbors;
      }

      /// <summary>
      /// Returns a string representation of the current GoalMap
      /// </summary>
      /// <returns>A string representing the current GoalMap</returns>
      public override string ToString()
      {
         var mapRepresentation = new StringBuilder();
         for ( int y = 0; y < _map.Height; y++ )
         {
            for ( int x = 0; x < _map.Width; x++ )
            {
               mapRepresentation.AppendFormat( "{0,5}", _cellWeights[x, y] == Wall ? "#" : _cellWeights[x, y].ToString() );
            }
            mapRepresentation.Append( Environment.NewLine );
         }
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      private struct WeightedPoint : IEquatable<WeightedPoint>
      {
         public Point Point;
         public int Weight { get; set; }

         public int X
         {
            get { return Point.X; }
            set { Point.X = value; }
         }

         public int Y
         {
            get { return Point.Y; }
            set { Point.Y = value; }
         }

         public bool Equals( WeightedPoint other )
         {
            return X == other.X && Y == other.Y && Weight == other.Weight;
         }
      }

      private class GoalMapPathFinder
      {
         private readonly GoalMap _goalMap;
         private readonly List<Path> _paths;
         private readonly Stack<ICell> _currentPath;
         private readonly HashSet<ICell> _visited;

         public GoalMapPathFinder( GoalMap goalMap )
         {
            _goalMap = goalMap;
            _paths = new List<Path>();
            _currentPath = new Stack<ICell>();
            _visited = new HashSet<ICell>();
         }

         public ReadOnlyCollection<Path> FindPaths( int x, int y )
         {
            _paths.Clear();
            _currentPath.Clear();
            _visited.Clear();
            RecursivelyFindPaths( x, y );
            var paths = new List<Path>();
            foreach ( var path in _paths )
            {
               paths.Add( path );
            }
            return new ReadOnlyCollection<Path>( paths );
         }

         private void RecursivelyFindPaths( int x, int y )
         {
            var currentCell = _goalMap._map.GetCell( x, y );
            if ( _visited.Add( currentCell ) )
            {
               _currentPath.Push( currentCell );
               List<WeightedPoint> neighbors = _goalMap.GetLowestWeightNeighbors( x, y );
               if ( neighbors != null )
               {
                  foreach ( WeightedPoint neighbor in neighbors )
                  {
                     RecursivelyFindPaths( neighbor.X, neighbor.Y );
                  }
               }
               else
               {
                  // We reached our destination so remove that from the list of visited cells in case there is another path here.
                  _visited.Remove( currentCell );
                  _paths.Add( new Path( _currentPath.Reverse() ) );
               }
               _currentPath.Pop();
            }
         }
      }
   }
}