using System;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;
using RogueSharp.Random;

namespace RogueSharp.PerformanceTest
{
   [TestClass]
   public class LibtcodTest
   {
      [TestMethod]
      public void Constructor_MapCreatedWithWidth40Height20_ExpectedWidthAndHeight()
      {
         int expectedWidth = 40;
         int expectedHeight = 20;

         LibtcodMap map = new LibtcodMap( expectedWidth, expectedHeight );

         Assert.AreEqual( expectedWidth, map.Width );
         Assert.AreEqual( expectedHeight, map.Height );
      }

      [TestMethod]
      public void Clear_IsTransparentTrueIsWalkableFalse_AllCellsHaveExpectedValues()
      {
         LibtcodMap map = new LibtcodMap( 10, 10 );

         map.Clear( true, false );
         foreach ( Cell cell in map.GetAllCells() )
         {
            Assert.IsTrue( map.IsTransparent( cell.X, cell.Y ) );
            Assert.IsFalse( map.IsWalkable( cell.X, cell.Y ) );
         }
      }

      [TestMethod]
      public void Create_MapCreatedWithWidth40Height20BorderOnlyStrategy_ExpectedMap()
      {
         int expectedWidth = 40;
         int expectedHeight = 20;
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new BorderOnlyMapCreationStrategy<LibtcodMap>( expectedWidth, expectedHeight );

         IMap map = LibtcodMap.Create( mapCreationStrategy );
         Trace.Write( map );

         Assert.AreEqual( expectedWidth, map.Width );
         Assert.AreEqual( expectedHeight, map.Height );

         for ( int x = 0; x < map.Width; x++ )
         {
            Assert.IsFalse( map.IsTransparent( x, 0 ) );
            Assert.IsFalse( map.IsWalkable( x, 0 ) );
            Assert.IsFalse( map.IsTransparent( x, map.Height - 1 ) );
            Assert.IsFalse( map.IsWalkable( x, map.Height - 1 ) );
         }

         for ( int y = 0; y < map.Height; y++ )
         {
            Assert.IsFalse( map.IsTransparent( 0, y ) );
            Assert.IsFalse( map.IsWalkable( 0, y ) );
            Assert.IsFalse( map.IsTransparent( map.Width - 1, y ) );
            Assert.IsFalse( map.IsWalkable( map.Width - 1, y ) );
         }
      }

      [TestMethod]
      public void Create_MapCreatedWithRandomRoomsMapCreationStrategy_ExpectedMap()
      {
         int expectedWidth = 17;
         int expectedHeight = 10;
         IRandom random = new DotNetRandom( 13 );
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new RandomRoomsMapCreationStrategy<LibtcodMap>( expectedWidth, expectedHeight, 30, 5, 3, random );
         string expectedMapRepresentation = @"#################
                                              #################
                                              ##...#######...##
                                              ##.............##
                                              ###.###....#...##
                                              ###...##.#####.##
                                              ###...##...###..#
                                              ####............#
                                              ##############..#
                                              #################";

         IMap actualMap = LibtcodMap.Create( mapCreationStrategy );
         Trace.Write( actualMap );

         Assert.AreEqual( expectedWidth, actualMap.Width );
         Assert.AreEqual( expectedHeight, actualMap.Height );
         Assert.AreEqual( expectedMapRepresentation.Replace( " ", string.Empty ), actualMap.ToString() );
      }

      [TestMethod]
      public void Create_StringDeserializeMapCreationStrategy_ExpectedMap()
      {
         string expectedMapRepresentation = @"####
                                              #..#
                                              #so#
                                              ####";

         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( expectedMapRepresentation );
         IMap actualMap = LibtcodMap.Create( mapCreationStrategy );

         Assert.AreEqual( expectedMapRepresentation.Replace( " ", string.Empty ), actualMap.ToString() );
      }

      [TestMethod]
      public void Clone_SmallMap_MapAndCloneHaveDifferentReferencesButSameValues()
      {
         string mapRepresentation = @"####
                                      #..#
                                      #so#
                                      ####";
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap originalMap = LibtcodMap.Create( mapCreationStrategy );

         IMap clonedMap = originalMap.Clone();

         Assert.AreNotEqual( originalMap, clonedMap );
         Assert.AreEqual( originalMap.ToString(), clonedMap.ToString() );
      }

