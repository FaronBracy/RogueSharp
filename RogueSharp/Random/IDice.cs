using System.Collections.Generic;

namespace RogueSharp.Random
{
   /// <summary>
   /// An interface for representing a collection of Dice that can all be rolled together
   /// </summary>
   public interface IDice
   {
      /// <summary>
      /// Rolls each Die in this collection and returns a ICollection of int which are the results of each Die roll
      /// </summary>
      /// <returns>ICollection of ints which are the results of each Die roll</returns>
      ICollection<int> Roll();
      /// <summary>
      /// Add a Die to this collection of Dice
      /// </summary>
      /// <param name="die">The Die to add to this collection</param>
      void AddDie( IDie die );
      /// <summary>
      /// Remove a Die from this collection of Dice
      /// </summary>
      /// <param name="die">The Die to remove from this collection</param>
      void RemoveDie( IDie die );
   }
}