using System.Collections.Generic;
using System.Linq;

namespace RogueSharp.Random
{
   /// <summary>
   /// A class representing a collection of Dice that can all be rolled together
   /// </summary>
   public class Dice : IDice
   {
      private readonly ICollection<IDie> _dieCollection = new List<IDie>();
      /// <summary>
      /// Constructs a new Dice object containing the specified ICollection of IDie
      /// </summary>
      /// <param name="dieCollection">An ICollection of IDie which is basically a bunch of Dice that will be ready to Roll</param>
      public Dice( ICollection<IDie> dieCollection )
      {
         _dieCollection = dieCollection;
      }
      /// <summary>
      /// Rolls each Die in this collection and returns a ICollection of int which are the results of each Die roll
      /// </summary>
      /// <returns>ICollection of ints which are the results of each Die roll</returns>
      public ICollection<int> Roll()
      {
         return _dieCollection.Select( die => die.Roll() ).ToList();
      }
      /// <summary>
      /// Add the specified Die to this collection of Dice
      /// </summary>
      /// <param name="die">The Die to add to this collection</param>
      public void AddDie( IDie die )
      {
         _dieCollection.Add( die );
      }
      /// <summary>
      /// Remove the specified Die from this collection of Dice
      /// </summary>
      /// <param name="die">The Die to remove from this collection</param>
      public void RemoveDie( IDie die )
      {
         _dieCollection.Remove( die );
      }
   }
}