namespace RogueSharp.Random
{
   /// <summary>
   /// 
   /// </summary>
   public class Die : IDie
   {
      private readonly IRandom _random;
      private readonly int _sides;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="random"></param>
      /// <param name="sides"></param>
      public Die( IRandom random, int sides )
      {
         _random = random;
         _sides = sides;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public int Roll()
      {
         return _random.Next( 1, _sides );
      }
   }
}