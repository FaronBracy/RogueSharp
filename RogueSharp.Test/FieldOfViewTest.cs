using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class FieldOfViewTest
   {
      [TestMethod]
      public void ComputeFov_SmallMap_ExpectedFov()
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
 
         IMapCreationStrategy<Map> mapCreationStrategy2 = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy2 );
         
         map.ComputeFov( 6, 1, 20, true );

         string expectedFov = @"###########################%%%%%%%%%
                                #..........................%%%%%%%%%
                                #..###.########%%.........%%%%%%%%%%
                                #%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%#.#%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%%.%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                                %%%%%###%%%%%%%%%%%%%%%%%%%%%%%%%%%%";

         Assert.AreEqual( expectedFov.Replace( " ", "" ), map.ToString( true ) );
      }
   }
}
