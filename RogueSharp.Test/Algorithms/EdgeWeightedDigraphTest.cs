using System;
using System.Linq;
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

      [TestMethod]
      public void EdgesOutgoingFrom_WhenVertexHasNoOutgoingEdges_WillReturn0DirectedEdges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         Assert.AreEqual( 0, graph.EdgesOutgoingFrom( 1 ).Count() );
      }

      [TestMethod]
      public void EdgesOutgoingFrom_WhenVertexHas3OutgoingEdges_WillReturn3DirectedEdges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.5 ) );
         graph.AddEdge( new DirectedEdge( 1, 4, 2.0 ) );

         DirectedEdge[] outgoing = graph.EdgesOutgoingFrom( 1 ).ToArray();

         Assert.AreEqual( 3, outgoing.Length );
         Assert.AreEqual( 2, outgoing[0].To );
         Assert.AreEqual( 1, outgoing[0].From );
         Assert.AreEqual( 1.0, outgoing[0].Weight );
         Assert.AreEqual( 3, outgoing[1].To );
         Assert.AreEqual( 1, outgoing[1].From );
         Assert.AreEqual( 1.5, outgoing[1].Weight );
         Assert.AreEqual( 4, outgoing[2].To );
         Assert.AreEqual( 1, outgoing[2].From );
         Assert.AreEqual( 2.0, outgoing[2].Weight );
      }

      [TestMethod]
      public void EdgesIncomingTo_WhenVertexHasNoIncomingEdges_WillReturn0DirectedEdges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );

         Assert.AreEqual( 0, graph.EdgesIncomingTo( 1 ).Count() );
      }

      [TestMethod]
      public void EdgesIncomingTo_WhenVertexHas2IncomingEdges_WillReturn2DirectedEdges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.5 ) );
         graph.AddEdge( new DirectedEdge( 1, 4, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 3, 4, 5.0 ) );

         DirectedEdge[] incoming = graph.EdgesIncomingTo( 4 ).ToArray();

         Assert.AreEqual( 2, incoming.Length );
         Assert.AreEqual( 4, incoming[0].To );
         Assert.AreEqual( 1, incoming[0].From );
         Assert.AreEqual( 1.0, incoming[0].Weight );
         Assert.AreEqual( 4, incoming[1].To );
         Assert.AreEqual( 3, incoming[1].From );
         Assert.AreEqual( 5.0, incoming[1].Weight );
      }

      [TestMethod]
      public void Edges_WhenGraphHas6Edges_WillReturnAll6DirectedEdges()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 0, 2, 2.0 ) );
         graph.AddEdge( new DirectedEdge( 0, 4, 4.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 3, 4, 5.5 ) );
         graph.AddEdge( new DirectedEdge( 3, 0, 4.0 ) );

         DirectedEdge[] edges = graph.Edges().ToArray();

         Assert.AreEqual( 6, edges.Length );
         Assert.AreEqual( "From: 0, To: 2, Weight: 2", edges[0].ToString() );
         Assert.AreEqual( "From: 0, To: 4, Weight: 4", edges[1].ToString() );
         Assert.AreEqual( "From: 1, To: 2, Weight: 1", edges[2].ToString() );
         Assert.AreEqual( "From: 1, To: 3, Weight: 1", edges[3].ToString() );
         Assert.AreEqual( "From: 3, To: 4, Weight: 5.5", edges[4].ToString() );
         Assert.AreEqual( "From: 3, To: 0, Weight: 4", edges[5].ToString() );
      }

      [TestMethod]
      public void OutDegree_WhenVertexHas3EdgesToOtherVertices_WillBe3()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.5 ) );
         graph.AddEdge( new DirectedEdge( 1, 4, 1.0 ) );

         Assert.AreEqual( 3, graph.OutDegree( 1 ) );
      }

      [TestMethod]
      public void InDegree_WhenVertexHas2EdgesFromOtherVertices_WillBe2()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.5 ) );
         graph.AddEdge( new DirectedEdge( 1, 4, 4.0 ) );
         graph.AddEdge( new DirectedEdge( 3, 4, 2.0 ) );

         Assert.AreEqual( 2, graph.InDegree( 4 ) );
      }

      [TestMethod]
      public void ToString_WhenGraphHasMultipleVerticesAndEdges_WillReturnExceptedString()
      {
         EdgeWeightedDigraph graph = new EdgeWeightedDigraph( 5 );
         graph.AddEdge( new DirectedEdge( 0, 2, 2.0 ) );
         graph.AddEdge( new DirectedEdge( 0, 4, 4.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 2, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 1, 3, 1.0 ) );
         graph.AddEdge( new DirectedEdge( 2, 0, 3.75 ) );
         graph.AddEdge( new DirectedEdge( 3, 4, 5.5 ) );
         graph.AddEdge( new DirectedEdge( 3, 0, 4.0 ) );

         string[] tokens = graph.ToString().Split( Environment.NewLine );

         Assert.AreEqual( "5 vertices, 7 edges", tokens[0] );
         Assert.AreEqual( "0: 2 4", tokens[1] );
         Assert.AreEqual( "1: 2 3", tokens[2] );
         Assert.AreEqual( "2: 0", tokens[3] );
         Assert.AreEqual( "3: 4 0", tokens[4] );
         Assert.AreEqual( "4:", tokens[5] );
         Assert.AreEqual( 6, tokens.Length );
      }
   }
}
