using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RogueSharp.Algorithms;
using RogueSharp.Random;

namespace RogueSharp.MapCreation
{
   /// <summary>
   /// The DungeonCreationStrategy creates a Map of the Dungeon type by extending the generic Map class with rooms, corridors, entrances and stairs.
   /// </summary>
   public class DungeonCreationStrategy : IMapCreationStrategy<Dungeon, DungeonCell>
   {
      private readonly int _mapWidth;
      private readonly int _mapHeight;
      private readonly IRandom _random;
      private Dungeon _dungeon;

      /// <summary>
      /// Constructs a new DungeonCreationStrategy with the specified parameters
      /// </summary>
      /// <param name="mapWidth">The width of the Map to be created. Must be an odd number.</param>
      /// <param name="mapHeight">The height of the Map to be created. Must be an odd number.</param>
      /// <param name="random">A class implementing IRandom that will be used to generate pseudo-random numbers necessary to create the Map. Uses standard DotNetRandom if omitted.</param>
      public DungeonCreationStrategy( int mapWidth, int mapHeight, IRandom  random = null )
      {
         if ( mapWidth % 2 == 0 )
         {
            throw new ArgumentException( "mapWidth must be odd", nameof(mapWidth) );
         }
         if ( mapHeight % 2 == 0 )
         {
            throw new ArgumentException( "mapHeight must be odd", nameof( mapWidth ) );
         }

         _mapWidth = mapWidth;
         _mapHeight = mapHeight;
         if ( random == null )
         {
            random = new DotNetRandom();
         }
         _random = random;
      }

      /// <summary>
      /// Creates a new Dungeon
      /// </summary>
      /// <returns>A new Dungeon which extends the generic Map class with rooms, corridors, entrances and stairs</returns>
      public Dungeon CreateMap()
      {
         _dungeon = new Dungeon();
         _dungeon.Initialize( _mapWidth, _mapHeight );  
         var builder = new DungeonBuilder( _dungeon, 50, _random );
         _dungeon = builder.Create();
         return _dungeon;
      }
   }
   
   public class Dungeon : Map<DungeonCell>
   {
      public Dungeon()
      {
         AreaCount = 0;
         EntranceCount = 0;
         Rooms = new List<Room>();
         Corridors = new List<Corridor>();
         Entrances = new List<Entrance>();
      }

      public int AreaCount { get; private set; }
      public int EntranceCount { get; private set; }
      public List<Room> Rooms { get; private set; }
      public List<Corridor> Corridors { get; private set; }
      public List<Entrance> Entrances { get; private set; }
      public Feature StairUp { get; set; }
      public Feature StairDown { get; set; }

      public void AddRoom( Room room )
      {
         room.Id = AreaCount++;
         Rooms.Add( room );
      }

      public override string ToString()
      {
         var mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( DungeonCell dungeonCell in GetAllCells() )
         {
            if ( dungeonCell.Y != lastY )
            {
               lastY = dungeonCell.Y;
               mapRepresentation.Append( Environment.NewLine );
            }
            mapRepresentation.Append( dungeonCell );
         }
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      public void AddCorridor( Corridor corridor )
      {
         corridor.Id = AreaCount++;
         Corridors.Add( corridor );
      }

      public void AddEntrance( IDungeonArea dungeonArea, IDungeonArea connectedDungeonArea, Entrance entrance )
      {
         entrance.Id = EntranceCount++;
         Entrances.Add( entrance );
         dungeonArea.EntranceIds.Add( entrance.Id );
         connectedDungeonArea.EntranceIds.Add( entrance.Id );
      }

      public List<Entrance> GetEntrances( IDungeonArea dungeonArea )
      {
         return dungeonArea.EntranceIds.Select( entranceId => Entrances.Find( e => e.Id == entranceId ) ).ToList();
      }

      public IDungeonArea GetArea( DungeonCell dungeonCell )
      {
         foreach ( Room room in Rooms )
         {
            if ( room.Contains( dungeonCell ) )
            {
               return room;
            }
         }

         foreach ( Corridor cooridor in Corridors )
         {
            if ( cooridor.Contains( dungeonCell ) )
            {
               return cooridor;
            }
         }

         return null;
      }

      public IDungeonArea GetArea( int id )
      {
         foreach ( Room room in Rooms )
         {
            if ( room.Id == id )
            {
               return room;
            }
         }

         foreach ( Corridor corridor in Corridors )
         {
            if ( corridor.Id == id )
            {
               return corridor;
            }
         }

         return null;
      }
   }