      [TestMethod]
      public void Copy_SourceMapWiderThanDestinationMap_ThrowsArgumentException()
      {
         LibtcodMap sourceMap = new LibtcodMap( 10, 10 );
         LibtcodMap destinationMap = new LibtcodMap( 5, 10 );

         try
         {
            destinationMap.Copy( sourceMap, 0, 0 );
         }
         catch ( Exception ex )
         {
            Assert.IsInstanceOfType( ex, typeof( ArgumentException ) );
            return;
         }

         Assert.Fail( "Exception should have been thrown" );
      }

      [TestMethod]
      public void Copy_SourceMapPlusTopLargerThanDestinationMap_ThrowsArgumentException()
      {
         IMap sourceMap = new LibtcodMap( 10, 10 );
         IMap destinationMap = new LibtcodMap( 10, 10 );

         try
         {
            sourceMap.Copy( destinationMap, 0, 1 );
         }
         catch ( Exception ex )
         {
            Assert.IsInstanceOfType( ex, typeof( ArgumentException ) );
            return;
         }

         Assert.Fail( "Exception should have been thrown" );
      }

      [TestMethod]
      public void Copy_SmallSourceMapIntoSpecificPostionOfDestinationMap_ExpectedDestinationMap()
      {
         string sourceMapRepresentation = @"..";
         IMapCreationStrategy<LibtcodMap> sourceMapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( sourceMapRepresentation );
         IMap sourceMap = LibtcodMap.Create( sourceMapCreationStrategy );
         string destinationMapRepresentation = @"####
                                                 #..#
                                                 #so#
                                                 ####";
         IMapCreationStrategy<LibtcodMap> destinationMapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( destinationMapRepresentation );
         IMap destinationMap = LibtcodMap.Create( destinationMapCreationStrategy );
         string expectedRepresentationAfterCopy = @"####
                                                    #..#
                                                    #..#
                                                    ####";

         destinationMap.Copy( sourceMap, 1, 2 );

         Assert.AreEqual( expectedRepresentationAfterCopy.Replace( " ", string.Empty ), destinationMap.ToString() );
      }

      [TestMethod]
      public void GetCellsAlongLine_TopRightToBottomLeft_ExpectedCells()
      {
         string mapRepresentation = @"####
                                      #..#
                                      #so#
                                      ####";
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap map = LibtcodMap.Create( mapCreationStrategy );
         string expectedPath = "#.s#";

         StringBuilder actualPath = new StringBuilder();
         foreach ( Cell cell in map.GetCellsAlongLine( 3, 0, 0, 3 ) )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void GetCellsAlongLine_BottomRightToTopLeft_ExpectedCells()
      {
         string mapRepresentation = @"####
                                      #..#
                                      #so#
                                      ####";
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap map = LibtcodMap.Create( mapCreationStrategy );
         Debug.WriteLine( map.ToString() );
         string expectedPath = "#o.#";

         StringBuilder actualPath = new StringBuilder();
         foreach ( Cell cell in map.GetCellsAlongLine( 3, 3, 0, 0 ) )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void GetCellsAlongLine_ThirdRowLeftToRight_ExpectedCells()
      {
         string mapRepresentation = @"####
                                      #..#
                                      #so#
                                      ####";
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap map = LibtcodMap.Create( mapCreationStrategy );
         string expectedPath = "#so#";

         StringBuilder actualPath = new StringBuilder();
         foreach ( Cell cell in map.GetCellsAlongLine( 0, 2, 3, 2 ) )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void GetCellsAlongLine_ThirdRowRightToLeft_ExpectedCells()
      {
         string mapRepresentation = @"####
                                      #..#
                                      #so#
                                      ####";
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap map = LibtcodMap.Create( mapCreationStrategy );
         string expectedPath = "#os#";

         StringBuilder actualPath = new StringBuilder();
         foreach ( Cell cell in map.GetCellsAlongLine( 3, 2, 0, 2 ) )
         {
            actualPath.Append( cell.ToString() );
         }

         Assert.AreEqual( expectedPath, actualPath.ToString() );
      }

      [TestMethod]
      public void ComputeFov_X6Y1CellRadius20_ExpectedFovMap()
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
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap map = LibtcodMap.Create( mapCreationStrategy );

         map.ComputeFov( 6, 1, 20, true );

         string expectedFovMap = @"###########################%%%%%%%%%
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
         Assert.AreEqual( expectedFovMap.Replace( " ", string.Empty ), map.ToString( true ) );
      }
   }
}