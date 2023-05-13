using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The EdgeWeightedDigraph class represents an edge-weighted directed graph of vertices named 0 through V-1, where each directed edge
   /// is of type DirectedEdge and has real-valued weight.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/44sp/EdgeWeightedDigraph.java.html">EdgeWeightedDigraph class from Princeton University's Java Algorithms</seealso>
   public class EdgeWeightedDigraph
   {
      private readonly LinkedList<DirectedEdge>[] _edgesOutgoingFrom;
      private readonly LinkedList<DirectedEdge>[] _edgesIncomingTo;

      /// <summary>
      /// Constructs an empty edge-weighted digraph with the specified number of vertices and 0 edges
      /// </summary>
      /// <param name="vertices">Number of vertices in the Graph</param>
      public EdgeWeightedDigraph( int vertices )
      {
         if ( vertices <= 0 )
         {
            throw new ArgumentOutOfRangeException( nameof( vertices ) );
         }
         NumberOfVertices = vertices;
         NumberOfEdges = 0;
         _edgesOutgoingFrom = new LinkedList<DirectedEdge>[NumberOfVertices];
         _edgesIncomingTo = new LinkedList<DirectedEdge>[NumberOfVertices];
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            _edgesOutgoingFrom[v] = new LinkedList<DirectedEdge>();
            _edgesIncomingTo[v] = new LinkedList<DirectedEdge>();
         }
      }

      /// <summary>
      /// The number of vertices in the edge-weighted digraph
      /// </summary>
      public int NumberOfVertices { get; }

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
            throw new ArgumentNullException( nameof( edge ), "DirectedEdge cannot be null" );
         }

         _edgesOutgoingFrom[edge.From].AddLast( edge );
         _edgesIncomingTo[edge.To].AddLast( edge );
         NumberOfEdges++;
      }

      /// <summary>
      /// Returns an IEnumerable of the DirectedEdges incident from the specified vertex
      /// </summary>
      /// <param name="vertex">The vertex to find incident DirectedEdges from</param>
      /// <returns>IEnumerable of the DirectedEdges incident from the specified vertex</returns>
      public IEnumerable<DirectedEdge> EdgesOutgoingFrom( int vertex )
      {
         if ( vertex < 0 || vertex >= NumberOfVertices )
         {
            throw new ArgumentOutOfRangeException( nameof( vertex ) );
         }
         return _edgesOutgoingFrom[vertex];
      }

      /// <summary>
      /// Returns an IEnumerable of the DirectedEdges incident to the specified vertex
      /// </summary>
      /// <param name="vertex">The vertex to find incident DirectedEdges to</param>
      /// <returns>IEnumerable of the DirectedEdges incident to the specified vertex</returns>
      public IEnumerable<DirectedEdge> EdgesIncomingTo( int vertex )
      {
         if ( vertex < 0 || vertex >= NumberOfVertices )
         {
            throw new ArgumentOutOfRangeException( nameof( vertex ) );
         }
         return _edgesIncomingTo[vertex];
      }

      /// <summary>
      /// Returns an IEnumerable of all directed edges in the edge-weighted digraph
      /// </summary>
      /// <returns>IEnumerable of of all directed edges in the edge-weighted digraph</returns>
      public IEnumerable<DirectedEdge> Edges()
      {
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            foreach ( DirectedEdge edge in _edgesOutgoingFrom[v] )
            {
               yield return edge;
            }
         }
      }

      /// <summary>
      /// Returns the number of directed edges incident from the specified vertex
      /// This is known as the out-degree of the vertex
      /// </summary>
      /// <param name="vertex">The vertex to find find the out-degree of</param>
      /// <returns>The number of directed edges incident from the specified vertex</returns>
      public int OutDegree( int vertex )
      {
         if ( vertex < 0 || vertex >= NumberOfVertices )
         {
            throw new ArgumentOutOfRangeException( nameof( vertex ) );
         }
         return _edgesOutgoingFrom[vertex].Count;
      }

      /// <summary>
      /// Returns the number of directed edges incident to the specified vertex
      /// This is known as the in-degree of the vertex
      /// </summary>
      /// <param name="vertex">The vertex to find find the in-degree of</param>
      /// <returns>The number of directed edges incident to the specified vertex</returns>
      public int InDegree( int vertex )
      {
         if ( vertex < 0 || vertex >= NumberOfVertices )
         {
            throw new ArgumentOutOfRangeException( nameof( vertex ) );
         }
         return _edgesIncomingTo[vertex].Count;
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
         formattedString.AppendFormat( CultureInfo.CurrentCulture, "{0} vertices, {1} edges{2}", NumberOfVertices, NumberOfEdges, Environment.NewLine );
         for ( int v = 0; v < NumberOfVertices; v++ )
         {
            formattedString.AppendFormat( CultureInfo.CurrentCulture, "{0}:", v );
            foreach ( DirectedEdge edge in _edgesOutgoingFrom[v] )
            {
               formattedString.AppendFormat( CultureInfo.CurrentCulture, " {0}", edge.To );
            }
            if ( v < NumberOfVertices - 1 )
            {
               formattedString.AppendLine();
            }
         }
         return formattedString.ToString();
      }
   }
}