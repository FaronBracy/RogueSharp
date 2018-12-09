using System;
using System.Collections.Generic;
using System.Linq;
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

      [TestMethod]
      public void AddEdge_WhenGraphHasNoExistingEdges_GraphWillHave1Edge()
      {
         Graph graph = new Graph( 10 );

         graph.AddEdge( 1, 2 );

         int numberOfEdges = graph.NumberOfEdges;
         Assert.AreEqual( 1, numberOfEdges );
      }

      [TestMethod]
      public void Adjacent_When2EdgesExistBetweenVertices_WillReturnBothAdjacentVertices()
      {
         Graph graph = new Graph( 10 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 1, 3 );

         List<int> adjacentVertices = graph.Adjacent( 1 ).ToList();

         Assert.AreEqual( 2, adjacentVertices.Count );
         Assert.IsTrue( adjacentVertices.Contains( 2 ) );
         Assert.IsTrue( adjacentVertices.Contains( 3 ) );
      }

      [TestMethod]
      public void ToString_WhenGraphHas5VerticesAnd4Edges_WillReturnExpectedStringRepresentationOfGraph()
      {
         Graph graph = new Graph( 5 );
         graph.AddEdge( 0, 1 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 2, 3 );
         graph.AddEdge( 0, 4 );

         string graphString = graph.ToString();

         string[] tokens = graphString.Split( Environment.NewLine );  
         Assert.AreEqual( "5 vertices, 4 edges", tokens[0] );
         Assert.AreEqual( "0: 1 4", tokens[1] );
         Assert.AreEqual( "1: 0 2", tokens[2] );
         Assert.AreEqual( "2: 1 3", tokens[3] );
         Assert.AreEqual( "3: 2", tokens[4] );
         Assert.AreEqual( "4: 0", tokens[5] );
         Assert.AreEqual( 6, tokens.Length );
      }
   }
}
