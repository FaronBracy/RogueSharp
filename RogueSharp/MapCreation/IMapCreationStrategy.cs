namespace RogueSharp.MapCreation
{
   /// <summary>
   /// An generic interface for creating a new IMap of a specified type
   /// </summary>
   /// <typeparam name="T">The type of IMap that will be created</typeparam>
   public interface IMapCreationStrategy<T> where T : IMap
   {
      /// <summary>
      /// Creates a new IMap of the specified type
      /// </summary>
      /// <returns>An IMap of the specified type</returns>
      T CreateMap();
   }
}
