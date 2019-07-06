using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.MapCreation;

namespace RogueSharp.Test
{
   [TestClass]
   public class DungeonCreationStrategyTest
   {
      [TestMethod]
      public void Create()
      {
         DungeonCreationStrategy strategy = new DungeonCreationStrategy();
         Dungeon dungeon = Map.Create( strategy );
         Trace.Write(dungeon.ToString() );
      }
   }
}
