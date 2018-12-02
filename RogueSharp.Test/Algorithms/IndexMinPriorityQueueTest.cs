using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class IndexMinPriorityQueueTest
   {
      [TestMethod]
      public void IsEmpty_AfterConstructingNewQueue_WillBeTrue()
      {
         IndexMinPriorityQueue<double> queue = new IndexMinPriorityQueue<double>( 10 );

         Assert.IsTrue( queue.IsEmpty() );
      }

      [TestMethod]
      public void Size_AfterInserting1Key_WillBe1()
      {
         IndexMinPriorityQueue<double> queue = new IndexMinPriorityQueue<double>( 10 );
         queue.Insert( 4, 12.5 );  

         Assert.AreEqual( 1, queue.Size );
      }
   }
}
