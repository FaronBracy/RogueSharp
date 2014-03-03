using System;
using System.Collections.Generic;
using RogueSharp.Random;

namespace RogueSharp
{
   public interface IMapCreationStrategy<T> where T : IMap
   {
      T CreateMap();
   }

   public class RandomRoomsMapCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
   {
      private static IRandom _random;
      private readonly int _height;
      private readonly int _maxRooms;
      private readonly int _roomMaxSize;
      private readonly int _roomMinSize;
      private readonly int _width;

      public RandomRoomsMapCreationStrategy( int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, IRandom random = null )
      {
         _width = width;
         _height = height;
         _maxRooms = maxRooms;
         _roomMaxSize = roomMaxSize;
         _roomMinSize = roomMinSize;
         _random = random ?? new DotNetRandom();
      }

      public T CreateMap()
      {
         var rooms = new List<Rectangle>();
         var map = new T();
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

      private static void MakeRoom( T map, Rectangle room )
      {
         for ( int x = room.Left + 1; x < room.Right; x++ )
         {
            for ( int y = room.Top + 1; y < room.Bottom; y++ )
            {
               map.SetCellProperties( x, y, true, true );
            }
         }
      }

      private static void MakeHorizontalTunnel( T map, int xStart, int xEnd, int yPosition )
      {
         for ( int x = Math.Min( xStart, xEnd ); x <= Math.Max( xStart, xEnd ); x++ )
         {
            map.SetCellProperties( x, yPosition, true, true );
         }
      }

      private static void MakeVerticalTunnel( T map, int yStart, int yEnd, int xPosition )
      {
         for ( int y = Math.Min( yStart, yEnd ); y <= Math.Max( yStart, yEnd ); y++ )
         {
            map.SetCellProperties( xPosition, y, true, true );
         }
      }
   }

   public class DungeonCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
   {
      private readonly int _height;
      private readonly IRandom _random;
      private readonly int _rooms;
      private readonly int _width;

      public DungeonCreationStrategy( int width, int height, int rooms, IRandom random = null )
      {
         if ( width % 2 == 0 )
         {
            width--;
         }
         if ( height % 2 == 0 )
         {
            height--;
         }
         _width = width;
         _height = height;
         _rooms = rooms;
         _random = random ?? new DotNetRandom();
      }

      public T CreateMap()
      {
         var rooms = new List<Rectangle>();
         int roomsCreated = 0;
         var map = new T();
         map.Initialize( _width, _height );
         for ( int i = 0; i < 1000; i++ )
         {
            if ( roomsCreated == _rooms )
            {
               break;
            }

            int roomWidth = RandomSize();
            int roomHeight = RandomSize();
            int roomXPosition = RandomX( roomWidth );
            int roomYPosition = RandomY( roomHeight );
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

         return map;
      }

      private int RandomSize()
      {
         int size = _random.Next( 1, 3 );
         if ( size == 1 )
         {
            size = 7;
         }
         else if ( size == 2 )
         {
            size = 5;
         }
         return size;
      }

      private int RandomX( int roomWidth )
      {
         int roomXPosition = _random.Next( 1, _width - roomWidth - 1 );
         if ( roomXPosition % 2 == 0 )
         {
            roomXPosition--;
         }
         return roomXPosition;
      }

      private int RandomY( int roomHeight )
      {
         int roomYPosition = _random.Next( 1, _height - roomHeight - 1 );
         if ( roomYPosition % 2 == 0 )
         {
            roomYPosition--;
         }
         return roomYPosition;
      }

      private static void MakeRoom( T map, Rectangle room )
      {
         for ( int x = room.Left; x < room.Right; x++ )
         {
            for ( int y = room.Top; y < room.Bottom; y++ )
            {
               map.SetCellProperties( x, y, true, true );
            }
         }
      }
   }

   public class BorderOnlyMapCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
   {
      private readonly int _height;
      private readonly int _width;

      public BorderOnlyMapCreationStrategy( int width, int height )
      {
         _width = width;
         _height = height;
      }

      public T CreateMap()
      {
         var map = new T();
         map.Initialize( _width, _height );
         map.Clear( true, true );

         foreach ( Cell cell in map.GetCellsInRows( 0, _height - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         foreach ( Cell cell in map.GetCellsInColumns( 0, _width - 1 ) )
         {
            map.SetCellProperties( cell.X, cell.Y, false, false );
         }

         return map;
      }
   }

   public class StringDeserializeMapCreationStrategy<T> : IMapCreationStrategy<T> where T : IMap, new()
   {
      public string _mapRepresentation;

      public StringDeserializeMapCreationStrategy( string mapRepresentation )
      {
         _mapRepresentation = mapRepresentation;
      }

      public T CreateMap()
      {
         string[] lines = _mapRepresentation.Replace( " ", "" ).Replace( "\r", "" ).Split( '\n' );

         int width = lines[0].Length;
         int height = lines.Length;
         var map = new T();
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