using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class FieldOfViewTest
   {
      [TestMethod]
      public void ComputeFov_SmallMap_ExpectedCollectionOfVisibleCells()
      {
         string mapRepresentation = @"####################################
                                      #..................................#
                                      #..###.########....................#
                                      #....#.#......#....................#
                                      #....#.#......#....................#
                                      #.............#....................#
                                      #....#.#......######################
                                      #....#.#...........................#
                                      #....#.#...........................#
                                      #..................................#
                                      ####################################";

         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         var fieldOfView = new FieldOfView( map );
         var visibleCells = fieldOfView.ComputeFov( 6, 1, 20, true );

         // The field of view should be calculated as follows
         //
         //    ###########################%%%%%%%%%
         //    #..........................%%%%%%%%%
         //    #..###.########%%.........%%%%%%%%%%
         //    #%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%###%%%%%%%%%%%%%%%%%%%%%%%%%%%%

         int floorCells = 0;
         int wallCells = 0;
         foreach ( ICell cell in visibleCells )
         {
            if ( cell.IsWalkable )
            {
               floorCells++;
            }
            else
            {
               wallCells++;
            }
         }

         Assert.AreEqual( 99, visibleCells.Count );
         Assert.AreEqual( 45, floorCells );
         Assert.AreEqual( 54, wallCells );
      }

      [TestMethod]
      public void AppendFov_SmallMap_ExpectedCollectionOfVisibleCells()
      {
         string mapRepresentation = @"####################################
                                      #..................................#
                                      #..###.########....................#
                                      #....#.#......#....................#
                                      #....#.#......#....................#
                                      #.............#....................#
                                      #....#.#......######################
                                      #....#.#...........................#
                                      #....#.#...........................#
                                      #..................................#
                                      ####################################";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         var fieldOfView = new FieldOfView( map );
         fieldOfView.ComputeFov( 6, 1, 20, true );
         var visibleCells = fieldOfView.AppendFov( 15, 1, 5, true );

         // The field of view should be calculated as follows
         //
         //  ###########################%%%%%%%%%
         //  #..........................%%%%%%%%%
         //  #..###.########...........%%%%%%%%%%
         //  #%%%%#.#%%%%%%#....%%%%%%%%%%%%%%%%%
         //  %%%%%#.#%%%%%%#...%%%%%%%%%%%%%%%%%%
         //  %%%%%%.%%%%%%%#..%%%%%%%%%%%%%%%%%%%
         //  %%%%%#.#%%%%%%####%%%%%%%%%%%%%%%%%%
         //  %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //  %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //  %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //  %%%%%###%%%%%%%%%%%%%%%%%%%%%%%%%%%%

         int floorCells = 0;
         int wallCells = 0;
         foreach ( ICell cell in visibleCells )
         {
            if ( cell.IsWalkable )
            {
               floorCells++;
            }
            else
            {
               wallCells++;
            }
         }

         Assert.AreEqual( 117, visibleCells.Count );
         Assert.AreEqual( 56, floorCells );
         Assert.AreEqual( 61, wallCells );
      }

      [TestMethod]
      public void IsInFov_SampleOfCellsFromSmallMap_ExpectedCellsInFov()
      {
         string mapRepresentation = @"####################################
                                      #..................................#
                                      #..###.########....................#
                                      #....#.#......#....................#
                                      #....#.#......#....................#
                                      #.............#....................#
                                      #....#.#......######################
                                      #....#.#...........................#
                                      #....#.#...........................#
                                      #..................................#
                                      ####################################";
         IMapCreationStrategy<Map> mapCreationStrategy = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy );

         var fieldOfView = new FieldOfView( map );
         fieldOfView.ComputeFov( 6, 1, 20, true );

         // The field of view should be calculated as follows
         //
         //    ###########################%%%%%%%%%
         //    #..........................%%%%%%%%%
         //    #..###.########%%.........%%%%%%%%%%
         //    #%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
         //    %%%%%###%%%%%%%%%%%%%%%%%%%%%%%%%%%%

         Assert.IsTrue( fieldOfView.IsInFov( 2, 2 ) );
         Assert.IsFalse( fieldOfView.IsInFov( 2, 3 ) );
      }
   }
}
