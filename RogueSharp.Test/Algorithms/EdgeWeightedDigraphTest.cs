using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class EdgeWeightedDigraphTest
   {
      [TestMethod]
      public void NumberOfVertices_WhenConstructingNewGraphWith5Vertices_WillBe5()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         Assert.AreEqual( 5, graph.NumberOfVertices );
      }

      [TestMethod]
      public void NumberOfEdges_WhenConstructingNewGraphWith5Vertices_WillBe0()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         Assert.AreEqual( 0, graph.NumberOfEdges );
      }

      [TestMethod]
      public void AddEdge_WhenGraphHad0Edges_WillNowHave1Edge()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );  

         Assert.AreEqual( 1, graph.NumberOfEdges );
      }

      [TestMethod]
      public void AddEdge_6TimesWhenGraphHad0Edges_WillNowHave6Edges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 0, 2, 2.0 ) );
         graph.AddEdge( new DirectedEdge( 0, 4, 4.0 ) );
         graph.AddEdge( new DirectedEdge( 3, 4, 5.5 ) );
         graph.AddEdge( new DirectedEdge( 3, 0, 4.0 ) );

         Assert.AreEqual( 6, graph.NumberOfEdges );
      }

      [TestMethod]
      public void AddEdge_WhenEdgeToAddIsNull_WillThrowArgumentNullException()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         Assert.ThrowsException<ArgumentNullException>( () => graph.AddEdge( null ) );
      }
   }
}
