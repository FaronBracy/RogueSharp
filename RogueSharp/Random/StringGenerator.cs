using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class StringGenerator
   {
      private readonly Dictionary<string, IWeightedPool<string>> _wordPools;
      private readonly IRandom _random;

      public StringGenerator()
         : this( Singleton.DefaultRandom )
      {
      }

      public StringGenerator( IRandom random )
      {
         _wordPools = new Dictionary<string, IWeightedPool<string>>();
         _random = random;
      }

      public void AddWordsToPool( string poolName, params string[] words )
      {
         if ( string.IsNullOrWhiteSpace( poolName ) )
         {
            throw new ArgumentException( "Parameter poolName must not be null or whitespace", nameof( poolName ) );
         }
         if ( words == null )
         {
            throw new ArgumentNullException( nameof( words ), "Parameter words must not be null" );
         }

         IWeightedPool<string> wordPool = new WeightedPool<string>( _random, x => x );

         if ( !_wordPools.ContainsKey( poolName ) )
         {
            _wordPools.Add( poolName, wordPool );
         }
         else
         {
            _wordPools.TryGetValue( poolName, out wordPool );
         }
         foreach ( string word in words )
         {
            wordPool?.Add( word, 1 );
         }
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
               string poolName = $"{{{wordPool.Key}}}";
               int index = parameterizedString.IndexOf( poolName, StringComparison.OrdinalIgnoreCase );
               while ( index != -1 )
               {
                  parameterizedString = parameterizedString.Remove( index, poolName.Length ).Insert( index, wordPool.Value.Choose() );
                  index = parameterizedString.IndexOf( poolName, StringComparison.OrdinalIgnoreCase );
               }
            }
         }

         return parameterizedString;
      }
   }
}