   public class Room : IDungeonArea
   {
      public Room()
      {
         EntranceIds = new List<int>();
      }

      public Rectangle Floor { get; set; }
      public int Id { get; set; }
      public List<int> EntranceIds { get; set; }
      public string Description { get; set; }

      public bool Contains( DungeonCell dungeonCell )
      {
         return Floor.Contains( new Point( dungeonCell.X, dungeonCell.Y ) );
      }
   }

   public class Corridor : IDungeonArea
   {
      public Corridor()
      {
         EntranceIds = new List<int>();
      }

      public List<DungeonCell> Floors { get; set; }
      public int Id { get; set; }
      public List<int> EntranceIds { get; set; }
      public string Description { get; set; }

      public bool Contains( DungeonCell dungeonCell )
      {
         return Floors.Contains( dungeonCell );
      }
   }

   public interface IDungeonArea
   {
      int Id { get; set; }
      List<int> EntranceIds { get; set; }
      string Description { get; set; }
      bool Contains( DungeonCell dungeonCell );
   }

   public class Feature
   {
      public int Id { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
   }

   public class Entrance
   {
      public int Id { get; set; }
      public int X { get; set; }
      public int Y { get; set; }
      public EntranceKind EntranceKind { get; set; }
      public bool IsTrapped { get; set; }
      public bool IsLocked { get; set; }
      public string Description { get; set; }
      public int Symbol { get; set; }
      public Color Color { get; set; }
   }

   public enum EntranceKind
   {
      Empty = 0,
      DoorClosed = 1,
      DoorOpen = 2
   }

   public class Color
   {
      public int Red { get; set; }
      public int Green { get; set; }
      public int Blue { get; set; }

      public Color( int red, int green, int blue )
      {
         Red = red;
         Green = green;
         Blue = blue;
      }
   }

   public class DungeonCell : Cell
   {
      public override string ToString()
      {
         if ( IsWalkable )
         {
            if ( IsTransparent )
            {
               return ".";
            }
            else
            {
               return "s";
            }
         }
         else
         {
            if ( IsTransparent )
            {
               return "o";
            }
            else
            {
               return "#";
            }
         }
      }
   }

   #region DungeonBuilder

   public class DungeonBuilder
   {
      public enum Side
      {
         None = 0,
         Top = 1,
         Left = 2,
         Bottom = 3,
         Right = 4
      }

      private readonly int _height;
      private readonly Dungeon _dungeon;
      private readonly IRandom _random;
      private readonly int _rooms;
      private readonly int _width;

      private DungeonAreaFeatureBuilder _featureBuilder;
      private EntranceBuilder _entranceBuilder;

      public DungeonBuilder( Dungeon dungeon, int rooms, IRandom random = null )
      {
         _width = dungeon.Width;
         _height = dungeon.Height;
         if ( _width % 2 == 0 )
         {
            _width--;
         }

         if ( _height % 2 == 0 )
         {
            _height--;
         }

         _dungeon = dungeon;
         _dungeon.Initialize( _width, _height );
         _rooms = rooms;
         _random = random ?? new DotNetRandom();
      }

      public Dungeon Create()
      {
         _featureBuilder = new DungeonAreaFeatureBuilder( _dungeon, _random );
         _entranceBuilder = new EntranceBuilder( _dungeon, _random );
         int roomsCreated = 0;
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
            foreach ( Room room in _dungeon.Rooms )
            {
               if ( newRoom.Intersects( room.Floor ) )
               {
                  newRoomIntersects = true;
                  break;
               }
            }

            if ( !newRoomIntersects )
            {
               _dungeon.AddRoom( new Room
               {
                  Floor = newRoom,
                  Description = _featureBuilder.GetRoomDescription()
               } );
            }
         }

         foreach ( Room room in _dungeon.Rooms )
         {
            DigRoom( _dungeon, room.Floor );
         }

         DigCorridors( _dungeon );

         DigEntrances( _dungeon, _random );

         AddStairs( _dungeon, _random );

         return _dungeon;
      }

