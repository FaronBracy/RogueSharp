using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class UnionFindTest
   {
      [TestMethod]
      public void UnionFind_WhenConstructorIsCalledWithNegativeNumber_WillThrowArgumentException()
      {
         Assert.ThrowsException<ArgumentException>( () => new UnionFind( -1 ) );
      }

      [TestMethod]
      public void Count_WhenConstructorIsCalledWith5IsolatedSets_WillBe5()
      {
         UnionFind unionFind = new UnionFind( 5 );

         int count = unionFind.Count;

         Assert.AreEqual( 5, count );
      }

      [TestMethod]
      public void Count_WhenUnionIsCalledOn2SetsOf5_WillBe4()
      {
         UnionFind unionFind = new UnionFind( 5 );
         unionFind.Union( 0, 1 );  

         int count = unionFind.Count;

         Assert.AreEqual( 4, count );
      }

      [TestMethod]
      public void Union_WhenCalledWith3SetsOf1ComponentEach_WillUpdateCountTo2()
      {
         UnionFind unionFind = new UnionFind( 3 );

         unionFind.Union( 1, 2 );  

         int count = unionFind.Count;
         Assert.AreEqual( 2, count );
      }

      [TestMethod]
      public void Connected_WhenEachSetHasOnly1Component_WillBeFalse()
      {
         UnionFind unionFind = new UnionFind( 3 );

         bool isConnected = unionFind.Connected( 1, 2 );

         Assert.IsFalse( isConnected );
      }

      [TestMethod]
      public void Connected_WhenUnionIsCalledOnComponents_WillBeTrue()
      {
         UnionFind unionFind = new UnionFind( 3 );
         unionFind.Union( 1, 2 );

         bool isConnected = unionFind.Connected( 1, 2 );

         Assert.IsTrue( isConnected );
      }

      [TestMethod]
      public void Find_WhenCalledForSet3And3SetsExist_WillThrowArgumentOutOfRangeException()
      {
         UnionFind unionFind = new UnionFind( 3 );

         Assert.ThrowsException<ArgumentOutOfRangeException>( () => unionFind.Find( 3 ) );
      }

      [TestMethod]
      public void Find_WhenUnionHasNotBeenCalled_WillBeInOriginalSet()
      {
         UnionFind unionFind = new UnionFind( 3 );

         int set = unionFind.Find( 2 );

         Assert.AreEqual( 2, set );
      }

      [TestMethod]
      public void Find_WhenUnionHasBeenCalled_WillBeInNewSet()
      {
         UnionFind unionFind = new UnionFind( 3 );
         unionFind.Union( 1, 2 );

         int set = unionFind.Find( 2 );

         Assert.AreEqual( 1, set );
      }
   }
}
