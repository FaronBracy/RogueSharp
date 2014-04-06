namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public class Cell
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="isTransparent"></param>
      /// <param name="isWalkable"></param>
      /// <param name="isInFov"></param>
      /// <param name="isExplored"></param>
      public Cell( int x, int y, bool isTransparent, bool isWalkable, bool isInFov, bool isExplored = false )
      {
         X = x;
         Y = y;
         IsTransparent = isTransparent;
         IsWalkable = isWalkable;
         IsInFov = isInFov;
         IsExplored = isExplored;
      }
      /// <summary>
      /// 
      /// </summary>
      public int X { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public int Y { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public bool IsTransparent { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public bool IsWalkable { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public bool IsInFov { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      public bool IsExplored { get; private set; }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return ToString( false );
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="useFov"></param>
      /// <returns></returns>
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