using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class GraphTest
   {
      [TestMethod]
      public void NumberOfVertices_AfterConstructingNewGraphWith10Vertices_WillBe10()
      {
         Graph graph = new Graph( 10 );

         int numberOfVertices = graph.NumberOfVertices;

         Assert.AreEqual( 10, numberOfVertices );
      }

      [TestMethod]
      public void NumberOfEdges_AfterConstructingNewGraphWith10Vertices_WillBe0()
      {
         Graph graph = new Graph( 10 );

         int numberOfEdges = graph.NumberOfEdges;

         Assert.AreEqual( 0, numberOfEdges );
      }
   }
}