      private void AddStairs( Dungeon dungeon, IRandom random )
      {
         bool haveStairsUpBeenBuilt = false;

         while ( !haveStairsUpBeenBuilt )
         {
            int x = random.Next( 0, dungeon.Width - 1 );
            int y = random.Next( 0, dungeon.Height - 1 );

            if ( dungeon.GetCell( x, y ).IsWalkable )
            {
               dungeon.StairUp = new Feature
               {
                  Id = 1,
                  X = x,
                  Y = y
               };
               var entranceArea = dungeon.GetArea( dungeon.GetCell( x, y ) );
               entranceArea.Description = "Dungeon entrance with stairs leading up";
               haveStairsUpBeenBuilt = true;
            }
         }

         GoalMap<DungeonCell> goalMap = new GoalMap<DungeonCell>( dungeon );
         goalMap.AddGoal( dungeon.StairUp.X, dungeon.StairUp.Y, 0 );
         ICell cell = dungeon.GetCell( dungeon.StairUp.X + 1, dungeon.StairUp.Y );
         if ( !cell.IsWalkable )
         {
            cell = dungeon.GetCell( dungeon.StairUp.X - 1, dungeon.StairUp.Y );
         }

         if ( !cell.IsWalkable )
         {
            cell = dungeon.GetCell( dungeon.StairUp.X, dungeon.StairUp.Y + 1 );
         }

         if ( !cell.IsWalkable )
         {
            cell = dungeon.GetCell( dungeon.StairUp.X, dungeon.StairUp.Y - 1 );
         }

         ICell stairDownLocation = goalMap.FindPathAvoidingGoals( cell.X, cell.Y ).End;

         dungeon.StairDown = new Feature
         {
            Id = 2,
            X = stairDownLocation.X,
            Y = stairDownLocation.Y
         };

         var exitArea = dungeon.GetArea( dungeon.GetCell( stairDownLocation.X, stairDownLocation.Y ) );
         exitArea.Description = "Dungeon exit with stairs leading down";
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

      private static void DigRoom( Dungeon dungeon, Rectangle room )
      {
         for ( int x = room.Left; x < room.Right; x++ )
         {
            for ( int y = room.Top; y < room.Bottom; y++ )
            {
               dungeon.SetCellProperties( x, y, true, true );
            }
         }
      }

      private void DigCorridors( Dungeon dungeon )
      {
         for ( int y = 1; y < dungeon.Height - 1; y = y + 2 )
         {
            for ( int x = 1; x < dungeon.Width - 1; x = x + 2 )
            {
               if ( !dungeon.IsWalkable( x, y ) )
               {
                  var corridor = new Corridor
                  {
                     Floors = new List<DungeonCell>(),
                     Description = _featureBuilder.GetCooridorDescription()
                  };
                  dungeon.AddCorridor( corridor );
                  DigCorridors( dungeon, x, y, corridor );
               }
            }
         }
      }

      private static void DigCorridors( Dungeon dungeon, int x, int y, Corridor corridor )
      {
         corridor.Floors.Add( dungeon.GetCell(x, y ) );
         dungeon.SetCellProperties( x, y, true, true );
         // horizontal
         if ( x + 2 < dungeon.Width && !dungeon.IsWalkable( x + 1, y ) && !dungeon.IsWalkable( x + 2, y ) )
         {
            corridor.Floors.Add( dungeon.GetCell( x + 1, y ) );
            dungeon.SetCellProperties( x + 1, y, true, true );
            DigCorridors( dungeon, x + 2, y, corridor );
         }

         // vertical
         if ( y + 2 < dungeon.Height && !dungeon.IsWalkable( x, y + 1 ) && !dungeon.IsWalkable( x, y + 2 ) )
         {
            corridor.Floors.Add( dungeon.GetCell( x, y + 1 ) );
            dungeon.SetCellProperties( x, y + 1, true, true );
            DigCorridors( dungeon, x, y + 2, corridor );
         }
      }

      private void DigEntrances( Dungeon dungeon, IRandom random )
      {
         var uf = new UnionFind( dungeon.AreaCount );

         foreach ( Room room in dungeon.Rooms )
         {
            Rectangle floor = room.Floor;
            while ( dungeon.GetEntrances( room ).Count == 0 )
            {
               var side = (Side) random.Next( 1, 4 );
               switch ( side )
               {
                  case Side.Top:
                  {
                     int x = random.Next( floor.Left, floor.Right - 1 );
                     if ( floor.Top != 1 && dungeon.IsWalkable( x, floor.Top - 2 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( x, floor.Top - 2 ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( x, floor.Top - 1 );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( x, floor.Top - 1, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Left:
                  {
                     int y = random.Next( floor.Top, floor.Bottom - 1 );
                     if ( floor.Left != 1 && dungeon.IsWalkable( floor.Left - 2, y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.Left - 2, y ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.Left - 1, y );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.Left - 1, y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Bottom:
                  {
                     int x = random.Next( floor.Left, floor.Right - 1 );
                     if ( floor.Bottom != dungeon.Height - 1 && dungeon.IsWalkable( x, floor.Bottom + 1 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( x, floor.Bottom + 1 ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( x, floor.Bottom );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( x, floor.Bottom, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Right:
                  {
                     int y = random.Next( floor.Top, floor.Bottom - 1 );
                     if ( floor.Right != dungeon.Width - 1 && dungeon.IsWalkable( floor.Right + 1, y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.Right + 1, y ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.Right, y );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.Right, y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }
               }
            }
         }

         foreach ( Corridor corridor in dungeon.Corridors )
         {
            while ( dungeon.GetEntrances( corridor ).Count == 0 )
            {
               DungeonCell floor = corridor.Floors[random.Next( 0, corridor.Floors.Count - 1 )];
               var side = (Side) random.Next( 1, 4 );
               switch ( side )
               {
                  case Side.Top:
                  {
                     if ( floor.Y != 1 && !dungeon.IsWalkable( floor.X, floor.Y - 1 ) && dungeon.IsWalkable( floor.X, floor.Y - 2 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.X, floor.Y - 2 ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( corridor.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.X, floor.Y - 1 );
                           dungeon.AddEntrance( corridor, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.X, floor.Y - 1, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Left:
                  {
                     if ( floor.X != 1 && !dungeon.IsWalkable( floor.X - 1, floor.Y ) && dungeon.IsWalkable( floor.X - 2, floor.Y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.X - 2, floor.Y ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( corridor.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.X - 1, floor.Y );
                           dungeon.AddEntrance( corridor, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.X - 1, floor.Y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Bottom:
                  {
                     if ( floor.Y != dungeon.Height - 2 && !dungeon.IsWalkable( floor.X, floor.Y + 1 ) && dungeon.IsWalkable( floor.X, floor.Y + 2 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.X, floor.Y + 2 ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( corridor.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.X, floor.Y + 1 );
                           dungeon.AddEntrance( corridor, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.X, floor.Y + 1, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Right:
                  {
                     if ( floor.X != dungeon.Width - 2 && !dungeon.IsWalkable( floor.X + 1, floor.Y ) && dungeon.IsWalkable( floor.X + 2, floor.Y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.X + 2, floor.Y ) );
                        if ( dungeonArea != null )
                        {
                           uf.Union( corridor.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.X + 1, floor.Y );
                           dungeon.AddEntrance( corridor, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.X + 1, floor.Y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }
               }
            }
         }

         // Connect areas that are not connected 
         while ( uf.Count != 1 )
         {
            // get an area at random;
            IDungeonArea area = dungeon.GetArea( random.Next( 0, dungeon.AreaCount - 1 ) );
            if ( area is Room )
            {
               var room = area as Room;
               Rectangle floor = room.Floor;
               var side = (Side) random.Next( 1, 4 );
               switch ( side )
               {
                  case Side.Top:
                  {
                     int x = random.Next( floor.Left, floor.Right - 1 );
                     if ( floor.Top != 1 && dungeon.IsWalkable( x, floor.Top - 2 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( x, floor.Top - 2 ) );
                        if ( dungeonArea != null && !uf.Connected( area.Id, dungeonArea.Id ) )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( x, floor.Top - 1 );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( x, floor.Top - 1, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Left:
                  {
                     int y = random.Next( floor.Top, floor.Bottom - 1 );
                     if ( floor.Left != 1 && dungeon.IsWalkable( floor.Left - 2, y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.Left - 2, y ) );
                        if ( dungeonArea != null && !uf.Connected( area.Id, dungeonArea.Id ) )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.Left - 1, y );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.Left - 1, y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Bottom:
                  {
                     int x = random.Next( floor.Left, floor.Right - 1 );
                     if ( floor.Bottom != dungeon.Height - 1 && dungeon.IsWalkable( x, floor.Bottom + 1 ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( x, floor.Bottom + 1 ) );
                        if ( dungeonArea != null && !uf.Connected( area.Id, dungeonArea.Id ) )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( x, floor.Bottom );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( x, floor.Bottom, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }

                  case Side.Right:
                  {
                     int y = random.Next( floor.Top, floor.Bottom - 1 );
                     if ( floor.Right != dungeon.Width - 1 && dungeon.IsWalkable( floor.Right + 1, y ) )
                     {
                        IDungeonArea dungeonArea = dungeon.GetArea( dungeon.GetCell( floor.Right + 1, y ) );
                        if ( dungeonArea != null && !uf.Connected( area.Id, dungeonArea.Id ) )
                        {
                           uf.Union( room.Id, dungeonArea.Id );
                           Entrance entrance = _entranceBuilder.CreateEntrance( floor.Right, y );
                           dungeon.AddEntrance( room, dungeonArea, entrance );
                           dungeon.SetCellProperties( floor.Right, y, IsTransparent( entrance.EntranceKind ), true );
                        }
                     }

                     break;
                  }
               }
            }
         }
      }

      private static bool IsTransparent( EntranceKind entranceKind )
      {
         switch ( entranceKind )
         {
            case EntranceKind.Empty:
               return true;
            case EntranceKind.DoorClosed:
               return false;
            case EntranceKind.DoorOpen:
               return true;
            default:
               throw new ArgumentOutOfRangeException( "entranceKind" );
         }
      }
   }

   public class EntranceBuilder
   {
      private IRandom _random;
      private Dungeon _dungeon;

      public EntranceBuilder( Dungeon dungeon, IRandom random )
      {
         _dungeon = dungeon;
         _random = random;
      }

      public enum Material
      {
         OakWood = 1,
         PineWood = 2,
         CherryWood = 3,
         Stone = 4,
         RustedIron = 5,
         Iron = 6,
         Steel = 7,
         Crystal = 8,
         Marble = 9
      }

      public Entrance CreateEntrance( int x, int y )
      {
         bool isTrapped = _random.Next( 1, 100 ) <= 20;
         bool isLocked = _random.Next( 1, 100 ) <= 40;
         Material material = (Material) _random.Next( 1, 9 );
         Color color;
         string materialDescription;
         switch ( material )
         {
            case Material.OakWood:
               color = new Color( 85, 39, 0 );
               materialDescription = "oak wood";
               break;
            case Material.PineWood:
               color = new Color( 170, 109, 57 );
               materialDescription = "pine wood";
               break;
            case Material.CherryWood:
               color = new Color( 111, 39, 4 );
               materialDescription = "cherry wood";
               break;
            case Material.Stone:
               color = new Color( 70, 62, 58 );
               materialDescription = "stone";
               break;
            case Material.RustedIron:
               color = new Color( 160, 107, 80 );
               materialDescription = "rusted iron";
               break;
            case Material.Iron:
               color = new Color( 116, 90, 77 );
               materialDescription = "iron";
               break;
            case Material.Steel:
               color = new Color( 37, 35, 34 );
               materialDescription = "steel";
               break;
            case Material.Crystal:
               color = new Color( 54, 63, 81 );
               materialDescription = "crystal";
               break;
            case Material.Marble:
               color = new Color( 23, 29, 41 );
               materialDescription = "marble";
               break;
            default:
               color = new Color( 170, 109, 57 );
               materialDescription = "wood";
               break;
         }

         StringBuilder description = new StringBuilder();
         if ( isLocked )
         {
            description.Append( "A locked" );
         }
         else
         {
            description.Append( "An unlocked" );
         }

         if ( isTrapped )
         {
            description.Append( " and trapped" );
         }

         description.AppendFormat( " {0} door", materialDescription );
         return new Entrance
         {
            Description = description.ToString(),
            EntranceKind = EntranceKind.DoorClosed,
            IsLocked = isLocked,
            IsTrapped = isTrapped,
            X = x,
            Y = y,
            Color = color,
         };
      }
   }

   public class DungeonAreaFeatureBuilder
   {
      private Dungeon _dungeon;
      private IRandom _random;

      private List<string> _roomDescriptions = new List<string>
      {
         "Empty room that smells of dust and mold",
         "Armory with racks of serviceable weapons",
         "Storeroom containing numerous crates and barrels",
         "Kitchen with mysterious meat rotting on the counters",
         "Barracks full sleeping enemies",
         "Musty tomb of a forgotten king",
         "Blacksmith's forge that is cold and unused for some time",
         "Living area with serveral lice-ridden straw beds",
         "Training hall full of enemies practicing for battle",
         "Partially excavated room full of chips of rock and dust",
         "Throne room with a menacing boss",
         "Mirrored room with sparkling torches",
         "Red tapestries cover the walls of this room",
         "A large yellow rug is in the center of this otherwise empty room",
         "High ceiling chamber with intricate stonework",
         "Nothing but spiderwebs and dust",
         "Racks of wine bottles in neat rows",
         "A strange fountain sits in the center of the room",
         "A horrible smell and rubbish in the corners",
         "Several pedestals with interesting art",
         "Mess hall with enemies eating a meal",
         "Gambling hall full of humanoid monsters",
         "A brazier with green magical flame",
         "Multitude of statues",
         "Library with old and mildewed rows of books",
         "Archives with scrolls and books that smell of dust and decay",
         "Greenhouse has light coming from the ceiling and vegtables growing",
         // Duplicated
         "Empty room that smells of dust and mold",
         "Armory with racks of serviceable weapons",
         "Storeroom containing numerous crates and barrels",
         "Kitchen with mysterious meat rotting on the counters",
         "Barracks full sleeping enemies",
         "Musty tomb of a forgotten king",
         "Blacksmith's forge that is cold and unused for some time",
         "Living area with serveral lice-ridden straw beds",
         "Training hall full of enemies practicing for battle",
         "Partially excavated room full of chips of rock and dust",
         "Throne room with a menacing boss",
         "Mirrored room with sparkling torches",
         "Red tapestries cover the walls of this room",
         "A large yellow rug is in the center of this otherwise empty room",
         "High ceiling chamber with intricate stonework",
         "Nothing but spiderwebs and dust",
         "Racks of wine bottles in neat rows",
         "A strange fountain sits in the center of the room",
         "A horrible smell and rubbish in the corners",
         "Several pedestals with interesting art",
         "Mess hall with enemies eating a meal",
         "Gambling hall full of humanoid monsters",
         "A brazier with green magical flame",
         "Multitude of statues",
         "Library with old and mildewed rows of books",
         "Archives with scrolls and books that smell of dust and decay",
         "Greenhouse has light coming from the ceiling and vegtables growing",
         // Duplicated
         "Empty room that smells of dust and mold",
         "Armory with racks of serviceable weapons",
         "Storeroom containing numerous crates and barrels",
         "Kitchen with mysterious meat rotting on the counters",
         "Barracks full sleeping enemies",
         "Musty tomb of a forgotten king",
         "Blacksmith's forge that is cold and unused for some time",
         "Living area with serveral lice-ridden straw beds",
         "Training hall full of enemies practicing for battle",
         "Partially excavated room full of chips of rock and dust",
         "Throne room with a menacing boss",
         "Mirrored room with sparkling torches",
         "Red tapestries cover the walls of this room",
         "A large yellow rug is in the center of this otherwise empty room",
         "High ceiling chamber with intricate stonework",
         "Nothing but spiderwebs and dust",
         "Racks of wine bottles in neat rows",
         "A strange fountain sits in the center of the room",
         "A horrible smell and rubbish in the corners",
         "Several pedestals with interesting art",
         "Mess hall with enemies eating a meal",
         "Gambling hall full of humanoid monsters",
         "A brazier with green magical flame",
         "Multitude of statues",
         "Library with old and mildewed rows of books",
         "Archives with scrolls and books that smell of dust and decay",
         "Greenhouse has light coming from the ceiling and vegtables growing",
         // Duplicated
         "Empty room that smells of dust and mold",
         "Armory with racks of serviceable weapons",
         "Storeroom containing numerous crates and barrels",
         "Kitchen with mysterious meat rotting on the counters",
         "Barracks full sleeping enemies",
         "Musty tomb of a forgotten king",
         "Blacksmith's forge that is cold and unused for some time",
         "Living area with serveral lice-ridden straw beds",
         "Training hall full of enemies practicing for battle",
         "Partially excavated room full of chips of rock and dust",
         "Throne room with a menacing boss",
         "Mirrored room with sparkling torches",
         "Red tapestries cover the walls of this room",
         "A large yellow rug is in the center of this otherwise empty room",
         "High ceiling chamber with intricate stonework",
         "Nothing but spiderwebs and dust",
         "Racks of wine bottles in neat rows",
         "A strange fountain sits in the center of the room",
         "A horrible smell and rubbish in the corners",
         "Several pedestals with interesting art",
         "Mess hall with enemies eating a meal",
         "Gambling hall full of humanoid monsters",
         "A brazier with green magical flame",
         "Multitude of statues",
         "Library with old and mildewed rows of books",
         "Archives with scrolls and books that smell of dust and decay",
         "Greenhouse has light coming from the ceiling and vegtables growing", };

      private List<string> _cooridorDescriptions = new List<string>
      {
         "Everburning Torches",
         "Pit Trap",
         "Gas Trap",
         "Fountain",
         "Casket",
         "Rubble",
         "Vines",
         "Cobwebs",
         "Dust",
         "Under Construction",
         "Empty",
         "Crystal Hall",
         "Red",
         "Yellow",
         "High Ceiling Corridor",
         "Spiderwebs and Dust",
         "Spike Trap",
         "Secret Compartment",
         "Barrels",
         "Litter",
         "Blood Stains",
         "Sewer Grate",
         "Everburning Torches",
         "Strange Smell",
         "Strange Sound",
         "Old Books",
         "Fancy Banners",
         // Duplicated
         "Everburning Torches",
         "Pit Trap",
         "Gas Trap",
         "Fountain",
         "Casket",
         "Rubble",
         "Vines",
         "Cobwebs",
         "Dust",
         "Under Construction",
         "Empty",
         "Crystal Hall",
         "Red",
         "Yellow",
         "High Ceiling Corridor",
         "Spiderwebs and Dust",
         "Spike Trap",
         "Secret Compartment",
         "Barrels",
         "Litter",
         "Blood Stains",
         "Sewer Grate",
         "Everburning Torches",
         "Strange Smell",
         "Strange Sound",
         "Old Books",
         "Fancy Banners",
         // Duplicated
         "Everburning Torches",
         "Pit Trap",
         "Gas Trap",
         "Fountain",
         "Casket",
         "Rubble",
         "Vines",
         "Cobwebs",
         "Dust",
         "Under Construction",
         "Empty",
         "Crystal Hall",
         "Red",
         "Yellow",
         "High Ceiling Corridor",
         "Spiderwebs and Dust",
         "Spike Trap",
         "Secret Compartment",
         "Barrels",
         "Litter",
         "Blood Stains",
         "Sewer Grate",
         "Everburning Torches",
         "Strange Smell",
         "Strange Sound",
         "Old Books",
         "Fancy Banners",
         // Duplicated
         "Everburning Torches",
         "Pit Trap",
         "Gas Trap",
         "Fountain",
         "Casket",
         "Rubble",
         "Vines",
         "Cobwebs",
         "Dust",
         "Under Construction",
         "Empty",
         "Crystal Hall",
         "Red",
         "Yellow",
         "High Ceiling Corridor",
         "Spiderwebs and Dust",
         "Spike Trap",
         "Secret Compartment",
         "Barrels",
         "Litter",
         "Blood Stains",
         "Sewer Grate",
         "Everburning Torches",
         "Strange Smell",
         "Strange Sound",
         "Old Books",
         "Fancy Banners"
      };


      public DungeonAreaFeatureBuilder( Dungeon dungeon, IRandom random )
      {
         _dungeon = dungeon;
         _random = random;
      }

      public string GetRoomDescription()
      {
         int index = _random.Next( 0, _roomDescriptions.Count - 1 );
         string description = _roomDescriptions[index];
         _roomDescriptions.RemoveAt( index );
         return description;
      }

      public string GetCooridorDescription()
      {
         int index = _random.Next( 0, _cooridorDescriptions.Count - 1 );
         string description = _cooridorDescriptions[index];
         _cooridorDescriptions.RemoveAt( index );
         return description;
      }
   }

   #endregion DungeonBuilder
}
