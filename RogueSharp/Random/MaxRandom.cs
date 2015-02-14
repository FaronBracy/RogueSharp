namespace RogueSharp.Random
{
   /// <summary>
   /// A class implementing IRandom which always returns the highest possible result 
   /// </summary>
   public class MaxRandom : IRandom
   {
      /// <summary>
      /// Gets the next integer in the series which will always be maxValue
      /// </summary>
      /// <param name="maxValue">Inclusive maximum result which is always returned in this case</param>
      /// <returns>Returns the integer maxValue</returns>
      public int Next( int maxValue )
      {
         return maxValue;
      }

      /// <summary>
      /// Gets the next integer in the series which will always be maxValue
      /// </summary>
      /// <param name="minValue">Inclusive minimum result which is never used in this case</param>
      /// <param name="maxValue">Inclusive maximum result which is always returned in this case</param>
      /// <returns>Returns the integer maxValue</returns>
      public int Next( int minValue, int maxValue )
      {
         return maxValue;
      }

      /// <summary>
      /// Save the current state of the generator which is essentially a no-op for this generator
      /// </summary>
      /// <returns>A new RandomState object</returns>
      public RandomState Save()
      {
         return new RandomState();
      }

      /// <summary>
      /// Restores the state of the generator which is essentially a no-op for this generator
      /// </summary>
      /// <param name="state">Not used</param>
      public void Restore( RandomState state )
      {
         // No operation required
      }
   }
}
