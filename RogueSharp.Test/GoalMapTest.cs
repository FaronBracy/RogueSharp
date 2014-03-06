using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RogueSharp.Test
{
   [TestClass]
   public class GoalMapTest
   {
      [TestMethod]
      public void FindAllPathsToAllGoals_SmallMapWithTwoGoals_Finds2PathsWith6Points()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 1, 1, 0 );
         goalMap.AddGoal( 6, 1, 0 );
         List<List<Point>> paths = goalMap.FindAllPathsToAllGoals( 3, 4 );

         string expectedGoalMapRepresentation = @"#    #    #    #    #    #    #    #
                                                  #    0    1    2    3    #    0    #
                                                  #    1    #    3    4    #    1    #
                                                  #    2    #    4    5    #    2    #
                                                  #    3    4    5    5    4    3    #
                                                  #    #    #    #    #    #    #    #";
         Assert.AreEqual( expectedGoalMapRepresentation.Replace( " ", string.Empty ), goalMap.ToString().Replace( " ", string.Empty ) );
         Assert.AreEqual( 2, paths.Count );
         Assert.AreEqual( 6, paths[0].Count );
         Assert.AreEqual( 6, paths[1].Count );
      }

      [TestMethod]
      public void FindPath_SmallMapWithTwoGoalsAndTwoObstacles_FindsBestPath()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 1, 1, 0 );
         goalMap.AddGoal( 6, 1, 0 );
         List<Point> obstacles = new List<Point> { new Point( 1, 2 ), new Point( 3, 2 ) };
         List<Point> path = goalMap.FindPath( 3, 4, obstacles );

         Assert.AreEqual( 6, path.Count );
         Assert.AreEqual( new Point( 2, 4 ), path[1] );
      }

      [TestMethod]
      public void FindPath_SmallMapAfterAddingAndClearingGoals_PathHasOnePointAndAllCellsAreMax()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 1, 1, 0 );
         goalMap.AddGoal( 6, 1, 0 );
         goalMap.ClearGoals();
         List<Point> obstacles = new List<Point> { new Point( 1, 2 ), new Point( 3, 2 ) };
         List<Point> path = goalMap.FindPath( 3, 4, obstacles );

         Assert.AreEqual( 1, path.Count );
         Assert.AreEqual( new Point( 3, 4 ), path[0] );
         string expectedGoalMapRepresentation = @"#    #    #    #    #    #    #    #
                                                  #   48   48   48   48    #   48    #
                                                  #   48    #   48   48    #   48    #
                                                  #   48    #   48   48    #   48    #
                                                  #   48   48   48   48   48   48    #
                                                  #    #    #    #    #    #    #    #";
         Assert.AreEqual( expectedGoalMapRepresentation.Replace( " ", string.Empty ), goalMap.ToString().Replace( " ", string.Empty ) );
      }

      [TestMethod]
      public void FindPathAvoiding_BoxedInCornerWithObstacle_ExpectedPath()
      {
         string mapRepresentation = @"###########
                                      #.....#...#
                                      #.....#.#.#
                                      #.....#.#.#
                                      #.....#.#.#
                                      #.....s.#.#
                                      ###########";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 2, 2, 0 );
         Point obstacle = new Point( 2, 2 );
         string expectedPath = "........s...........";

         List<Point> path = goalMap.FindPathAvoidingGoals( 1, 2, new List<Point> { obstacle } );
         var actualPath = new StringBuilder();
         foreach( Point p in path )
         {
            actualPath.Append( map.GetCell( p.X, p.Y ).ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void FindPath_BigMapFromGame_ExpectedPath()
      {
         string mapRepresentation = @"###########################################################
                                      #.....#...............#...#.....#.....#.s.....s...........#
                                      #.....#.#.###########.#...#.#.#.#.....#.#.....#.#.#######.#
                                      #.....#.#.#.....s...#.#...s.#.#.#.....#.#.....#.#.#.....#.#
                                      #.....#.#.#.....#...#####s#s#######s#####.....#.#.#.....#.#
                                      #.....s.#.s.....#...s.....#.......#.#...#.....#.#.#.....s.#
                                      #######s#.#######...#.....#.......#.#...#.....#.#.#######.#
                                      #.......#.#.....#...#.....#.......#.s...#.....#.#.......#.#
                                      #.......#.s.....#####s#.###.......#.#...#####s#.#.####s####
                                      #.......#.#.....s.#...#...#.......s.#...#.s...#.#.#.......#
                                      #.......###########...#.###############s###...#.#.#.......#
                                      #.......s...#.#...#...#.#.......#.#.......#...#.#.#.......#
                                      #######s#...#.#...##s##.s.......#.#.......#...#######s#####
                                      #.#.....#...#.#...#...#.#.......#.#.......s...#.....#...#.#
                                      #.#.....#...#.#...#####.#.......s.##s######...#.....#...#.#
                                      #.#.....#...#.#...#...#.#.......#.#.....#.#...s.....#...#.#
                                      #.#.....#...s.###s#...#.####s######.....#s#####s#####...s.#
                                      #.#.....#...#.....s...#...#...#...#.....s.....#.....#...#.#
                                      #.#.....#####.#.#.#...#.#.#...s...#s#####.....###s###...#.#
                                      #.#.....s...#.#.#.#...#.#.#...#...#.....#.....#.....#...#.#
                                      #.##s##############...###.######s##############.....#######
                                      #.........#.......#...#.......#...#.......#...#.....s.#...#
                                      #.#.#.#.#.#.......#####.......#####.......#.###########...#
                                      #.#.#.#.#.s.......#...#.......#...#.......s.#.....#...#...#
                                      ###s###############...#.......#...####s####.s.....s...#...#
                                      #...#.......#.....s...#.......s...#.......#.#.....#...#...#
                                      #...#.......#######s###.......##########s##.###s###...s...#
                                      #...#.......s...s...#.#.......#.s...#.....#.#...#.#...#...#
                                      #...#.......#...#...s.###########...#.....#.#...#.#########
                                      #...#.......#...#...#...........#...#.....#.#...#.........#
                                      #...#.......######s##.###s#####.#...#.....#.#...#.#######.#
                                      #...s.......#.......#.#.......#.s...#.....#.#...s.#.....#.#
                                      ########s####.#.#.#.#.#.......#.#...###s###.#######.....s.#
                                      #...........#.#.#.#.#.#.......#.#...s.....#.......#.....#.#
                                      ###########################################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 51, 31, 0 );
         goalMap.AddGoal( 51, 33, 0 );
         string expectedPath = ".....s.....s.......s...........s.....s..........s....s.......s...........s...s....s..............s.....s...s......s...s....s..s....s.....s..............s......";

         List<Point> path = goalMap.FindPath( 23, 7, new List<Point>() );
         var actualPath = new StringBuilder();
         foreach( Point p in path )
         {
            actualPath.Append( map.GetCell( p.X, p.Y ).ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }
   }
}
