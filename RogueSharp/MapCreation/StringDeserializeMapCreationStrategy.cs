namespace RogueSharp.MapCreation
{
   /// <summary>
   /// The StringDeserializeMapCreationStrategy creates a Map of the specified type from a string representation of the Map
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   public class StringDeserializeMapCreationStrategy<TMap> : StringDeserializeMapCreationStrategy<TMap,Cell>, IMapCreationStrategy<TMap> where TMap : IMap<Cell>, new()
   {
      /// <summary>
      /// Constructs a new StringDeserializeMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="mapRepresentation">A string representation of the Map to be created</param>
      public StringDeserializeMapCreationStrategy( string mapRepresentation )
         : base( mapRepresentation )
      {
      }
   }

   /// <summary>
   /// The StringDeserializeMapCreationStrategy creates a Map of the specified type from a string representation of the Map
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   /// <typeparam name="TCell">The type of ICell that the Map will use</typeparam>
   public class StringDeserializeMapCreationStrategy<TMap,TCell> : IMapCreationStrategy<TMap,TCell> where TMap : IMap<TCell>, new() where TCell : ICell
   {
      private readonly string _mapRepresentation;

      /// <summary>
      /// Constructs a new StringDeserializeMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="mapRepresentation">A string representation of the Map to be created</param>
      public StringDeserializeMapCreationStrategy( string mapRepresentation )
      {
         _mapRepresentation = mapRepresentation;
      }

      /// <summary>
      /// Creates a Map of the specified type from a string representation of the Map
      /// </summary>
      /// <remarks>
      /// The following symbols represent Cells on the Map:
      /// - `.`: `Cell` is transparent and walkable
      /// - `s`: `Cell` is walkable (but not transparent)
      /// - `o`: `Cell` is transparent (but not walkable)
      /// - `#`: `Cell` is not transparent or walkable
      /// </remarks>
      /// <returns>An IMap of the specified type</returns>
      public TMap CreateMap()
      {
         string[] lines = _mapRepresentation.Replace( " ", "" ).Replace( "\r", "" ).Split( '\n' );

         int width = lines[0].Length;
         int height = lines.Length;
         var map = new TMap();
         map.Initialize( width, height );

         for ( int y = 0; y < height; y++ )
         {
            string line = lines[y];
            for ( int x = 0; x < width; x++ )
            {
               if ( line[x] == '.' )
               {
                  map.SetCellProperties( x, y, true, true );
               }
               else if ( line[x] == 's' )
               {
                  map.SetCellProperties( x, y, false, true );
               }
               else if ( line[x] == 'o' )
               {
                  map.SetCellProperties( x, y, true, false );
               }
               else if ( line[x] == '#' )
               {
                  map.SetCellProperties( x, y, false, false );
               }
            }
         }

         return map;
      }
   }
}