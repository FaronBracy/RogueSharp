using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class GoalMapTest
   {
      [TestMethod]
      public void FindPaths_SmallMapWithTwoGoalsOfEqualWeightAndDistance_Finds2PathsWith6Points()
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
         ReadOnlyCollection<Path> paths = goalMap.FindPaths( 3, 4 );

         string expectedGoalMapRepresentation = @"#    #    #    #    #    #    #    #
                                                  #    0    1    2    3    #    0    #
                                                  #    1    #    3    4    #    1    #
                                                  #    2    #    4    5    #    2    #
                                                  #    3    4    5    5    4    3    #
                                                  #    #    #    #    #    #    #    #";
         Assert.AreEqual( expectedGoalMapRepresentation.Replace( " ", string.Empty ), goalMap.ToString().Replace( " ", string.Empty ) );
         Assert.AreEqual( 2, paths.Count );
         Assert.AreEqual( 6, paths[0].Length );
         Assert.AreEqual( 6, paths[1].Length );
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
         goalMap.AddObstacles( obstacles );
         Path path = goalMap.FindPath( 3, 4 );

         Assert.AreEqual( 7, path.Length );
         Cell stepForward = path.StepForward();
         Assert.AreEqual( new Cell( 4, 4, true, true, false ), stepForward );
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
         goalMap.AddObstacles( obstacles );
         Path path = goalMap.FindPath( 3, 4 );

         Assert.AreEqual( 1, path.Length );
         Assert.AreEqual( new Cell( 3, 4, true, true, false ), path.Start );
         string expectedGoalMapRepresentation = @"#    #    #    #    #    #    #    #
                                                  #   48   48   48   48    #   48    #
                                                  #    #    #    #   48    #   48    #
                                                  #   48    #   48   48    #   48    #
                                                  #   48   48   48   48   48   48    #
                                                  #    #    #    #    #    #    #    #";
         Assert.AreEqual( expectedGoalMapRepresentation.Replace( " ", string.Empty ), goalMap.ToString().Replace( " ", string.Empty ) );
      }

      [TestMethod, Ignore]
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
         string expectedPath = "........s...........";

         Path path = goalMap.FindPathAvoidingGoals( 1, 2 );
         var actualPath = new StringBuilder();
         foreach ( Cell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
         Assert.AreEqual( new Cell( 2, 1, true, true, false ), path.StepForward() );
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

         Path path = goalMap.FindPath( 23, 7 );
         var actualPath = new StringBuilder();
         foreach ( Cell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }
   }
}
