using System.Collections.Generic;

namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public interface IGoalMap
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="weight"></param>
      void AddGoal( int x, int y, int weight );
      /// <summary>
      /// 
      /// </summary>
      void ClearGoals();
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      List<List<Point>> FindAllPathsToAllGoals( int x, int y );
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="obstacles"></param>
      /// <returns></returns>
      List<Point> FindPath( int x, int y, IEnumerable<Point> obstacles );
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="obstacles"></param>
      /// <returns></returns>
      List<Point> FindPathAvoidingGoals( int x, int y, IEnumerable<Point> obstacles );
   }
}