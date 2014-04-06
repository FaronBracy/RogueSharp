using System;

namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public class GaussianRandom : IRandom
   {
      private int _seed;
      private System.Random _random;
      private long _numberGenerated;
      private double _nextGaussian;
      private bool _uselast = true;
      /// <summary>
      /// 
      /// </summary>
      public GaussianRandom()
         : this( Environment.TickCount )
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="seed"></param>
      public GaussianRandom( int seed )
      {
         _seed = seed;
         _random = new System.Random( _seed );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="maxValue"></param>
      /// <returns></returns>
      public int Next( int maxValue )
      {
         return Next( 0, maxValue );
      }
      /// <summary>
      /// Will approximitely give a random gaussian integer between min and max so that min and max are at
      /// 3.5 deviations from the mean (half-way of min and max).
      /// </summary>
      /// <param name="minValue"></param>
      /// <param name="maxValue"></param>
      /// <returns></returns>
      public int Next( int minValue, int maxValue )
      {
         _numberGenerated++;
         double deviations = 3.5;
         var r = (int) BoxMuller( minValue + ( maxValue - minValue ) / 2.0, ( maxValue - minValue ) / 2.0 / deviations );
         if ( r > maxValue )
         {
            r = maxValue;
         }
         else if ( r < minValue )
         {
            r = minValue;
         }
         //int r;
         //do
         //{
         //   r = (int) BoxMuller( minValue + ( maxValue - minValue ) / 2.0, ( maxValue - minValue ) / 2.0 / deviations );
         //}
         //while ( r > maxValue || r < minValue );

         return r;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public RandomState Save()
      {
         return new RandomState
         {
            NumberGenerated = _numberGenerated,
            Seed = new[]
            {
               _seed
            }
         };
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="state"></param>
      public void Restore( RandomState state )
      {
         _seed = state.Seed[0];
         _random = new System.Random( _seed );
         _numberGenerated = default( long );
         _nextGaussian = default( double );
         _uselast = true;
         for ( long i = 0; i < state.NumberGenerated; i++ )
         {
            Next( 1 );
         }
      }
      private double BoxMuller()
      {
         if ( _uselast )
         {
            _uselast = false;
            return _nextGaussian;
         }
         else
         {
            double v1, v2, s;
            do
            {
               v1 = 2.0 * _random.NextDouble() - 1.0;
               v2 = 2.0 * _random.NextDouble() - 1.0;
               s = v1 * v1 + v2 * v2;
            }
            while ( s >= 1.0 || s == 0 );

            s = Math.Sqrt( ( -2.0 * Math.Log( s ) ) / s );

            _nextGaussian = v2 * s;
            _uselast = true;
            return v1 * s;
         }
      }
      private double BoxMuller( double mean, double standardDeviation )
      {
         return mean + BoxMuller() * standardDeviation;
      }
   }
}