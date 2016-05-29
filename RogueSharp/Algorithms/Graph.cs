using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The Graph class represents an undirected graph of vertices named 0 through V - 1.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/41undirected/Graph.java.html">Graph class from Princeton University's Java Algorithms</seealso>
   public class Graph
   {
      private readonly LinkedList<int>[] _adjacent;

      /// <summary>
      /// Constructs a new Graph containing the specified number of vertices and 0 edges
      /// </summary>
      /// <param name="vertices">Number of vertices in the Graph</param>
      public Graph( int vertices )
      {
         NumberOfVertices = vertices;
         NumberOfEdges = 0;
         _adjacent = new LinkedList<int>[vertices];
         for ( int v = 0; v < vertices; v++ )
         {
            _adjacent[v] = new LinkedList<int>();
         }
      }

      /// <summary>
      /// The number of vertices in the Graph
      /// </summary>
      public int NumberOfVertices { get; private set; }

      /// <summary>
      /// The number of edges in the Graph
      /// </summary>
      public int NumberOfEdges { get; private set; }

      /// <summary>
      /// Adds the undirected edge vertexSource-vertexDestination to the Graph
      /// </summary>
      /// <param name="vertexSource">First vertex in the edge</param>
      /// <param name="vertexDestination">Second vertex in the edge</param>
      public void AddEdge( int vertexSource, int vertexDestination )
      {
         NumberOfEdges++;
         _adjacent[vertexSource].AddLast( vertexDestination );
         _adjacent[vertexDestination].AddLast( vertexSource );
      }

      /// <summary>
      /// Gets an IEnumerable of the vertices adjacent to the specified vertex 
      /// </summary>
      /// <param name="vertex">The vertex from which adjacent vertices will be located</param>
      /// <returns>IEnumerable of the vertices adjacent to the specified vertex </returns>
      public IEnumerable<int> Adjacent( int vertex )
      {
         return _adjacent[vertex];
      }

      /// <summary>
      /// Returns a string that represents this Graph
      /// </summary>
      /// <returns>
      /// A string that represents this Graph
      /// </returns>
      public override string ToString()
      {
         var formattedString = new StringBuilder();
         formattedString.AppendFormat( "{0} vertices, {1} edges {2}", NumberOfVertices, NumberOfEdges, Environment.NewLine );
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            formattedString.AppendFormat( "{0}: ", v );
            foreach ( int vertex in _adjacent[v] )
            {
               formattedString.AppendFormat( "{0} ", vertex );
            }
            formattedString.AppendLine();
         }
         return formattedString.ToString();
      }
   }
}