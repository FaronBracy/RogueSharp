using System.Collections.Generic;

namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public interface IDice
   {
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      ICollection<int> Roll();
      /// <summary>
      /// 
      /// </summary>
      /// <param name="die"></param>
      void AddDie( IDie die );
      /// <summary>
      /// 
      /// </summary>
      /// <param name="die"></param>
      void RemoveDie( IDie die );
   }
}