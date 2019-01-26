// TODO: Random notes for myself
// - String replacement thing
// - AStar
// - Map generation
// - Make sure no TODOs in solution
// - Replace FxCop
// - [,] map helpers look at PR

using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class WeightedPool<T> : IWeightedPool<T>
   {
      private int _totalWeight;
      private readonly IRandom _random;
      private List<WeightedItem<T>> _pool = new List<WeightedItem<T>>();
      private readonly Func<T, T> _cloneFunc;

      public int Count
      {
         get
         {
            return _pool.Count;
         }
      }

      public WeightedPool()
         : this( Singleton.DefaultRandom )
      {
      }

      public WeightedPool( IRandom random, Func<T, T> cloneFunc = null )
      {
         if ( random == null )
         {
            throw new ArgumentNullException( "random", "Implementation of IRandom must not be null" );
         }

         _random = random;
         _cloneFunc = cloneFunc;
      }

      public void Add( T item, int weight )
      {
         if ( item == null )
         {
            throw new ArgumentNullException( "item", "Can not add null item to the pool" );
         }
         if ( weight <= 0 )
         {
            throw new ArgumentException( "Weight must be greater than 0", "weight" );
         }

         WeightedItem<T> weightedItem = new WeightedItem<T>( item, weight );
         _pool.Add( weightedItem );
         if ( Int32.MaxValue - weight < _totalWeight )
         {
            throw new OverflowException( "The weight of items in the pool would be over Int32.MaxValue" );
         }
         _totalWeight += weight;
      }

      public T Select() 
      {
         if ( _cloneFunc == null )
         {
            throw new InvalidOperationException(  "A clone function was not defined when this pool was constructed" );
         }

         if ( Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to select one" );
         }

         WeightedItem<T> item = ChooseRandomWeightedItem();

         return _cloneFunc( item.Item );
      }

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

   public interface IWeightedPool<T>
   {
      /// <summary>
      /// Add an item of type T to the pool with the given weight
      /// </summary>
      /// <param name="item">The item to add to the pool</param>
      /// <param name="weight">The chance that the item will be drawn from the pool when weighted against all other items. Higher weights mean it is more likely to be drawn.</param>
      void Add( T item, int weight );

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      T Select();

      T Draw();

      void Clear();
   }
}
