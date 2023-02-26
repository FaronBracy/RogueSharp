using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;
using RogueSharp.Random;

namespace RogueSharp.Test
{
   [TestClass]
   public class DijkstraPathFinderTest
   {
      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_NullMap_ThrowsArgumentNullException()
      {
         var dijkstraPathFinder = new DijkstraPathFinder( null );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_NullMapWithDiagonalCostSet_ThrowsArgumentNullException()
      {
         var dijkstraPathFinder = new DijkstraPathFinder( null, 1.41 );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void ShortestPath_SourceIsNull_ThrowsArgumentNullException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = null;
         Cell destination = map.GetCell( 5, 4 );

         Path shortestPath = dijkstraPathFinder.ShortestPath( source, destination );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void ShortestPath_DestinationIsNull_ThrowsArgumentNullException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 4 );
         Cell destination = null;

         Path shortestPath = dijkstraPathFinder.ShortestPath( source, destination );
      }

      [TestMethod]
      public void ShortestPath_DestinationReachableFromSource_ExpectedPath()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 4 );
         Cell destination = map.GetCell( 5, 4 );

         Path shortestPath = dijkstraPathFinder.ShortestPath( source, destination );

         Assert.AreEqual( 5, shortestPath.Length );
         Assert.AreEqual( source, shortestPath.Start );
         Assert.AreEqual( destination, shortestPath.End );
         Assert.AreEqual( map.GetCell( 2, 4 ), shortestPath.StepForward() );
      }

      [TestMethod]
      public void ShortestPath_DestinationReachableFromSourceAndDiagonalMovementIsAllowed_ExpectedPath()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map, 1.41 );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 6, 4 );

         Path shortestPath = dijkstraPathFinder.ShortestPath( source, destination );

         Assert.AreEqual( 6, shortestPath.Length );
         Assert.AreEqual( source, shortestPath.Start );
         Assert.AreEqual( destination, shortestPath.End );
         Assert.AreEqual( map.GetCell( 2, 1 ), shortestPath.StepForward() );
         Assert.AreEqual( map.GetCell( 3, 2 ), shortestPath.StepForward() );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void ShortestPath_SourceCellNotWalkable_ThrowsPathNotFoundException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 0, 1 );
         Cell destination = map.GetCell( 1, 1 );

         dijkstraPathFinder.ShortestPath( source, destination );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void ShortestPath_DestinationNotWalkable_ThrowsPathNotFoundException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 0, 1 );

         dijkstraPathFinder.ShortestPath( source, destination );
      }

      [TestMethod]
      [ExpectedException( typeof( PathNotFoundException ) )]
      public void ShortestPath_DestinationUnreachable_ThrowsPathNotFoundException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #....#.#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 6, 1 );

         dijkstraPathFinder.ShortestPath( source, destination );
      }

      [TestMethod]
      public void TryFindShortestPath_DestinationReachableFromSource_ExpectedPath()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 4 );
         Cell destination = map.GetCell( 5, 4 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( 5, shortestPath.Length );
         Assert.AreEqual( source, shortestPath.Start );
         Assert.AreEqual( destination, shortestPath.End );
         Assert.AreEqual( map.GetCell( 2, 4 ), shortestPath.StepForward() );
      }

      [TestMethod]
      public void TryFindShortestPath_DestinationReachableFromSourceAndDiagonalMovementIsAllowed_ExpectedPath()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map, 1.41 );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 6, 4 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( 6, shortestPath.Length );
         Assert.AreEqual( source, shortestPath.Start );
         Assert.AreEqual( destination, shortestPath.End );
         Assert.AreEqual( map.GetCell( 2, 1 ), shortestPath.StepForward() );
         Assert.AreEqual( map.GetCell( 3, 2 ), shortestPath.StepForward() );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void TryFindShortestPath_SourceIsNull_ThrowsArgumentNullException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = null;
         Cell destination = map.GetCell( 5, 4 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void TryFindShortestPath_DestinationIsNull_ThrowsArgumentNullException()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 4 );
         Cell destination = null;

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );
      }

      [TestMethod]
      public void TryFindShortestPath_SourceCellNotWalkable_ReturnsNull()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 0, 1 );
         Cell destination = map.GetCell( 1, 1 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( null, shortestPath );
      }

      [TestMethod]
      public void TryFindShortestPath_DestinationNotWalkable_ReturnsNull()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #......#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 0, 1 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( null, shortestPath );
      }

      [TestMethod]
      public void TryFindShortestPath_DestinationUnreachable_ReturnsNull()
      {
         string mapRepresentation = @"########
                                      #....#.#
                                      #.#..#.#
                                      #.#..#.#
                                      #....#.#
                                      ########";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         Cell source = map.GetCell( 1, 1 );
         Cell destination = map.GetCell( 6, 1 );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( null, shortestPath );
      }

      [TestMethod]
      public void TryFindShortestPath_Large200x400MapFromX5Y1ToX29Y187_ReturnsExpectedPath()
      {
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( Algorithms.TestSetup.TestHelpers.Map200x400 );
         IMap map = Map.Create( mapCreationStrategy );
         Cell source = map.GetCell( 5, 1 );
         Cell destination = map.GetCell( 29, 187 );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );

         Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( 705, shortestPath.Length );
      }
      
      [TestMethod]
      public void TryFindShortestPath_Large200x400MapTrying12KnownPaths_ReturnsExpectedPaths()
      {
         KnownSeriesRandom randomX = new KnownSeriesRandom( 150, 137, 51, 31, 40, 135, 116, 148, 83, 94, 153, 30, 63, 80, 31, 107, 64, 95, 6, 145, 105, 66, 96, 37 );
         KnownSeriesRandom randomY = new KnownSeriesRandom( 255, 359, 175, 279, 169, 293, 335, 208, 235, 327, 67, 234, 56, 272, 241, 215, 230, 377, 194, 301, 161, 348, 89, 171 );
         int[] pathLengths = { 822, 229, 598, 730, 344, 507, 398, 655, 737, 799, 683, 350 };
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( Algorithms.TestSetup.TestHelpers.Map200x400 );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         for ( int i = 0; i < 12; i++ )
         {
            int x1 = randomX.Next( 199 );
            int y1 = randomY.Next( 399 );
            int x2 = randomX.Next( 199 );
            int y2 = randomY.Next( 399 );
            Cell source = map.GetCell( x1, y1 );
            Cell destination = map.GetCell( x2, y2 );

            Stopwatch timer = Stopwatch.StartNew();

            Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

            Console.WriteLine(
               $"Path from `{x1}:{y1}` to `{x2}:{y2}` was {shortestPath?.Steps?.Count()} long and took Elapsed Milliseconds: {timer.ElapsedMilliseconds}" );
            Assert.AreEqual( pathLengths[i % 12], shortestPath?.Steps?.Count() );
         }

         // Sample Output (Release Mode)
         //Path from `150:255` to `137:359` was 822 long and took Elapsed Milliseconds: 64 ( A* 9 )
         //Path from `51:175` to `31:279` was 229 long and took Elapsed Milliseconds:   47 ( A* 3 )
         //Path from `40:169` to `135:293` was 598 long and took Elapsed Milliseconds:  59 ( A* 1 )
         //Path from `116:335` to `148:208` was 730 long and took Elapsed Milliseconds: 82 ( A* 2 )
         //Path from `83:235` to `94:327` was 344 long and took Elapsed Milliseconds:   52 ( A* 1 )
         //Path from `153:67` to `30:234` was 507 long and took Elapsed Milliseconds:   73 ( A* 4 )
         //Path from `63:56` to `80:272` was 398 long and took Elapsed Milliseconds:    72 ( A* 1 )
         //Path from `31:241` to `107:215` was 655 long and took Elapsed Milliseconds:  76 ( A* 2 )
         //Path from `64:230` to `95:377` was 737 long and took Elapsed Milliseconds:   67 ( A* 4 )
         //Path from `6:194` to `145:301` was 799 long and took Elapsed Milliseconds:   69 ( A* 3 )
         //Path from `105:161` to `66:348` was 683 long and took Elapsed Milliseconds:  61 ( A* 4 )
         //Path from `96:89` to `37:171` was 350 long and took Elapsed Milliseconds:    57 ( A* 2 )
      }
      
      [TestMethod]
      public void TryFindShortestPath_Large200x400MapTrying12KnownPathsWithDiagonals_ReturnsExpectedPaths()
      {
         KnownSeriesRandom randomX = new KnownSeriesRandom( 150, 137, 51, 31, 40, 135, 116, 148, 83, 94, 153, 30, 63, 80, 31, 107, 64, 95, 6, 145, 105, 66, 96, 37 );
         KnownSeriesRandom randomY = new KnownSeriesRandom( 255, 359, 175, 279, 169, 293, 335, 208, 235, 327, 67, 234, 56, 272, 241, 215, 230, 377, 194, 301, 161, 348, 89, 171 );
         int[] pathLengths = { 749, 203, 557, 667, 328, 463, 371, 602, 692, 733, 626, 326 };
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( Algorithms.TestSetup.TestHelpers.Map200x400 );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map, 1 );
         for ( int i = 0; i < 12; i++ )
         {
            int x1 = randomX.Next( 199 );
            int y1 = randomY.Next( 399 );
            int x2 = randomX.Next( 199 );
            int y2 = randomY.Next( 399 );
            Cell source = map.GetCell( x1, y1 );
            Cell destination = map.GetCell( x2, y2 );

            Stopwatch timer = Stopwatch.StartNew();
            
            Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

            Console.WriteLine( $"Path from `{x1}:{y1}` to `{x2}:{y2}` was {shortestPath?.Steps?.Count()} long and took Elapsed Milliseconds: {timer.ElapsedMilliseconds}" );
            Assert.AreEqual( pathLengths[i % 12], shortestPath?.Steps?.Count() );
         }

         // Sample Output (Release Mode)
         //Path from `150:255` to `137:359` was 749 long and took Elapsed Milliseconds: 32
         //Path from `51:175` to `31:279` was 203 long and took Elapsed Milliseconds: 24
         //Path from `40:169` to `135:293` was 557 long and took Elapsed Milliseconds: 32
         //Path from `116:335` to `148:208` was 667 long and took Elapsed Milliseconds: 30
         //Path from `83:235` to `94:327` was 328 long and took Elapsed Milliseconds: 28
         //Path from `153:67` to `30:234` was 463 long and took Elapsed Milliseconds: 24
         //Path from `63:56` to `80:272` was 371 long and took Elapsed Milliseconds: 25
         //Path from `31:241` to `107:215` was 602 long and took Elapsed Milliseconds: 26
         //Path from `64:230` to `95:377` was 692 long and took Elapsed Milliseconds: 26
         //Path from `6:194` to `145:301` was 733 long and took Elapsed Milliseconds: 23
         //Path from `105:161` to `66:348` was 626 long and took Elapsed Milliseconds: 23
         //Path from `96:89` to `37:171` was 326 long and took Elapsed Milliseconds: 25
      }

      [TestMethod]
      public void TryFindShortestPath_Large200x400MapTrying24KnownPathsFrom1Source_ReturnsExpectedPaths()
      {
         KnownSeriesRandom randomX = new KnownSeriesRandom( 150, 137, 51, 31, 40, 135, 116, 148, 83, 94, 153, 30, 63, 80, 31, 107, 64, 95, 6, 145, 105, 66, 96, 37 );
         KnownSeriesRandom randomY = new KnownSeriesRandom( 255, 359, 175, 279, 169, 293, 335, 208, 235, 327, 67, 234, 56, 272, 241, 215, 230, 377, 194, 301, 161, 348, 89, 171 );
         int[] pathLengths = { 398, 489, 219, 423, 206, 421, 444, 351, 311, 414, 213, 323, 118, 345, 369, 315, 301, 465, 389, 439, 259, 453, 178, 201 };
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( Algorithms.TestSetup.TestHelpers.Map200x400 );
         IMap map = Map.Create( mapCreationStrategy );
         DijkstraPathFinder dijkstraPathFinder = new DijkstraPathFinder( map );
         for ( int i = 0; i < 24; i++ )
         {
            int x1 = 7;
            int y1 = 1;
            int x2 = randomX.Next( 199 );
            int y2 = randomY.Next( 399 );
            Cell source = map.GetCell( x1, y1 );
            Cell destination = map.GetCell( x2, y2 );

            Stopwatch timer = Stopwatch.StartNew();

            Path shortestPath = dijkstraPathFinder.TryFindShortestPath( source, destination );

            Console.WriteLine(
               $"Path from `{x1}:{y1}` to `{x2}:{y2}` was {shortestPath?.Steps?.Count()} long and took Elapsed Milliseconds: {timer.ElapsedMilliseconds}" );
            Assert.AreEqual( pathLengths[i % 24], shortestPath?.Steps?.Count() );
         }

         // Sample Output (Release Mode)
         //Path from `7:1` to `150:255` was 398 long and took Elapsed Milliseconds: 36
         //Path from `7:1` to `137:359` was 489 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `51:175` was 219 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `31:279` was 423 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `40:169` was 206 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `135:293` was 421 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `116:335` was 444 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `148:208` was 351 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `83:235` was 311 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `94:327` was 414 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `153:67` was 213 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `30:234` was 323 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `63:56` was 118 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `80:272` was 345 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `31:241` was 369 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `107:215` was 315 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `64:230` was 301 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `95:377` was 465 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `6:194` was 389 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `145:301` was 439 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `105:161` was 259 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `66:348` was 453 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `96:89` was 178 long and took Elapsed Milliseconds: 0
         //Path from `7:1` to `37:171` was 201 long and took Elapsed Milliseconds: 0
      }
   }
}
