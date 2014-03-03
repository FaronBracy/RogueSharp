using System.Collections.Generic;

namespace RogueSharp.Random
{
   public interface IDice
   {
      ICollection<int> Roll();
      void AddDie( IDie die );
      void RemoveDie( IDie die );
   }
}