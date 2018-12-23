using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class DepthFirstPathsTest
   {
      [TestMethod]
      public void Constructor_WhenGraphIsNull_WillThrowArgumentException()
      {
         Assert.ThrowsException<ArgumentException>( () => new DepthFirstPaths( null, 3 ) );
      }

      [TestMethod]
      public void HasPathTo_WhenPathExistsBetweenVertices_WillReturnTrue()
      {
         Graph graph = new Graph( 5 );
         graph.AddEdge( 0, 1 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 1, 4 );
         graph.AddEdge( 2, 3 );
         graph.AddEdge( 3, 4 );
         DepthFirstPaths paths = new DepthFirstPaths( graph, 3 );

         Assert.IsTrue( paths.HasPathTo( 0 ) );
      }

      [TestMethod]
      public void HasPathTo_WhenPathDoesNotExistBetweenVertices_WillReturnFalse()
      {
         Graph graph = new Graph( 5 );
         graph.AddEdge( 0, 1 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 1, 4 );
         DepthFirstPaths paths = new DepthFirstPaths( graph, 0 );

         Assert.IsFalse( paths.HasPathTo( 3 ) );
      }

      [TestMethod]
      public void PathTo_WhenPathExistsBetweenVertices_WillReturnVerticesInPath()
      {
         Graph graph = new Graph( 5 );
         graph.AddEdge( 0, 1 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 1, 4 );
         graph.AddEdge( 2, 3 );
         graph.AddEdge( 3, 4 );
         DepthFirstPaths paths = new DepthFirstPaths( graph, 3 );

         int[] pathVertices = paths.PathTo( 0 ).ToArray();

         Assert.AreEqual( pathVertices[0], 2 );
         Assert.AreEqual( pathVertices[1], 1 );
         Assert.AreEqual( pathVertices[2], 0 );
         Assert.AreEqual( pathVertices.Length, 3 );
      }

      [TestMethod]
      public void PathTo_WhenPathDoesNotExistBetweenVertices_WillReturnNull()
      {
         Graph graph = new Graph( 5 );
         graph.AddEdge( 0, 1 );
         graph.AddEdge( 1, 2 );
         graph.AddEdge( 1, 4 );
         DepthFirstPaths paths = new DepthFirstPaths( graph, 0 );

         Assert.IsNull( paths.PathTo( 3 ) );
      }
   }
}
