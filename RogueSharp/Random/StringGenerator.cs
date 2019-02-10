using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class StringGenerator
   {
      private Dictionary<string, IWeightedPool<string>> _wordPools;

      public StringGenerator()
      {
         _wordPools = new Dictionary<string, IWeightedPool<string>>();
      }

      public void AddWordPool( string key, IWeightedPool<string> weightedWordPool )
      {
         if ( weightedWordPool == null )
         {
            throw new ArgumentNullException( nameof( weightedWordPool ), "Can not add null word pool" );
         }

         _wordPools.Add( key, weightedWordPool );
      }
   }
}
