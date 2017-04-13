using System;
using System.Collections.Generic;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The DijkstraShortestPathOnly class represents a data type for solving the single-source shortest paths problem
   /// in edge-weighted digraphs where the edge weights are non-negative
   /// Only the path to the destinationVertex is guaranteed to be calculated as an optimisation
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/44sp/DijkstraSP.java.html">DijkstraSP class from Princeton University's Java Algorithms</seealso>
   public class DijkstraShortestPathOnly
   {
      private readonly double[] _distanceTo;
      private readonly DirectedEdge[] _edgeTo;
      private readonly IndexMinPriorityQueue<double> _priorityQueue;
      private readonly int _destinationVertex;

      /// <summary>
      /// Computes a shortest paths tree from the specified sourceVertex to every other vertex in the edge-weighted directed graph
      /// </summary>
      /// <param name="graph">The edge-weighted directed graph</param>
      /// <param name="sourceVertex">The source vertex to compute the shortest paths tree from</param>
      /// <param name="destinationVertex">The destination vertex to compute the shortest paths tree to</param>
      /// <exception cref="ArgumentOutOfRangeException">Throws an ArgumentOutOfRangeException if an edge weight is negative</exception>
      /// <exception cref="ArgumentNullException">Thrown if EdgeWeightedDigraph is null</exception>
      public DijkstraShortestPathOnly( EdgeWeightedDigraph graph, int sourceVertex, int destinationVertex )
      {
         if ( graph == null )
         {
            throw new ArgumentNullException( "graph", "EdgeWeightedDigraph cannot be null" );
         }

         foreach ( DirectedEdge edge in graph.Edges() )
         {
            if ( edge.Weight < 0 )
            {
               throw new ArgumentOutOfRangeException( string.Format( "Edge: '{0}' has negative weight", edge ) );
            }
         }

         _destinationVertex = destinationVertex;

         _distanceTo = new double[graph.NumberOfVertices];
         _edgeTo = new DirectedEdge[graph.NumberOfVertices];
         for ( int v = 0; v < graph.NumberOfVertices; v++ )
         {
            _distanceTo[v] = Double.PositiveInfinity;
         }
         _distanceTo[sourceVertex] = 0.0;

         _priorityQueue = new IndexMinPriorityQueue<double>( graph.NumberOfVertices );
         _priorityQueue.Insert( sourceVertex, _distanceTo[sourceVertex] );
         while ( !_priorityQueue.IsEmpty() )
         {
            int v = _priorityQueue.DeleteMin();

            if ( v == _destinationVertex )
            {
               return;
            }

            foreach ( DirectedEdge edge in graph.Adjacent( v ) )
            {
               Relax( edge );
            }
         }
      }

      private void Relax( DirectedEdge edge )
      {
         int v = edge.From;
         int w = edge.To;
         if ( _distanceTo[w] > _distanceTo[v] + edge.Weight )
         {
            _distanceTo[w] = _distanceTo[v] + edge.Weight;
            _edgeTo[w] = edge;
            if ( _priorityQueue.Contains( w ) )
            {
               _priorityQueue.DecreaseKey( w, _distanceTo[w] );
            }
            else
            {
               _priorityQueue.Insert( w, _distanceTo[w] );
            }
         }
      }

      /// <summary>
      /// Returns the length of a shortest path from the sourceVertex to the destinationVertex
      /// </summary>
      /// <returns>The length of a shortest path from the sourceVertex to the destinationVertex or double.PositiveInfinity if no such path exists</returns>
      public double DistanceToDest( )
      {
         return _distanceTo[_destinationVertex];
      }

      /// <summary>
      /// Is there a path from the sourceVertex to the destinationVertex?
      /// </summary>
      /// <returns>True if there is a path from the sourceVertex to the specified destinationVertex, false otherwise</returns>
      public bool HasPathTo( )
      {
         return _distanceTo[_destinationVertex] < double.PositiveInfinity;
      }

      /// <summary>
      /// Returns an IEnumerable of DirectedEdges representing a shortest path from the sourceVertex to the destinationVertex
      /// </summary>
      /// <returns>IEnumerable of DirectedEdges representing a shortest path from the sourceVertex to the destinationVertex</returns>
      public IEnumerable<DirectedEdge> PathTo( )
      {
         if ( !HasPathTo( ) )
         {
            return null;
         }
         var path = new Stack<DirectedEdge>();
         for ( DirectedEdge edge = _edgeTo[_destinationVertex]; edge != null; edge = _edgeTo[edge.From] )
         {
            path.Push( edge );
         }
         return path;
      }
   }
}