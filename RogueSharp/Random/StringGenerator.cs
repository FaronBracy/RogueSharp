using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueSharp.Random
{
   /// <summary>
   /// A class for generating strings with placeholders that can be replaced randomly by words from lookup pools.
   /// </summary>
   /// <example>
   /// A MadLibs style description of a room could be implemented using the StringGenerator.
   /// "This {color} room has a {feature} in the corner."
   /// Where '{color}' and '{feature}' would be replaced by words from the "color" and "feature" word pools. 
   /// </example>
   public class StringGenerator
   {
      private readonly Dictionary<string, IWeightedPool<string>> _wordPools;
      private readonly IRandom _random;

      /// <summary>
      /// Construct a new StringGenerator using the default random number generator provided with .NET
      /// </summary>
      public StringGenerator()
         : this( Singleton.DefaultRandom )
      {
      }

      /// <summary>
      /// Construct a new StringGenerator using the provided random number generator
      /// </summary>
      /// <param name="random">A class implementing IRandom that will be used to generate pseudo-random numbers necessary to pick words from a pool</param>
      public StringGenerator( IRandom random )
      {
         _wordPools = new Dictionary<string, IWeightedPool<string>>();
         _random = random;
      }

      /// <summary>
      /// Create pool with the provided poolName and add the provided words to it.
      /// If a pool with the provided poolName already exists, add the provided words to that existing pool.
      /// </summary>
      /// <param name="poolName">The name of the pool to create. If the pool already exists it will be used</param>
      /// <param name="words">An array of words to add to the pool</param>
      /// <exception cref="ArgumentException">Required parameter "poolName" is null or whitespace</exception>
      /// <exception cref="ArgumentNullException">Required parameter "words" is null</exception>
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

      /// <summary>
      /// Create pool with the provided poolName and add the provided words to it.
      /// If a pool with the provided poolName already exists, add the provided words to that existing pool.
      /// </summary>
      /// <param name="poolName">The name of the pool to create. If the pool already exists it will be used</param>
      /// <param name="words">An IEnumerable of words to add to the pool</param>
      /// <exception cref="ArgumentException">Required parameter "poolName" is null or whitespace</exception>
      /// <exception cref="ArgumentNullException">Required parameter "words" is null</exception>
      public void AddWordsToPool( string poolName, IEnumerable<string> words )
      {
         AddWordsToPool( poolName, words.ToArray() );
      }

      /// <summary>
      /// Add an IWeightedPool of strings to the dictionary of pools to use for word substitution.
      /// A pool with the provided poolName must not already exist.
      /// </summary>
      /// <param name="poolName">The name of the pool to create. This must be unique and not the name of an existing pool</param>
      /// <param name="weightedWordPool">A class implementing IWeightedPool representing a collection of words to use for substitution</param>
      /// <exception cref="ArgumentException">Required parameter "poolName" is null or whitespace</exception>
      /// <exception cref="ArgumentNullException">Required parameter "weightedWordPool" is null</exception>
      /// <exception cref="InvalidOperationException">A pool with the provided "poolName" was previously added to this string generator</exception>
      public void AddWordPool( string poolName, IWeightedPool<string> weightedWordPool )
      {
         if ( string.IsNullOrWhiteSpace( poolName ) )
         {
            throw new ArgumentException( "Parameter poolName must not be null or whitespace", nameof( poolName ) );
         }
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

      /// <summary>
      /// Generate a string by replacing parameters surrounded by curly braces such as "{color}"
      /// with a random word from the pool with the parameterized name
      /// </summary>
      /// <remarks>
      /// A word chosen from the pool can be used more than once.
      /// </remarks>
      /// <param name="parameterizedString">A string with parameters in curly braces, which will have parameters replaced by a random word from the corresponding pool</param>
      /// <returns>A string with parameters replaced by random words from the corresponding pools</returns>
      public string Generate( string parameterizedString )
      {
         return Generate( parameterizedString, weightedPool => weightedPool.Choose() );
      }

      /// <summary>
      /// Generate a unique string by replacing parameters surrounded by curly braces such as "{color}"
      /// with a random word from the pool with the parameterized name
      /// </summary>
      /// <remarks>
      /// Once a word is chosen from the pool, it will be removed and will not be picked in the future.
      /// </remarks>
      /// <param name="parameterizedString">A string with parameters in curly braces, which will have parameters replaced by a random word from the corresponding pool</param>
      /// <returns>A string with parameters replaced by random words from the corresponding pools</returns>
      public string GenerateUnique( string parameterizedString )
      {
         return Generate( parameterizedString, weightedPool => weightedPool.Draw() );
      }

      private string Generate( string parameterizedString, Func<IWeightedPool<string>,string> stringSelectionFunc )
      {
         foreach ( KeyValuePair<string, IWeightedPool<string>> wordPool in _wordPools )
         {
            if ( wordPool.Key != null )
            {
               string poolName = $"{{{wordPool.Key}}}";
               int index = parameterizedString.IndexOf( poolName, StringComparison.OrdinalIgnoreCase );
               while ( index != -1 )
               {
                  parameterizedString = parameterizedString.Remove( index, poolName.Length ).Insert( index, stringSelectionFunc.Invoke( wordPool.Value ) );
                  index = parameterizedString.IndexOf( poolName, StringComparison.OrdinalIgnoreCase );
               }
            }
         }

         return parameterizedString;
      }
   }
}
