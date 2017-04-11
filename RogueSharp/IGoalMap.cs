using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RogueSharp
{
   /// <summary>
   /// An interface for classes that assign weights to every cell on the Map and then use this for finding paths or building desire-driven AI
   /// </summary>
   /// <remarks>
   /// </remarks>
   /// <seealso href="http://www.roguebasin.com/index.php?title=The_Incredible_Power_of_Dijkstra_Maps">Inspired by the article "The Incredible Power of Dijkstra Maps on roguebasin</seealso>
   public interface IGoalMap
   {
      /// <summary>
      /// Add a Goal at the specified location with the specified weight
      /// </summary>
      /// <param name="x">X location of the Goal starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Goal starting with 0 as the top</param>
      /// <param name="weight">The priority of this goal with respect to other goals with lower numbers being a higher priority</param>
      void AddGoal( int x, int y, int weight );

      /// <summary>
      /// Remove a Goal at the specified location
      /// </summary>
      /// <param name="x">X location of the Goal starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Goal starting with 0 as the top</param>
      void RemoveGoal( int x, int y );

      /// <summary>
      /// Remove all goals from this GoalMap
      /// </summary>
      void ClearGoals();

      /// <summary>
      /// Add an Obstacle at the specified location. Any paths found must not go through Obstacles
      /// </summary>
      /// <param name="x">X location of the Obstacle starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Obstacle starting with 0 as the top</param>
      void AddObstacle( int x, int y );

      /// <summary>
      /// Add multiple obstacles from the specified enumeration of locations
      /// </summary>
      /// <param name="obstacles">An enumeration of points representing X, Y locations of Obstacles to avoid when pathfinding</param>
      void AddObstacles( IEnumerable<Point> obstacles );

      /// <summary>
      /// Remove an Obstacle at the specified location
      /// </summary>
      /// <param name="x">X location of the Obstacle starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Obstacle starting with 0 as the top</param>
      void RemoveObstacle( int x, int y );

      /// <summary>
      /// Remove all Obstacles from this GoalMap
      /// </summary>
      void ClearObstacles();

      /// <summary>
      /// Returns a ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority
      /// This method is useful when there are multiple paths that would all work and we want to have some additional logic to pick one of the best paths
      /// The FindPath( int x, int y ) method in the GoalMap class uses this method and then chooses the first path.
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority</returns>
      ReadOnlyCollection<Path> FindPaths( int x, int y );

      /// <summary>
      /// Returns a ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority
      /// This method is useful when there are multiple paths that would all work and we want to have some additional logic to pick one of the best paths
      /// The FindPath( int x, int y ) method in the GoalMap class uses this method and then chooses the first path.
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A ReadOnlyCollection of Paths representing all of the shortest paths from the specified location to the Goal or Goals determined to have the highest priority. Returns null if no path is found.</returns>
      ReadOnlyCollection<Path> TryFindPaths( int x, int y );

      /// <summary>
      /// Returns a shortest Path representing an ordered List of Points from the specified location to the Goal determined to have the highest priority
      /// Distance to the goals and the weight of the goals are both used in determining the priority
      /// The path must not pass through any obstacles specified in this GoalMap instance
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>An ordered List of Points representing a shortest path from the specified location to the Goal determined to have the highest priority</returns>
      Path FindPath( int x, int y );

      /// <summary>
      /// Returns a shortest Path representing an ordered List of Points from the specified location to the Goal determined to have the highest priority
      /// Distance to the goals and the weight of the goals are both used in determining the priority
      /// The path must not pass through any obstacles specified in this GoalMap instance
      /// null will be returned if a path cannot be found
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>An ordered List of Points representing a shortest path from the specified location to the Goal determined to have the highest priority. null is returned if a path cannot be found</returns>
      Path TryFindPath( int x, int y );

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
      Path FindPathAvoidingGoals( int x, int y );

      /// <summary>
      /// Returns a Path representing an ordered list of Points from the specified location away from Goals specified in this GoalMap instance
      /// Distance to the goals and the weight of the goals are both used in determining the priority of avoiding the Goals
      /// The path must not pass through any Obstacles specified in this GoalMap instance
      /// Returns null if a Path is not found
      /// </summary>
      /// <exmaple>
      /// In order to make the enemy AI try to flee from the player and his allies, Goals could be set on each object that the
      /// AI should stay away from. Then calling this method will find a path away from those Goals
      /// </exmaple>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A Path representing ordered List of Points from the specified location away from Goals and avoiding Obstacles. Returns null if a Path is not found</returns>
      Path TryFindPathAvoidingGoals( int x, int y );
   }
}