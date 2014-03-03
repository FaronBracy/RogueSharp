using System.Collections.Generic;
using System.Linq;

namespace RogueSharp.Random
{
   public class Dice : IDice
   {
      private readonly ICollection<IDie> _dieCollection = new List<IDie>();

      public Dice( ICollection<IDie> dieCollection )
      {
         _dieCollection = dieCollection;
      }

      public ICollection<int> Roll()
      {
         return _dieCollection.Select( die => die.Roll() ).ToList();
      }

      public void AddDie( IDie die )
      {
         _dieCollection.Add( die );
      }

      public void RemoveDie( IDie die )
      {
         _dieCollection.Remove( die );
      }
   }
}