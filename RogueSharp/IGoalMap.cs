using System.Collections.Generic;

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
      /// Remove all goals from this GoalMap
      /// </summary>
      void ClearGoals();
      /// <summary>
      /// Returns a List of ordered Lists of Points representing all of the shortest paths from the specified location to all defined Goals
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <returns>A List of ordered Lists of Points representing all of the shortest paths from the specified location to all defined Goals</returns>
      List<List<Point>> FindAllPathsToAllGoals( int x, int y );
      /// <summary>
      /// Returns an ordered List of Points representing a shortest path from the specified location to the Goal determined to have the highest priority
      /// Distance to the goals and the weight of the goals are both used in determining the priority
      /// The path must avoid the specified obstacles
      /// </summary>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <param name="obstacles">An array of points that must be avoided while calculating the path</param>
      /// <returns>An ordered List of Points representing a shortest path from the specified location to the Goal determined to have the highest priority</returns>
      List<Point> FindPath( int x, int y, IEnumerable<Point> obstacles );
      /// <summary>
      /// Returns an ordered List of Points representing a path from the specified location away from Goals
      /// Distance to the goals and the weight of the goals are both used in determining the priority of avoiding the Goals
      /// The path must not pass through any of the specified obstacles
      /// </summary>
      /// <exmaple>
      /// In order to make the enemy AI try to flee from the player and his allies, Goals could be set on each object that the
      /// AI should stay away from. Then calling this method will find a path away from those Goals
      /// </exmaple>
      /// <param name="x">X location of the beginning of the path, starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the beginning of the path, starting with 0 as the top</param>
      /// <param name="obstacles">An array of points that must be avoided while calculating the path</param>
      /// <returns>An ordered List of Points representing a path from the specified location away from Goals</returns>
      List<Point> FindPathAvoidingGoals( int x, int y, IEnumerable<Point> obstacles );
   }
}