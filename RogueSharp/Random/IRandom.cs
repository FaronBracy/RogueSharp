namespace RogueSharp.Random
{
   public interface IRandom
   {
      /// <summary>
      /// Returns a pseudo random integer between 0 and maxValue inclusive
      /// </summary>
      /// <param name="maxValue">Inclusive Maximum Result</param>
      /// <returns></returns>
      int Next( int maxValue );

      /// <summary>
      /// Returns a pseudo random integer between minValue and maxValue inclusive
      /// </summary>
      /// <param name="minValue">Inclusive Minimum Result</param>
      /// <param name="maxValue">Inclusive Maximum Result</param>
      /// <returns></returns>
      int Next( int minValue, int maxValue );
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      RandomState Save();
      /// <summary>
      /// 
      /// </summary>
      /// <param name="state"></param>
      void Restore( RandomState state );
   }

   /// <summary>
   /// 
   /// </summary>
   public class RandomState
   {
      /// <summary>
      /// 
      /// </summary>
      public int[] Seed { get; set; }
      /// <summary>
      /// 
      /// </summary>
      public long NumberGenerated { get; set; }
   }
}