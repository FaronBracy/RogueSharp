using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class UnionFindTest
   {
      [TestMethod]
      public void Count_WhenConstructorIsCalledWith5IsolatedSets_WillBe5()
      {
         UnionFind unionFind = new UnionFind( 5 );

         int count = unionFind.Count;

         Assert.AreEqual( 5, count );
      }
   }
}
