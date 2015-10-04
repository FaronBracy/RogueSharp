using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The EdgeWeightedDigrpah class represents an edge-weighted directed graph of vertices named 0 through V-1, where each directed edge
   /// is of type DirectedEdge and has real-valued weight.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/44sp/EdgeWeightedDigraph.java.html">EdgeWeightedDigraph class from Princeton University's Java Algorithms</seealso>
   public class EdgeWeightedDigraph
   {
      private readonly LinkedList<DirectedEdge>[] _adjacent;

      /// <summary>
      /// Constructs an empty edge-weighted digraph with the specified number of vertices and 0 edges
      /// </summary>
      /// <param name="vertices">Number of vertices in the Graph</param>
      public EdgeWeightedDigraph( int vertices )
      {
         NumberOfVertices = vertices;
         NumberOfEdges = 0;
         _adjacent = new LinkedList<DirectedEdge>[NumberOfVertices];
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            _adjacent[v] = new LinkedList<DirectedEdge>();
         }
      }

      /// <summary>
      /// The number of vertices in the edge-weighted digraph
      /// </summary>
      public int NumberOfVertices { get; private set; }

      /// <summary>
      /// The number of edges in the edge-weighted digraph
      /// </summary>
      public int NumberOfEdges { get; private set; }

      /// <summary>
      /// Adds the specified directed edge to the edge-weighted digraph
      /// </summary>
      /// <param name="edge">The DirectedEdge to add</param>
      /// <exception cref="ArgumentNullException">DirectedEdge cannot be null</exception>
      public void AddEdge( DirectedEdge edge )
      {
         if ( edge == null )
         {
            throw new ArgumentNullException( "edge", "DirectedEdge cannot be null" );
         }

         _adjacent[edge.From].AddLast( edge );
      }

      /// <summary>
      /// Returns an IEnumerable of the DirectedEdges incident from the specified vertex
      /// </summary>
      /// <param name="vertex">The vertex to find incident DirectedEdges from</param>
      /// <returns>IEnumerable of the DirectedEdges incident from the specified vertex</returns>
      public IEnumerable<DirectedEdge> Adjacent( int vertex )
      {
         return _adjacent[vertex];
      }

      /// <summary>
      /// Returns an IEnumerable of all directed edges in the edge-weighted digraph
      /// </summary>
      /// <returns>IEnumerable of of all directed edges in the edge-weighted digraph</returns>
      public IEnumerable<DirectedEdge> Edges()
      {
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            foreach ( DirectedEdge edge in _adjacent[v] )
            {
               yield return edge;
            }
         }
      }

      /// <summary>
      /// Returns the number of directed edges incident from the specified vertex
      /// This is known as the outdegree of the vertex
      /// </summary>
      /// <param name="vertex">The vertex to find find the outdegree of</param>
      /// <returns>The number of directed edges incident from the specified vertex</returns>
      public int OutDegree( int vertex )
      {
         return _adjacent[vertex].Count;
      }

      /// <summary>
      /// Returns a string that represents the current edge-weighted digraph
      /// </summary>
      /// <returns>
      /// A string that represents the current edge-weighted digraph
      /// </returns>
      public override string ToString()
      {
         var formattedString = new StringBuilder();
         formattedString.AppendFormat( "{0} vertices, {1} edges {2}", NumberOfVertices, NumberOfEdges, Environment.NewLine );
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            formattedString.AppendFormat( "{0}: ", v );
            foreach ( DirectedEdge edge in _adjacent[v] )
            {
               formattedString.AppendFormat( "{0} ", edge.To );
            }
            formattedString.AppendLine();
         }
         return formattedString.ToString();
      }
   }
}