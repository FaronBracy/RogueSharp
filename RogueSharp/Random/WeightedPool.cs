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
   public class WeightedPool<T> : IWeightedPool<T> //where T : ICloneable<T> 
   {
      private int _totalWeight;
      private readonly IRandom _random;
      private List<WeightedItem<T>> _pool = new List<WeightedItem<T>>();

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

      public WeightedPool( IRandom random )
      {
         if ( random == null )
         {
            throw new ArgumentNullException( "random", "Implementation of IRandom must not be null" );
         }

         _random = random;
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
         if ( Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to select one" );
         }
         throw new NotImplementedException();
      }

      public T Draw()
      {
         if ( Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to draw one" );
         }

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

         if ( item != null )
         {
            Remove( item );
            return item.Item;
         }

         throw new InvalidOperationException( "The random lookup was greater than the total weight" );
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
      void Add( T item, int weight );

      T Select();

      T Draw();

      void Clear();
   }

   //public interface ICloneable<T>
   //{
   //   T Clone();
   //}
}
