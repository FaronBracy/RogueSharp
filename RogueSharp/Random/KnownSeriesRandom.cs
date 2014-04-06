using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public class KnownSeriesRandom : IRandom
   {
      private long _numberGenerated;
      private Queue<int> _series;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="series"></param>
      public KnownSeriesRandom( params int[] series )
      {
         _series = new Queue<int>();
         foreach ( int number in series )
         {
            _series.Enqueue( number );
         }
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
      /// 
      /// </summary>
      /// <param name="minValue"></param>
      /// <param name="maxValue"></param>
      /// <returns></returns>
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
      /// 
      /// </summary>
      /// <returns></returns>
      public RandomState Save()
      {
         return new RandomState
         {
            NumberGenerated = _numberGenerated,
            Seed = _series.ToArray()
         };
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="state"></param>
      public void Restore( RandomState state )
      {
         _series = new Queue<int>();
         foreach ( int i in state.Seed )
         {
            _series.Enqueue( i );
         }
         _numberGenerated = state.NumberGenerated;
      }
   }
}