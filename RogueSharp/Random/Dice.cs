using System.Collections.Generic;
using System.Linq;

namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public class Dice : IDice
   {
      private readonly ICollection<IDie> _dieCollection = new List<IDie>();
      /// <summary>
      /// 
      /// </summary>
      /// <param name="dieCollection"></param>
      public Dice( ICollection<IDie> dieCollection )
      {
         _dieCollection = dieCollection;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public ICollection<int> Roll()
      {
         return _dieCollection.Select( die => die.Roll() ).ToList();
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="die"></param>
      public void AddDie( IDie die )
      {
         _dieCollection.Add( die );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="die"></param>
      public void RemoveDie( IDie die )
      {
         _dieCollection.Remove( die );
      }
   }
}