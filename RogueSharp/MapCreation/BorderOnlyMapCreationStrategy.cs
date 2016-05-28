namespace RogueSharp.MapCreation
{
   /// <summary>
   /// The BorderOnlyMapCreationStrategy creates a Map of the specified type by making an empty map with only the outermost border being solid walls
   /// </summary>
   /// <typeparam name="T">The type of IMap that will be created</typeparam>
   public class BorderOnlyMapCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
   {
      private readonly int _height;
      private readonly int _width;

      /// <summary>
      /// Constructs a new BorderOnlyMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      public BorderOnlyMapCreationStrategy( int width, int height )
      {
         _width = width;
         _height = height;
      }

      /// <summary>
      /// Creates a Map of the specified type by making an empty map with only the outermost border being solid walls
      /// </summary>
      /// <returns>An IMap of the specified type</returns>
      public T CreateMap()
      {
         var map = new T();
         map.Initialize( _width, _height );
         map.Clear( true, true );

         foreach ( ICell cell in map.GetCellsInRows( 0, _height - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         foreach ( ICell cell in map.GetCellsInColumns( 0, _width - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         return map;
      }
   }
}