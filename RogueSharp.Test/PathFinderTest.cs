using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class PathFinderTest
   {
      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_NullMap_ThrowsArgumentNullException()
      {
         var pathFinder = new PathFinder( null );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_NullMapWithDiagonalCostSet_ThrowsArgumentNullException()
      {
         var pathFinder = new PathFinder( null, 1.41 );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = null;
         ICell destination = map.GetCell( 5, 4 );

         Path shortestPath = pathFinder.ShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 4 );
         ICell destination = null;

         Path shortestPath = pathFinder.ShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 4 );
         ICell destination = map.GetCell( 5, 4 );

         Path shortestPath = pathFinder.ShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map, 1.41 );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 6, 4 );

         Path shortestPath = pathFinder.ShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 0, 1 );
         ICell destination = map.GetCell( 1, 1 );

         pathFinder.ShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 0, 1 );

         pathFinder.ShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 6, 1 );

         pathFinder.ShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 4 );
         ICell destination = map.GetCell( 5, 4 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map, 1.41 );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 6, 4 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = null;
         ICell destination = map.GetCell( 5, 4 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 4 );
         ICell destination = null;

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );
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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 0, 1 );
         ICell destination = map.GetCell( 1, 1 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 0, 1 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );

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
         PathFinder pathFinder = new PathFinder( map );
         ICell source = map.GetCell( 1, 1 );
         ICell destination = map.GetCell( 6, 1 );

         Path shortestPath = pathFinder.TryFindShortestPath( source, destination );

         Assert.AreEqual( null, shortestPath );
      }
   }
}
