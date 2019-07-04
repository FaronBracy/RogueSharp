namespace RogueSharp.MapCreation
{
   /// <summary>
   /// An generic interface for creating a new IMap of a specified type
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   public interface IMapCreationStrategy<out TMap> : IMapCreationStrategy<TMap, Cell> where TMap : IMap<Cell>
   {
   }

   /// <summary>
   /// An generic interface for creating a new IMap of a specified type
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   /// <typeparam name="TCell">The type of ICell that the Map will use</typeparam>
   public interface IMapCreationStrategy<out TMap,TCell> where TMap : IMap<TCell> where TCell : ICell
   {
      /// <summary>
      /// Creates a new IMap of the specified type
      /// </summary>
      /// <returns>An IMap of the specified type</returns>
      TMap CreateMap();
   }
}
