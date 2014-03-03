namespace RogueSharp.Random
{
   public class Die : IDie
   {
      private readonly IRandom _random;
      private readonly int _sides;

      public Die( IRandom random, int sides )
      {
         _random = random;
         _sides = sides;
      }

      public int Roll()
      {
         return _random.Next( 1, _sides );
      }
   }
}