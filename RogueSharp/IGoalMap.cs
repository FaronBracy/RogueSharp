using System.Collections.Generic;

namespace RogueSharp
{
   public interface IGoalMap
   {
      void AddGoal( int x, int y, int weight );
      void ClearGoals();
      List<List<Point>> FindAllPathsToAllGoals( int x, int y );
      List<Point> FindPath( int x, int y, IEnumerable<Point> obstacles );
      List<Point> FindPathAvoidingGoals( int x, int y, IEnumerable<Point> obstacles );
   }
}