using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class DijkstraShortestPathTest
   {
      [TestMethod]
      public void Constructor_WhenGraphIsNull_WillThrowArgumentNullException()
      {
         Assert.ThrowsException<ArgumentNullException>( () => new DijkstraShortestPath( null, 0 ) );
      }

      [TestMethod]
      public void Constructor_WhenGraphHasEdgesWithNegativeWeights_WillThrowArgumentOutOfRangeException()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 2 );
         digraph.AddEdge( new DirectedEdge( 0, 1, -1.5 ) );

         Assert.ThrowsException<ArgumentOutOfRangeException>( () => new DijkstraShortestPath( digraph, 0 ) );
      }

      [TestMethod]
      public void HasPathTo_WhenPathExistsBetween0And4_WillBeTrue()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 0 );

         Assert.IsTrue( dijkstra.HasPathTo( 4 ) );
      }

      [TestMethod]
      public void HasPathTo_WhenPathDoesNotExistBetween4And0_WillBeFalse()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 4 );

         Assert.IsFalse( dijkstra.HasPathTo( 0 ) );
      }

      [TestMethod]
      public void DistanceTo_WhenPathExistsBetween0And4_WillBeSumOfWeightsInPath()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 0 );

         Assert.AreEqual( 9, dijkstra.DistanceTo( 4 ) );
      }

      [TestMethod]
      public void DistanceTo_WhenMultiplePathsExistBetween0And4_WillBeSumOfWeightsInShortestPath()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 0 );

         Assert.AreEqual( 7.25, dijkstra.DistanceTo( 4 ) );
      }

      [TestMethod]
      public void DistanceTo_WhenNoPathExists_WillBePositiveInfinity()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 4 );

         Assert.AreEqual( double.PositiveInfinity, dijkstra.DistanceTo( 0 ) );
      }

      [TestMethod]
      public void PathTo_WhenMultiplePathsExistBetween0And4_WillBeShortestPath()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 0 );

         DirectedEdge[] path = dijkstra.PathTo( 4 ).ToArray();

         Assert.AreEqual( "From: 0, To: 1, Weight: 2.5", path[0].ToString() );
         Assert.AreEqual( "From: 1, To: 4, Weight: 4.75", path[1].ToString() );
         Assert.AreEqual( 2, path.Length );
      }

      [TestMethod]
      public void PathTo_WhenNoPathExists_WillBeNull()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 4 );

         IEnumerable<DirectedEdge> path = dijkstra.PathTo( 0 );

         Assert.IsNull( path );
      }

      [TestMethod]
      public void FindPath_WhenMultiplePathsExistBetween0And4_WillBeShortestPath()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );

         DirectedEdge[] path = DijkstraShortestPath.FindPath( digraph, 0, 4 ).ToArray();

         Assert.AreEqual( "From: 0, To: 1, Weight: 2.5", path[0].ToString() );
         Assert.AreEqual( "From: 1, To: 4, Weight: 4.75", path[1].ToString() );
         Assert.AreEqual( 2, path.Length );
      }

      [TestMethod]
      public void FindPath_WhenNoPathExists_WillBeNull()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );

         IEnumerable<DirectedEdge> path = DijkstraShortestPath.FindPath( digraph, 4, 0 );

         Assert.IsNull( path );
      }

      [TestMethod]
      public void Check_WhenGivenValidGraph_WillBeTrue()
      {
         EdgeWeightedDigraph digraph = new EdgeWeightedDigraph( 5 );
         digraph.AddEdge( new DirectedEdge( 0, 1, 2.5 ) );
         digraph.AddEdge( new DirectedEdge( 1, 2, 3.25 ) );
         digraph.AddEdge( new DirectedEdge( 1, 4, 4.75 ) );
         digraph.AddEdge( new DirectedEdge( 2, 3, 1.25 ) );
         digraph.AddEdge( new DirectedEdge( 3, 4, 2 ) );
         DijkstraShortestPath dijkstra = new DijkstraShortestPath( digraph, 0 );

         Assert.IsTrue( dijkstra.Check( digraph, 0 ) );
      }
   }
}
