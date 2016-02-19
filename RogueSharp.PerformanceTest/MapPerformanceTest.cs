using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.PerformanceTest
{
   //[TestClass]
   public class MapPerformanceTest
   {
      //[TestMethod]
      public void ComputeFovTest()
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
         IMapCreationStrategy<LibtcodMap> mapCreationStrategy1 = new StringDeserializeMapCreationStrategy<LibtcodMap>( mapRepresentation );
         IMap libtcodMap = LibtcodMap.Create( mapCreationStrategy1 );
         double libtcodTime;
         libtcodTime = StopwatchHelper.AverageTime( () => libtcodMap.ComputeFov( 6, 1, 20, true ), 100 );

         long libtcodElapsed;
         libtcodElapsed = StopwatchHelper.ElapsedTime( () => libtcodMap.ComputeFov( 6, 1, 20, true ), 100 );

         Debug.WriteLine( string.Format( "Libtcod Average: {0} Elapsed: {1}", libtcodTime, libtcodElapsed ) );

         IMapCreationStrategy<Map> mapCreationStrategy2 = new StringDeserializeMapCreationStrategy<Map>( mapRepresentation );
         IMap map = Map.Create( mapCreationStrategy2 );
         double rogueSharpTime;
         rogueSharpTime = StopwatchHelper.AverageTime( () => map.ComputeFov( 6, 1, 20, true ), 100 );

         double rogueSharpElapsed;
         rogueSharpElapsed = StopwatchHelper.ElapsedTime( () => map.ComputeFov( 6, 1, 20, true ), 100 );

         Debug.WriteLine( string.Format( "RogueSharp Average: {0} Elapsed: {1}", rogueSharpTime, rogueSharpElapsed ) );

         Assert.IsTrue( rogueSharpElapsed <= libtcodElapsed, string.Format( "Elapsed R#: {0} Lib: {1}", rogueSharpElapsed, libtcodElapsed ) );
         Assert.IsTrue( rogueSharpTime <= libtcodTime, string.Format( "Average R#: {0} Lib: {1}", rogueSharpTime, libtcodTime ) );
      }
   }

   public static class StopwatchHelper
   {
      public static TimeSpan Time( Action action )
      {
         Stopwatch stopwatch = Stopwatch.StartNew();
         action();
         stopwatch.Stop();
         return stopwatch.Elapsed;
      }

      public static double AverageTime( Action action, int iterations )
      {
         List<TimeSpan> times = new List<TimeSpan>();
         for ( int i = 0; i < iterations; i++ )
         {
            TimeSpan time = Time( action );
            times.Add( time );
         }
         return times.Select( t => t.Milliseconds ).Average();
      }

      public static long ElapsedTime( Action action, int iterations )
      {
         long elapsed = 0;
         for ( int i = 0; i < iterations; i++ )
         {
            TimeSpan time = Time( action );
            elapsed += time.Milliseconds;
         }
         return elapsed;
      }
   }
}
