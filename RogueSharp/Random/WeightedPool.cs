using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class WeightedPool<T> : IWeightedPool<T>
   {
      private int _totalWeight;
      private readonly IRandom _random;
      private readonly List<WeightedItem<T>> _pool = new List<WeightedItem<T>>();

      public WeightedPool( IRandom random )
      {
         if ( random == null )
         {
            throw new ArgumentNullException( "Implementation of IRandom must not be null" );
         }

         _random = random;
      }

      public void Add( T item, int weight )
      {
         if ( item == null )
         {
            throw new ArgumentNullException( "Can not add null item to the pool" );
         }
         if ( weight <= 0 )
         {
            throw new ArgumentException( "Weight must be greater than 0" );
         }

         WeightedItem<T> weightedItem = new WeightedItem<T>( item, weight );
         _pool.Add( weightedItem );
         _totalWeight += weight;
      }

      public T Select()
      {
         if ( _pool.Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to select one" );
         }
         throw new NotImplementedException();
      }

      public T Draw()
      {
         if ( _pool.Count <= 0 || _totalWeight <= 0 )
         {
            throw new InvalidOperationException( "Add items to the pool before attempting to draw one" );
         }
         throw new NotImplementedException();
      }

      public void Clear()
      {
         throw new NotImplementedException();
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
}
