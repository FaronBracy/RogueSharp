using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class StringGenerator
   {
      private readonly Dictionary<string, IWeightedPool<string>> _wordPools;

      public StringGenerator()
      {
         _wordPools = new Dictionary<string, IWeightedPool<string>>();
      }

      public void AddWordsToPool( string poolName, string[] words )
      {
         throw new NotImplementedException();
      }

      public void AddWordsToPool( string poolName, IEnumerable<string> words )
      {
         throw new NotImplementedException();
      }

      public void AddWordPool( string poolName, IWeightedPool<string> weightedWordPool )
      {
         if ( weightedWordPool == null )
         {
            throw new ArgumentNullException( nameof( weightedWordPool ), "Can not add null word pool" );
         }

         if ( _wordPools.ContainsKey( poolName ) )
         {
            throw new InvalidOperationException( $"Word pool with poolName '{poolName}' has already been added to this string generator" );
         }

         _wordPools.Add( poolName, weightedWordPool );
      }

      public string Generate( string parameterizedString )
      {
         foreach ( KeyValuePair<string, IWeightedPool<string>> wordPool in _wordPools )
         {
            if ( wordPool.Key != null )
            {
               parameterizedString = parameterizedString.Replace( $"{{{wordPool.Key}}}", wordPool.Value.Choose() );
            }
         }

         return parameterizedString;
      }
   }
}
