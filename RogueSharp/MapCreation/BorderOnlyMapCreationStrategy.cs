namespace RogueSharp.MapCreation
{
   /// <summary>
   /// The BorderOnlyMapCreationStrategy creates a Map of the specified type by making an empty map with only the outermost border being solid walls
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   public class BorderOnlyMapCreationStrategy<TMap> : BorderOnlyMapCreationStrategy<TMap, Cell>, IMapCreationStrategy<TMap> where TMap : IMap<Cell>, new()
   {
      /// <summary>
      /// Constructs a new BorderOnlyMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      public BorderOnlyMapCreationStrategy( int width, int height )
         : base( width, height )
      {
      }
   }

   /// <summary>
   /// The BorderOnlyMapCreationStrategy creates a Map of the specified type by making an empty map with only the outermost border being solid walls
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   /// <typeparam name="TCell">The type of ICell that the Map will use</typeparam>
   public class BorderOnlyMapCreationStrategy<TMap,TCell> : IMapCreationStrategy<TMap,TCell> where TMap : IMap<TCell>, new() where TCell : ICell
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
      public TMap CreateMap()
      {
         var map = new TMap();
         map.Initialize( _width, _height );
         map.Clear( true, true );

         foreach ( TCell cell in map.GetCellsInRows( 0, _height - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         foreach ( TCell cell in map.GetCellsInColumns( 0, _width - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         return map;
      }
   }
}