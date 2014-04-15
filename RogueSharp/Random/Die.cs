namespace RogueSharp.Random
{
   /// <summary>
   /// A class representing a Die with a configurable number of sides that can be Rolled and report a result
   /// </summary>
   public class Die : IDie
   {
      private readonly IRandom _random;
      private readonly int _sides;
      /// <summary>
      /// Constructs a new Die object using the specified pseudo-random number generator and the specified number of sides
      /// </summary>
      /// <param name="random">A class implimenting IRandom that can generate pseudo-random numbers between 1 and the specified number of sides</param>
      /// <param name="sides">The number of sides that this Die has. When rolled this Die will generate a pseudo-random number between 1 and the specified number of sides</param>
      public Die( IRandom random, int sides )
      {
         _random = random;
         _sides = sides;
      }
      /// <summary>
      /// Simulates rolling a Die and returns an integer representing the number that was rolled
      /// </summary>
      /// <returns>An integer representing the number that was rolled</returns>
      public int Roll()
      {
         return _random.Next( 1, _sides );
      }
   }
}