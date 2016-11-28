using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class PathFinderTest
   {
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
   }
}
