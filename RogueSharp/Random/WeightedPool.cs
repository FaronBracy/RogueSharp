using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   /// <summary>
   /// A weighted collection from which items of type T can be chosen at random.
   /// Although the item is picked at random, weights will be considered.
   /// Items with higher weights will be more likely to be picked than items with lower weights.
   /// </summary>
   /// <example>
   /// A treasure lookup table in a board game could be implemented as a WeightedPool.
   /// A booster pack for a collectable card game could be implemented as a WeightedPool.
   /// </example>
   /// <typeparam name="T">The type of item to be stored in the WeightedPool</typeparam>
   public class WeightedPool<T> : IWeightedPool<T>
   {
      private int _totalWeight;
      private readonly IRandom _random;
      private List<WeightedItem<T>> _pool = new List<WeightedItem<T>>();
      private readonly Func<T, T> _cloneFunc;

      /// <summary>
      /// How many items are in the WeightedPool
      /// </summary>
      public int Count => _pool.Count;

      /// <summary>
      /// Construct a new weighted pool using the default random number generator provided with .NET
      /// This constructor does not take a clone function so Choose() cannot be called, only Draw()
      /// </summary>
      public WeightedPool()
         : this( Singleton.DefaultRandom )
      {
      }

      /// <summary>
      /// Construct a new weighted pool using the provided random number generator and clone function
      /// </summary>
      /// <param name="random">A class implementing IRandom that will be used to generate pseudo-random numbers necessary to pick items from the WeightedPool</param>
      /// <param name="cloneFunc">
      /// A function that takes an object of type T and returns a clone of the item.
      /// When comparing the original object and the clone, all properties should be equal.
      /// The clone will have a different reference than the original object.
      /// </param>
      /// <exception cref="ArgumentNullException">Thrown when provided "random" argument is null</exception>
      public WeightedPool( IRandom random, Func<T, T> cloneFunc = null )
      {
         _random = random ?? throw new ArgumentNullException( nameof( random ), "Implementation of IRandom must not be null" );
         _cloneFunc = cloneFunc;
      }

      /// <summary>
      /// Add an item of type T to the WeightedPool with the given weight
      /// </summary>
      /// <param name="item">The item to add to the WeightedPool</param>
      /// <param name="weight">
      /// The chance that the item will be picked at random from the pool when weighted against all other items.
      /// Higher weights mean it is more likely to be picked.
      /// </param>
      /// <exception cref="ArgumentNullException">Thrown when provided "item" argument is null</exception>
      /// <exception cref="ArgumentException">Thrown when provided "weight" argument is not greater than 0</exception>
      /// <exception cref="OverflowException">Thrown when adding the weight of the new item to the pool exceeds Int32.MaxValue</exception>
      public void Add( T item, int weight )
      {
         if ( item == null )
         {
            throw new ArgumentNullException( nameof( item ), "Can not add null item to the pool" );
         }
         if ( weight <= 0 )
         {
            throw new ArgumentException( "Weight must be greater than 0", nameof( weight ) );
         }

         WeightedItem<T> weightedItem = new WeightedItem<T>( item, weight );
         _pool.Add( weightedItem );
         if ( int.MaxValue - weight < _totalWeight )
         {
            throw new OverflowException( "The weight of items in the pool would be over Int32.MaxValue" );
         }
         _totalWeight += weight;
      }

      /// <summary>
      /// Choose an item at random from the pool, keeping weights into consideration.
      /// The item itself will remain in the pool and a clone of the item selected will be returned.
      /// </summary>
      /// <returns>A clone of the item of type T from the WeightedPool</returns>
      /// <exception cref="InvalidOperationException">Thrown when a clone function was not defined when the pool was constructed</exception>
      /// <exception cref="InvalidOperationException">Thrown when the pool is empty</exception>
      /// <exception cref="InvalidOperationException">Thrown when the random lookup is greater than the total weight. Could only happen if a bad implementation of IRandom were provided</exception>
      public T Choose()
      {
         if ( _cloneFunc == null )
         {
            throw new InvalidOperationException( "A clone function was not defined when this pool was constructed" );
         }

         if ( Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to choose one" );
         }

         WeightedItem<T> item = ChooseRandomWeightedItem();

         return _cloneFunc( item.Item );
      }

      /// <summary>
      /// Take an item at random from the pool, keeping weights into consideration.
      /// The item will be removed from the pool.
      /// </summary>
      /// <returns>An item of type T from the WeightedPool</returns>
      /// <exception cref="InvalidOperationException">Thrown when the pool is empty</exception>
      /// <exception cref="InvalidOperationException">Thrown when the random lookup is greater than the total weight. Could only happen if a bad implementation of IRandom were provided</exception>
      public T Draw()
      {
         if ( Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to draw one" );
         }

         WeightedItem<T> item = ChooseRandomWeightedItem();

         Remove( item );
         return item.Item;
      }

      private WeightedItem<T> ChooseRandomWeightedItem()
      {
         int lookupWeight = _random.Next( 1, _totalWeight );
         int currentWeight = 0;
         WeightedItem<T> item = null;
         foreach ( WeightedItem<T> weightedItem in _pool )
         {
            currentWeight += weightedItem.Weight;
            if ( currentWeight >= lookupWeight )
            {
               item = weightedItem;
               break;
            }
         }

         if ( item == null )
         {
            throw new InvalidOperationException( "The random lookup was greater than the total weight" );
         }

         return item;
      }

      private void Remove( WeightedItem<T> item )
      {
         if ( item != null && _pool.Remove( item ) )
         {
            _totalWeight -= item.Weight;
         }
      }

      /// <summary>
      /// Remove all items from the WeightedPool.
      /// The WeightedPool will be empty after calling this method.
      /// </summary>
      public void Clear()
      {
         _totalWeight = 0;
         _pool = new List<WeightedItem<T>>();
      }
   }

   internal class WeightedItem<T>
   {
      public T Item
      {
         get;
         private set;
      }

      public int Weight
      {
         get;
         private set;
      }

      public WeightedItem( T item, int weight )
      {
         Item = item;
         Weight = weight;
      }
   }
}
