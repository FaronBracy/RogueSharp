namespace RogueSharp
{
   public class Cell
   {
      public Cell( int x, int y, bool isTransparent, bool isWalkable, bool isInFov, bool isExplored = false )
      {
         X = x;
         Y = y;
         IsTransparent = isTransparent;
         IsWalkable = isWalkable;
         IsInFov = isInFov;
         IsExplored = isExplored;
      }

      public int X { get; private set; }
      public int Y { get; private set; }
      public bool IsTransparent { get; private set; }
      public bool IsWalkable { get; private set; }
      public bool IsInFov { get; private set; }
      public bool IsExplored { get; private set; }

      public override string ToString()
      {
         return ToString( false );
      }

      public string ToString( bool useFov )
      {
         if ( useFov && !IsInFov )
         {
            return "%";
         }
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
}