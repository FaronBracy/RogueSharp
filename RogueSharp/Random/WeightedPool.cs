using System;
using System.Collections.Generic;

namespace RogueSharp.Random
{
   public class WeightedPool<T> : IWeightedPool<T>
   {
      private int _totalWeight;
      private readonly List<WeightedItem<T>> _pool = new List<WeightedItem<T>>();

      public void Add( T item, int weight )
      {
         WeightedItem<T> weightedItem = new WeightedItem<T>( item, weight );
         _pool.Add( weightedItem );
         _totalWeight += weight;
      }

      public T Select()
      {
         throw new NotImplementedException();
      }

      public T Draw()
      {
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

      Clear();
   }
}
