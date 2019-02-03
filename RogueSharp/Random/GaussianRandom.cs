using System;

namespace RogueSharp.Random
{
   /// <summary>
   /// A class implementing IRandom which uses the Box-Muller transformation 
   /// to help generate Gaussian pseudo-random numbers
   /// </summary>
   /// <remarks>
   /// Gaussian pseudo-random generation can be useful if you want a bell shaped curve distribution of numbers.
   /// What this means is numbers half way between the min and max values are much more likely than
   /// numbers on the extreme edge. If you were to generate numbers between 1 and 10, it would be more
   /// likely a 5 would be generated than a 1 or a 10.
   /// </remarks>
   public class GaussianRandom : IRandom
   {
      private int _seed;
      private System.Random _random;
      private long _numberGenerated;
      private double _nextGaussian;
      private bool _useLast = true;

      /// <summary>
      /// Constructs a new Gaussian pseudo-random number generator 
      /// with a seed based on the number of milliseconds elapsed since the system started
      /// </summary>
      public GaussianRandom()
         : this( Environment.TickCount )
      {
      }

      /// <summary>
      /// Constructs a new Gaussian pseudo-random number generator with the specified seed
      /// </summary>
      /// <param name="seed">An integer used to calculate a starting value for the pseudo-random number sequence</param>
      public GaussianRandom( int seed )
      {
         _seed = seed;
         _random = new System.Random( _seed );
      }

      /// <summary>
      /// Will approximately give the next Gaussian pseudo-random integer between 0 and that specified max value inclusively
      /// so that min and max are at 3.5 deviations from the mean (half-way of min and max).
      /// </summary>
      /// <param name="maxValue">Inclusive maximum result</param>
      /// <returns>Returns a Gaussian pseudo-random integer between 0 and the specified maxValue inclusive</returns>
      public int Next( int maxValue )
      {
         return Next( 0, maxValue );
      }

      /// <summary>
      /// Will approximately give the next random Gaussian integer between the specified min and max values inclusively 
      /// so that min and max are at 3.5 deviations from the mean (half-way of min and max).
      /// </summary>
      /// <param name="minValue">Inclusive minimum result</param>
      /// <param name="maxValue">Inclusive maximum result</param>
      /// <returns>Returns a pseudo-random integer between the specified minValue and maxValue inclusive</returns>
      public int Next( int minValue, int maxValue )
      {
         _numberGenerated++;
         double deviations = 3.5;
         var r = (int) BoxMuller( minValue + ( ( maxValue - minValue ) / 2.0 ), ( maxValue - minValue ) / 2.0 / deviations );
         if ( r > maxValue )
         {
            r = maxValue;
         }
         else if ( r < minValue )
         {
            r = minValue;
         }

         return r;
      }

      /// <summary>
      /// Saves the current state of the pseudo-random number generator
      /// </summary>
      /// <example>
      /// If you generated three random numbers and then called Save to store the state and 
      /// followed that up by generating 10 more numbers before calling Restore with the previously saved RandomState
      /// the Restore method should return the generator back to the state when Save was first called.
      /// This means that if you went on to generate 10 more numbers they would be the same 10 numbers that were
      /// generated the first time after Save was called.
      /// </example>
      /// <returns>A RandomState class representing the current state of this pseudo-random number generator</returns>
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
      /// Restores the state of the pseudo-random number generator based on the specified state parameter
      /// </summary>
      /// <example>
      /// If you generated three random numbers and then called Save to store the state and 
      /// followed that up by generating 10 more numbers before calling Restore with the previously saved RandomState
      /// the Restore method should return the generator back to the state when Save was first called.
      /// This means that if you went on to generate 10 more numbers they would be the same 10 numbers that were
      /// generated the first time after Save was called.
      /// </example>
      /// <param name="state">The state to restore to, usually obtained from calling the Save method</param>
      /// <exception cref="ArgumentNullException">Thrown on null RandomState</exception>
      public void Restore( RandomState state )
      {
         if ( state == null )
         {
            throw new ArgumentNullException( nameof( state ), "RandomState cannot be null" );
         }

         _seed = state.Seed[0];
         _random = new System.Random( _seed );
         _numberGenerated = default( long );
         _nextGaussian = default( double );
         _useLast = true;
         for ( long i = 0; i < state.NumberGenerated; i++ )
         {
            Next( 1 );
         }
      }
      private double BoxMuller()
      {
         if ( _useLast )
         {
            _useLast = false;
            return _nextGaussian;
         }
         else
         {
            double v1, v2, s;
            do
            {
               v1 = ( 2.0 * _random.NextDouble() ) - 1.0;
               v2 = ( 2.0 * _random.NextDouble() ) - 1.0;
               s = ( v1 * v1 ) + ( v2 * v2 );
            }
            while ( s >= 1.0 || s == 0 );

            s = Math.Sqrt( -2.0 * Math.Log( s ) / s );

            _nextGaussian = v2 * s;
            _useLast = true;
            return v1 * s;
         }
      }
      private double BoxMuller( double mean, double standardDeviation )
      {
         return mean + ( BoxMuller() * standardDeviation );
      }
   }
}