using System;

namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public class DotNetRandom : IRandom
   {
      private int _seed;
      private long _numberGenerated;
      private System.Random _random = new System.Random();
      /// <summary>
      /// 
      /// </summary>
      public DotNetRandom()
         : this( Environment.TickCount )
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="seed"></param>
      public DotNetRandom( int seed )
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
      /// 
      /// </summary>
      /// <param name="minValue"></param>
      /// <param name="maxValue"></param>
      /// <returns></returns>
      public int Next( int minValue, int maxValue )
      {
         _numberGenerated++;
         return _random.Next( minValue, maxValue + 1 );
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
         for ( long i = 0; i < state.NumberGenerated; i++ )
         {
            _random.Next();
         }
      }
   }
}