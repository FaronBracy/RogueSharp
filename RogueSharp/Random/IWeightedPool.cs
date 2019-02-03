namespace RogueSharp.Random
{
   /// <summary>
   /// An Interface for weighted collections from which items of type T can be chosen at random.
   /// Although the item is picked at random, weights will be considered.
   /// Items with higher weights will be more likely to be picked than items with lower weights.
   /// </summary>
   /// <example>
   /// A treasure lookup table in a board game could be implemented as a WeightedPool.
   /// A booster pack for a collectable card game could be implemented as a WeightedPool.
   /// </example>
   /// <typeparam name="T">The type of item to be stored in the pool</typeparam>
   public interface IWeightedPool<T>
   {
      /// <summary>
      /// Add an item of type T to the WeightedPool with the given weight
      /// </summary>
      /// <param name="item">The item to add to the WeightedPool</param>
      /// <param name="weight">The chance that the item will be picked at random from the pool when weighted against all other items. Higher weights mean it is more likely to be picked.</param>
      void Add( T item, int weight );

      /// <summary>
      /// Choose an item at random from the pool, keeping weights into consideration.
      /// The item itself will remain in the pool and a clone of the item selected will be returned.
      /// </summary>
      /// <returns>A clone of the item of type T from the WeightedPool</returns>
      T Choose();

      /// <summary>
      /// Take an item at random from the pool, keeping weights into consideration.
      /// The item will be removed from the pool.
      /// </summary>
      /// <returns>An item of type T from the WeightedPool</returns>
      T Draw();

      /// <summary>
      /// Remove all items from the WeightedPool.
      /// The WeightedPool will be empty after calling this method.
      /// </summary>
      void Clear();
   }
}