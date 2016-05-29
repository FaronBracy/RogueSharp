using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   /// <summary>
   /// A class implementing IRandom which cycles through a specified series of integers each
   /// time the Next random number is asked for.
   /// </summary>
   /// <remarks>
   /// This class is normally used for unit tests and not production code.
   /// </remarks>
   public class KnownSeriesRandom : IRandom
   {
      private long _numberGenerated;
      private Queue<int> _series;

      /// <summary>
      /// Constructs a new integer generator with the specified series of integers in an array.
      /// When the Next method is called on this generator it will return the first integer in the series,
      /// followed by the next integer and so on until it reaches the end of the array.
      /// If the Next method is called once it is at the end of the array, it will start back over at the beginning.
      /// </summary>
      /// <param name="series">A known series of integers that will be returned in order from this generator</param>
      /// <exception cref="ArgumentNullException">Thrown on null series</exception>
      public KnownSeriesRandom( params int[] series )
      {
         if ( series == null )
         {
            throw new ArgumentNullException( "series", "Series cannot be null" );
         }

         _series = new Queue<int>();
         foreach ( int number in series )
         {
            _series.Enqueue( number );
         }
      }

      /// <summary>
      /// Return the first integer in the series that was specified when this generator was constructed,
      /// followed by the next integer and so on until it reaches the end of the array.
      /// If the Next method is called once it is at the end of the array, it will start back over at the beginning.
      /// </summary>
      /// <param name="maxValue">Inclusive maximum result</param>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown when the Next integer in the series for this generator is not between 0 and the specified maxValue inclusive
      /// </exception>
      /// <returns>The next integer in the series specified upon construction of this class</returns>
      public int Next( int maxValue )
      {
         return Next( 0, maxValue );
      }

      /// <summary>
      /// Return the first integer in the series that was specified when this generator was constructed,
      /// followed by the next integer and so on until it reaches the end of the array.
      /// If the Next method is called once it is at the end of the array, it will start back over at the beginning.
      /// </summary>
      /// <param name="minValue">Inclusive minimum result</param>
      /// <param name="maxValue">Inclusive maximum result</param>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown when the Next integer in the series for this generator is not between 
      /// the specified minValue and maxValue inclusive
      /// </exception>
      /// <returns>The next integer in the series specified upon construction of this class</returns>
      public int Next( int minValue, int maxValue )
      {
         int value = _series.Dequeue();
         if ( value < minValue )
         {
            throw new ArgumentOutOfRangeException( "minValue", "Next value in series is smaller than the minValue parameter" );
         }
         if ( value > maxValue )
         {
            throw new ArgumentOutOfRangeException( "maxValue", "Next value in series is larger than the maxValue parameter" );
         }
         _series.Enqueue( value );
         _numberGenerated++;
         return value;
      }

      /// <summary>
      /// Saves the current state of the number generator
      /// </summary>
      /// <example>
      /// If you generated three random numbers and then called Save to store the state and 
      /// followed that up by generating 10 more numbers before calling Restore with the previously saved RandomState
      /// the Restore method should return the generator back to the state when Save was first called.
      /// This means that if you went on to generate 10 more numbers they would be the same 10 numbers that were
      /// generated the first time after Save was called.
      /// </example>
      /// <returns>A RandomState class representing the current state of this number generator</returns>
      public RandomState Save()
      {
         return new RandomState
         {
            NumberGenerated = _numberGenerated,
            Seed = _series.ToArray()
         };
      }

      /// <summary>
      /// Restores the state of the number generator based on the specified state parameter
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
            throw new ArgumentNullException( "state", "RandomState cannot be null" );
         }

         _series = new Queue<int>();
         foreach ( int i in state.Seed )
         {
            _series.Enqueue( i );
         }
         _numberGenerated = state.NumberGenerated;
      }
   }
}