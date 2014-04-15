namespace RogueSharp.Random
{
   /// <summary>
   /// An interface for representing a Die with a configurable number of sides that can be Rolled and report a result
   /// </summary>
   public interface IDie
   {
      /// <summary>
      /// Simulates rolling a Die and returns an integer representing the number that was rolled
      /// </summary>
      /// <returns>An integer representing the number that was rolled</returns>
      int Roll();
   }
}