using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class PathFinderTest
   {
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
