using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp
{
   public class GoalMap : IGoalMap
   {
      private const int Wall = int.MinValue;
      private readonly int[,] _cellWeights;
      private readonly List<Cell> _goals;
      private readonly IMap _map;
      private bool _isRecomputeNeeded;

      public GoalMap( IMap map )
      {
         _map = map;
         _cellWeights = new int[map.Width, map.Height];
         _goals = new List<Cell>();
         _isRecomputeNeeded = true;
      }

      public void AddGoal( int x, int y, int weight )
      {
         _goals.Add( new Cell
         {
            X = x,
            Y = y,
            Weight = weight
         } );
         _isRecomputeNeeded = true;
      }

      public void ClearGoals()
      {
         _goals.Clear();
         _isRecomputeNeeded = true;
      }

      public List<Point> FindPathAvoidingGoals( int x, int y, IEnumerable<Point> obstacles )
      {
         MultiplyAndRecomputeCellWeights( -1.2f );
         return FindPath( x, y, obstacles );
      }

      private void MultiplyAndRecomputeCellWeights( float amount )
      {
         ComputeCellWeightsIfNeeded();
 
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

                  List<Cell> neighbors = GetLowestWeightNeighbors( x, y );
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

      public List<Point> FindPath( int x, int y, IEnumerable<Point> obstacles )
      {
         ComputeCellWeightsIfNeeded();
         int bestPathIndex = 0;
         int bestPathLength = 0;
         List<List<Point>> paths = FindAllPathsToAllGoals( x, y );
         for ( int i = 0; i < paths.Count; i++ )
         {
            List<Point> currentPath = paths[i];
            for ( int j = 0; j < currentPath.Count; j++ )
            {
               if ( obstacles.Contains( currentPath[j] ) )
               {
                  if ( j > bestPathLength )
                  {
                     bestPathLength = j;
                     bestPathIndex = i;
                  }
                  break;
               }
               if ( j > bestPathLength )
               {
                  bestPathLength = j;
                  bestPathIndex = i;
               }
            }
         }
         return paths[bestPathIndex];
      }

      public List<List<Point>> FindAllPathsToAllGoals( int x, int y )
      {
         ComputeCellWeightsIfNeeded();
         var pathFinder = new GoalMapPathFinder( this );
         return pathFinder.FindPaths( x, y );
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
         foreach ( Cell goal in _goals )
         {
            _cellWeights[goal.X, goal.Y] = goal.Weight;
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

                  List<Cell> neighbors = GetLowestWeightNeighbors( x, y );
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

      private List<Cell> GetNeighbors( int x, int y )
      {
         var neighbors = new List<Cell>();
         if ( y + 1 < _map.Height && _cellWeights[x, y + 1] != Wall )
         {
            // Direction = Direction.Down
            neighbors.Add( new Cell
            {
               X = x,
               Y = y + 1,
               Weight = _cellWeights[x, y + 1]
            } );
         }
         if ( y > 0 && _cellWeights[x, y - 1] != Wall )
         {
            // Direction = Direction.Up
            neighbors.Add( new Cell
            {
               X = x,
               Y = y - 1,
               Weight = _cellWeights[x, y - 1]
            } );
         }
         if ( x + 1 < _map.Width && _cellWeights[x + 1, y] != Wall )
         {
            // Direction = Direction.Right
            neighbors.Add( new Cell
            {
               X = x + 1,
               Y = y,
               Weight = _cellWeights[x + 1, y]
            } );
         }
         if ( x > 0 && _cellWeights[x - 1, y] != Wall )
         {
            // Direction = Direction.Up
            neighbors.Add( new Cell
            {
               X = x - 1,
               Y = y,
               Weight = _cellWeights[x - 1, y]
            } );
         }
         return neighbors;
      }

      private List<Cell> GetLowestWeightNeighbors( int x, int y )
      {
         List<Cell> neighbors = GetNeighbors( x, y );
         if ( neighbors.Count <= 0 )
         {
            return null;
         }
         int? targetWeight = null;
         foreach ( Cell neighbor in neighbors )
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
         var lowestWeightNeighbors = new List<Cell>();
         foreach ( Cell neighbor in neighbors )
         {
            if ( targetWeight.HasValue && neighbor.Weight == targetWeight.Value )
            {
               lowestWeightNeighbors.Add( neighbor );
            }
         }
         return lowestWeightNeighbors;
      }

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

      private class Cell
      {
         public int X { get; set; }
         public int Y { get; set; }
         public int Weight { get; set; }
      }

      private class GoalMapPathFinder
      {
         private readonly GoalMap _goalMap;
         private readonly List<List<Point>> _paths;
         private readonly Stack<Point> _currentPath;
         private readonly HashSet<Point> _visited; 

         public GoalMapPathFinder( GoalMap goalMap )
         {
            _goalMap = goalMap;
            _paths = new List<List<Point>>();
            _currentPath = new Stack<Point>();
            _visited = new HashSet<Point>();
         }

         public List<List<Point>> FindPaths( int x, int y )
         {
            _paths.Clear();
            _currentPath.Clear();
            _visited.Clear();
            RecursivelyFindPaths( x, y );
            return _paths;
         }

         private void RecursivelyFindPaths( int x, int y )
         {
            var currentCell = new Point
            {
               X = x,
               Y = y
            };
            if ( _visited.Add( currentCell ) )
            {
               _currentPath.Push( currentCell );
               List<Cell> neighbors = _goalMap.GetLowestWeightNeighbors( x, y );
               if ( neighbors != null )
               {
                  foreach ( Cell neighbor in neighbors )
                  {
                     RecursivelyFindPaths( neighbor.X, neighbor.Y );
                  }
               }
               else
               {
                  // We reached our destination so remove that from the list of visited cells in case there is another path here.
                  _visited.Remove( currentCell );
                  _paths.Add( _currentPath.Reverse().ToList() );
               }
               _currentPath.Pop();
            }
         }
      }
   }
}