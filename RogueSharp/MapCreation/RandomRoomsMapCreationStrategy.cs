using System;
using System.Collections.Generic;
using RogueSharp.Random;

namespace RogueSharp.MapCreation
{
   /// <summary>
   /// The RandomRoomsMapCreationStrategy creates a Map of the specified type by placing rooms randomly and then connecting them with corridors
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   public class RandomRoomsMapCreationStrategy<TMap> : RandomRoomsMapCreationStrategy<TMap, Cell>, IMapCreationStrategy<TMap> where TMap : IMap<Cell>, new()
   {
      /// <summary>
      /// Constructs a new RandomRoomsMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      /// <param name="maxRooms">The maximum number of rooms that will exist in the generated Map</param>
      /// <param name="roomMaxSize">The maximum width and height of each room that will be generated in the Map</param>
      /// <param name="roomMinSize">The minimum width and height of each room that will be generated in the Map</param>
      /// <param name="random">A class implementing IRandom that will be used to generate pseudo-random numbers necessary to create the Map</param>
      public RandomRoomsMapCreationStrategy( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, IRandom random )
         : base( width, height, maxRooms, roomMaxSize, roomMinSize, random )
      {
      }

      /// <summary>
      /// Constructs a new RandomRoomsMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      /// <param name="maxRooms">The maximum number of rooms that will exist in the generated Map</param>
      /// <param name="roomMaxSize">The maximum width and height of each room that will be generated in the Map</param>
      /// <param name="roomMinSize">The minimum width and height of each room that will be generated in the Map</param>
      /// <remarks>Uses DotNetRandom as its RNG</remarks>
      public RandomRoomsMapCreationStrategy( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize )
         : base( width, height, maxRooms, roomMaxSize, roomMinSize )
      {
      }
   }

   /// <summary>
   /// The RandomRoomsMapCreationStrategy creates a Map of the specified type by placing rooms randomly and then connecting them with corridors
   /// </summary>
   /// <typeparam name="TMap">The type of IMap that will be created</typeparam>
   /// <typeparam name="TCell">The type of ICell that the Map will use</typeparam>
   public class RandomRoomsMapCreationStrategy<TMap,TCell> : IMapCreationStrategy<TMap,TCell> where TMap : IMap<TCell>, new() where TCell : ICell
   {
      private readonly IRandom _random;
      private readonly int _height;
      private readonly int _maxRooms;
      private readonly int _roomMaxSize;
      private readonly int _roomMinSize;
      private readonly int _width;

      /// <summary>
      /// Constructs a new RandomRoomsMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      /// <param name="maxRooms">The maximum number of rooms that will exist in the generated Map</param>
      /// <param name="roomMaxSize">The maximum width and height of each room that will be generated in the Map</param>
      /// <param name="roomMinSize">The minimum width and height of each room that will be generated in the Map</param>
      /// <param name="random">A class implementing IRandom that will be used to generate pseudo-random numbers necessary to create the Map</param>
      public RandomRoomsMapCreationStrategy( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, IRandom random )
      {
         _width = width;
         _height = height;
         _maxRooms = maxRooms;
         _roomMaxSize = roomMaxSize;
         _roomMinSize = roomMinSize;
         _random = random;
      }

      /// <summary>
      /// Constructs a new RandomRoomsMapCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="width">The width of the Map to be created</param>
      /// <param name="height">The height of the Map to be created</param>
      /// <param name="maxRooms">The maximum number of rooms that will exist in the generated Map</param>
      /// <param name="roomMaxSize">The maximum width and height of each room that will be generated in the Map</param>
      /// <param name="roomMinSize">The minimum width and height of each room that will be generated in the Map</param>
      /// <remarks>Uses DotNetRandom as its RNG</remarks>
      public RandomRoomsMapCreationStrategy( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize )
      {
         _width = width;
         _height = height;
         _maxRooms = maxRooms;
         _roomMaxSize = roomMaxSize;
         _roomMinSize = roomMinSize;
         _random = Singleton.DefaultRandom;
      }

      /// <summary>
      /// Creates a new IMap of the specified type.
      /// </summary>
      /// <remarks>
      /// The Map will be generated by trying to place rooms up to the maximum number specified in random locations around the Map.
      /// Each room will have a height and width between the minimum and maximum room size.
      /// If a room would be placed in such a way that it overlaps another room, it will not be placed.
      /// Once all rooms have have been placed, or thrown out because they overlap, corridors will be created between rooms in a random manner.
      /// </remarks>
      /// <returns>An IMap of the specified type</returns>
      public TMap CreateMap()
      {
         var rooms = new List<Rectangle>();
         var map = new TMap();
         map.Initialize( _width, _height );

         for ( int r = 0; r < _maxRooms; r++ )
         {
            int roomWidth = _random.Next( _roomMinSize, _roomMaxSize );
            int roomHeight = _random.Next( _roomMinSize, _roomMaxSize );
            int roomXPosition = _random.Next( 0, _width - roomWidth - 1 );
            int roomYPosition = _random.Next( 0, _height - roomHeight - 1 );

            var newRoom = new Rectangle( roomXPosition, roomYPosition, roomWidth, roomHeight );
            bool newRoomIntersects = false;
            foreach ( Rectangle room in rooms )
            {
               if ( newRoom.Intersects( room ) )
               {
                  newRoomIntersects = true;
                  break;
               }
            }
            if ( !newRoomIntersects )
            {
               rooms.Add( newRoom );
            }
         }

         foreach ( Rectangle room in rooms )
         {
            MakeRoom( map, room );
         }

         for ( int r = 0; r < rooms.Count; r++ )
         {
            if ( r == 0 )
            {
               continue;
            }

            int previousRoomCenterX = rooms[r - 1].Center.X;
            int previousRoomCenterY = rooms[r - 1].Center.Y;
            int currentRoomCenterX = rooms[r].Center.X;
            int currentRoomCenterY = rooms[r].Center.Y;

            if ( _random.Next( 0, 2 ) == 0 )
            {
               MakeHorizontalTunnel( map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY );
               MakeVerticalTunnel( map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX );
            }
            else
            {
               MakeVerticalTunnel( map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX );
               MakeHorizontalTunnel( map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY );
            }
         }

         return map;
      }

      private static void MakeRoom( TMap map, Rectangle room )
      {
         for ( int x = room.Left + 1; x < room.Right; x++ )
         {
            for ( int y = room.Top + 1; y < room.Bottom; y++ )
            {
               map.SetCellProperties( x, y, true, true );
            }
         }
      }

      private static void MakeHorizontalTunnel( TMap map, int xStart, int xEnd, int yPosition )
      {
         for ( int x = Math.Min( xStart, xEnd ); x <= Math.Max( xStart, xEnd ); x++ )
         {
            map.SetCellProperties( x, yPosition, true, true );
         }
      }

      private static void MakeVerticalTunnel( TMap map, int yStart, int yEnd, int xPosition )
      {
         for ( int y = Math.Min( yStart, yEnd ); y <= Math.Max( yStart, yEnd ); y++ )
         {
            map.SetCellProperties( xPosition, y, true, true );
         }
      }
   }
}