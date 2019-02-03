using System;
using System.Collections.Generic;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The DepthFirstPaths class represents a data type for finding paths from a source vertex to 
   /// every other vertex in an undirected graph using depth-first search.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/41undirected/DepthFirstPaths.java.html">DepthFirstPaths class from Princeton University's Java Algorithms</seealso>
   public class DepthFirstPaths
   {
      private readonly int[] _edgeTo;
      private readonly bool[] _marked;
      private readonly int _sourceVertex;

      /// <summary>
      /// Computes a path between the specified sourceVertex and every other vertex in the Graph
      /// </summary>
      /// <param name="graph">The Graph</param>
      /// <param name="sourceVertex">The source vertex to compute a path from</param>
      /// <exception cref="ArgumentException">Thrown on null or invalid Graph</exception>
      public DepthFirstPaths( Graph graph, int sourceVertex )
      {
         if ( graph == null || graph.NumberOfVertices < 0 )
         {
            throw new ArgumentException( "Invalid Graph", nameof( graph ) );
         }

         _sourceVertex = sourceVertex;
         _edgeTo = new int[graph.NumberOfVertices];
         _marked = new bool[graph.NumberOfVertices];
         DepthFirstSearch( graph, sourceVertex );
      }

      private void DepthFirstSearch( Graph graph, int sourceVertex )
      {
         _marked[sourceVertex] = true;
         foreach ( int vertex in graph.Adjacent( sourceVertex ) )
         {
            if ( !_marked[vertex] )
            {
               _edgeTo[vertex] = sourceVertex;
               DepthFirstSearch( graph, vertex );
            }
         }
      }

      /// <summary>
      /// Is there a path between the sourceVertex and the specified destinationVertex?
      /// </summary>
      /// <param name="destinationVertex">The destination vertex</param>
      /// <returns>True if there is a path between the sourceVertex and the specified destinationVertex, false otherwise</returns>
      public bool HasPathTo( int destinationVertex )
      {
         return _marked[destinationVertex];
      }

      /// <summary>
      /// Returns a path between the sourceVertex and the specified destinationVertex or null if no such path exists
      /// </summary>
      /// <param name="destinationVertex">The destination vertex</param>
      /// <returns>An IEnumerable sequence of vertices representing the path between the sourceVertex and the specified destinationVertex or null if no such path exists. The path will not include the source vertex</returns>
      public IEnumerable<int> PathTo( int destinationVertex )
      {
         if ( !HasPathTo( destinationVertex ) )
         {
            return null;
         }
         var path = new Stack<int>();
         for ( int x = destinationVertex; x != _sourceVertex; x = _edgeTo[x] )
         {
            path.Push( x );
         }
         return path;
      }
   }
}