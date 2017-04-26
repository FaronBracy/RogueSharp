using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class GoalMapTest
   {
      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_NullMap_ThrowArgumentNullException()
      {
         IMap map = null;

         GoalMap goalMap = new GoalMap( map );
      }

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
         Assert.AreEqual( RemoveWhiteSpace( expectedGoalMapRepresentation ), RemoveWhiteSpace( goalMap.ToString() ) );
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
         ICell stepForward = path.StepForward();
         Assert.AreEqual( new Cell( 4, 4, true, true, false ), stepForward );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPath_SmallMapAfterAddingAndClearingGoals_ThrowsPathNotFoundException()
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

         goalMap.FindPath( 3, 4 );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPath_DestinationUnreachable_ThrowsPathNotFoundException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #....#.#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 6, 1, 0 );

         goalMap.FindPath( 1, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPath_SourceCellNotWalkable_ThrowsPathNotFoundException()
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

         goalMap.FindPath( 0, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPath_DestinationNotWalkableAndAdjacentToSource_ThrowsPathNotFoundException()
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
         goalMap.AddGoal( 0, 1, 0 );

         goalMap.FindPath( 1, 1 );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPath_DestinationNotWalkableAnd13StepsAway_ThrowsPathNotFoundException()
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
         goalMap.AddGoal( 7, 1, 0 );

         goalMap.FindPath( 1, 1 );
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
         foreach ( ICell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void FindPath_BigMapFromGameAllowingDiagonalMovement_ExpectedPath()
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
         GoalMap goalMap = new GoalMap( map, true );
         goalMap.AddGoal( 51, 31, 0 );
         goalMap.AddGoal( 51, 33, 0 );
         string expectedPath = "...s...s.....s........s.....s.......s...s......s.......s...s...s...........s...s..s.....s.s...s.s..s....s..........s.....";

         Path path = goalMap.FindPath( 23, 7 );
         var actualPath = new StringBuilder();
         foreach ( ICell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void FindPathAvoiding_BoxedInCornerWithObstacle_ExpectedPath()
      {
         string mapRepresentation = @"###############################################
                                      #..........#......................##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #.....................##......................#
                                      ###############################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 2, 2, 0 );
         goalMap.AddObstacle( 2, 1 );

         Path path = goalMap.FindPathAvoidingGoals( 1, 1 );

         Assert.AreEqual( 61, path.Length );
         Assert.AreEqual( map.GetCell( 1, 2 ), path.StepForward() );
         Assert.AreEqual( map.GetCell( 1, 1 ), path.Start );
         Assert.AreEqual( map.GetCell( 45, 1 ), path.End );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void FindPathAvoiding_NoPathExists_ThrowsPathNotFoundException()
      {
         string mapRepresentation = @"###############################################
                                      #..........#......................##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #.....................##......................#
                                      ###############################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 2, 2, 0 );
         goalMap.AddObstacle( 2, 1 );
         goalMap.AddObstacle( 1, 2 );

         Path path = goalMap.FindPathAvoidingGoals( 1, 1 );
      }

      [TestMethod]
      public void TryFindPaths_SmallMapWithTwoGoalsOfEqualWeightAndDistance_Finds2PathsWith6Points()
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
         ReadOnlyCollection<Path> paths = goalMap.TryFindPaths( 3, 4 );

         string expectedGoalMapRepresentation = @"#    #    #    #    #    #    #    #
                                                  #    0    1    2    3    #    0    #
                                                  #    1    #    3    4    #    1    #
                                                  #    2    #    4    5    #    2    #
                                                  #    3    4    5    5    4    3    #
                                                  #    #    #    #    #    #    #    #";
         Assert.AreEqual( RemoveWhiteSpace( expectedGoalMapRepresentation ), RemoveWhiteSpace( goalMap.ToString() ) );
         Assert.AreEqual( 2, paths.Count );
         Assert.AreEqual( 6, paths[0].Length );
         Assert.AreEqual( 6, paths[1].Length );
      }

      [TestMethod]
      public void TryFindPath_SmallMapWithTwoGoalsAndTwoObstacles_FindsBestPath()
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
         Path path = goalMap.TryFindPath( 3, 4 );

         Assert.AreEqual( 7, path.Length );
         ICell stepForward = path.StepForward();
         Assert.AreEqual( new Cell( 4, 4, true, true, false ), stepForward );
      }

      [TestMethod]
      public void TryFindPath_SmallMapAfterAddingAndClearingGoals_ReturnsNull()
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

         Path path = goalMap.TryFindPath( 3, 4 );

         Assert.AreEqual( null, path );
      }

      [TestMethod]
      public void TryFindPath_DestinationUnreachable_ReturnsNull()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #....#.#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 6, 1, 0 );

         var paths = goalMap.TryFindPath( 1, 1 );

         Assert.AreEqual( null, paths );
      }

      [TestMethod]
      public void TryFindPath_SourceCellNotWalkable_ReturnsNull()
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

         Path path = goalMap.TryFindPath( 0, 1 );

         Assert.AreEqual( null, path );
      }

      [TestMethod]
      public void TryFindPath_DestinationNotWalkableAndAdjacentToSource_ReturnsNull()
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
         goalMap.AddGoal( 0, 1, 0 );

         Path path = goalMap.TryFindPath( 1, 1 );

         Assert.AreEqual( null, path );
      }

      [TestMethod]
      public void TryFindPath_DestinationNotWalkableAnd13StepsAway_ReturnsNull()
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
         goalMap.AddGoal( 7, 1, 0 );

         Path path = goalMap.TryFindPath( 1, 1 );

         Assert.AreEqual( null, path );
      }

      [TestMethod]
      public void TryFindPath_BigMapFromGame_ExpectedPath()
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

         Path path = goalMap.TryFindPath( 23, 7 );
         var actualPath = new StringBuilder();
         foreach ( ICell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void TryFindPath_BigMapFromGameAllowingDiagonalMovement_ExpectedPath()
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
         GoalMap goalMap = new GoalMap( map, true );
         goalMap.AddGoal( 51, 31, 0 );
         goalMap.AddGoal( 51, 33, 0 );
         string expectedPath = "...s...s.....s........s.....s.......s...s......s.......s...s...s...........s...s..s.....s.s...s.s..s....s..........s.....";

         Path path = goalMap.TryFindPath( 23, 7 );
         var actualPath = new StringBuilder();
         foreach ( ICell cell in path.Steps )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void TryFindPathAvoiding_BoxedInCornerWithObstacle_ExpectedPath()
      {
         string mapRepresentation = @"###############################################
                                      #..........#......................##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #.....................##......................#
                                      ###############################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 2, 2, 0 );
         goalMap.AddObstacle( 2, 1 );

         Path path = goalMap.TryFindPathAvoidingGoals( 1, 1 );

         Assert.AreEqual( 61, path.Length );
         Assert.AreEqual( map.GetCell( 1, 2 ), path.StepForward() );
         Assert.AreEqual( map.GetCell( 1, 1 ), path.Start );
         Assert.AreEqual( map.GetCell( 45, 1 ), path.End );
      }

      [TestMethod]
      public void TryFindPathAvoiding_NoPathExists_ReturnsNull()
      {
         string mapRepresentation = @"###############################################
                                      #..........#......................##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #..........#..........##..........##..........#
                                      #.....................##......................#
                                      ###############################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         GoalMap goalMap = new GoalMap( map );
         goalMap.AddGoal( 2, 2, 0 );
         goalMap.AddObstacle( 2, 1 );
         goalMap.AddObstacle( 1, 1 );

         Path path = goalMap.TryFindPathAvoidingGoals( 1, 1 );

         Assert.AreEqual( null, path );
      }

      private static string RemoveWhiteSpace( string source )
      {
         return Regex.Replace( source, @"\s+", string.Empty );
      }
   }
}
