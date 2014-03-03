using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSharp.Algorithms
{
   public class Graph
   {
      private readonly LinkedList<int>[] _adjacent;

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

      public int NumberOfVertices { get; private set; }
      public int NumberOfEdges { get; private set; }

      public void AddEdge( int vertexSource, int vertexDestination )
      {
         NumberOfEdges++;
         _adjacent[vertexSource].AddLast( vertexDestination );
         _adjacent[vertexDestination].AddLast( vertexSource );
      }

      public IEnumerable<int> Adjacent( int vertex )
      {
         return _adjacent[vertex];
      }

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

   public class DepthFirstPaths
   {
      private readonly int[] _edgeTo;
      private readonly bool[] _marked;
      private readonly int _sourceVertex;

      public DepthFirstPaths( Graph graph, int sourceVertex )
      {
         _sourceVertex = sourceVertex;
         _edgeTo = new int[graph.NumberOfVertices];
         _marked = new bool[graph.NumberOfVertices];
         DepthFirstSeach( graph, sourceVertex );
      }

      private void DepthFirstSeach( Graph graph, int sourceVertex )
      {
         _marked[sourceVertex] = true;
         foreach ( int vertex in graph.Adjacent( sourceVertex ) )
         {
            if ( !_marked[vertex] )
            {
               _edgeTo[vertex] = sourceVertex;
               DepthFirstSeach( graph, vertex );
            }
         }
      }

      public bool HasPathTo( int destinationVertex )
      {
         return _marked[destinationVertex];
      }

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

   // http://algs4.cs.princeton.edu/44sp/EdgeWeightedDigraph.java.html
   public class EdgeWeightedDigraph
   {
      private readonly LinkedList<DirectedEdge>[] _adjacent;

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

      public int NumberOfVertices { get; private set; }
      public int NumberOfEdges { get; private set; }

      public void AddEdge( DirectedEdge edge )
      {
         _adjacent[edge.From].AddLast( edge );
      }

      public IEnumerable<DirectedEdge> Adjacent( int vertex )
      {
         return _adjacent[vertex];
      }

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

      public int OutDegree( int vertex )
      {
         return _adjacent[vertex].Count;
      }

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

   public class DirectedEdge
   {
      public DirectedEdge( int from, int to, double weight )
      {
         From = from;
         To = to;
         Weight = weight;
      }

      public int From { get; private set; }
      public int To { get; private set; }
      public double Weight { get; private set; }

      public override string ToString()
      {
         return string.Format( "From: {0}, To: {1}, Weight: {2}", From, To, Weight );
      }
   }

   public class DijkstraShortestPath
   {
      private readonly double[] _distanceTo;
      private readonly DirectedEdge[] _edgeTo;
      private readonly IndexMinPriorityQueue<double> _priorityQueue;

      public DijkstraShortestPath( EdgeWeightedDigraph graph, int sourceVertex )
      {
         foreach ( DirectedEdge edge in graph.Edges() )
         {
            if ( edge.Weight < 0 )
            {
               throw new ArgumentOutOfRangeException( string.Format( "Edge: '{0}' has negative weight", edge ) );
            }
         }

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

      public double DistanceTo( int v )
      {
         return _distanceTo[v];
      }

      public bool HasPathTo( int v )
      {
         return _distanceTo[v] < double.PositiveInfinity;
      }

      public IEnumerable<DirectedEdge> PathTo( int v )
      {
         if ( !HasPathTo( v ) )
         {
            return null;
         }
         var path = new Stack<DirectedEdge>();
         for ( DirectedEdge edge = _edgeTo[v]; edge != null; edge = _edgeTo[edge.From] )
         {
            path.Push( edge );
         }
         return path;
      }

      public bool Check( EdgeWeightedDigraph graph, int sourceVertex )
      {
         if ( _distanceTo[sourceVertex] != 0.0 || _edgeTo[sourceVertex] != null )
         {
            return false;
         }
         for ( int v = 0; v < graph.NumberOfVertices; v++ )
         {
            if ( v == sourceVertex )
            {
               continue;
            }
            if ( _edgeTo[v] == null && _distanceTo[v] != double.PositiveInfinity )
            {
               return false;
            }
         }
         for ( int v = 0; v < graph.NumberOfVertices; v++ )
         {
            foreach ( DirectedEdge edge in graph.Adjacent( v ) )
            {
               int w = edge.To;
               if ( _distanceTo[v] + edge.Weight < _distanceTo[w] )
               {
                  return false;
               }
            }
         }
         for ( int w = 0; w < graph.NumberOfVertices; w++ )
         {
            if ( _edgeTo[w] == null )
            {
               continue;
            }
            DirectedEdge edge = _edgeTo[w];
            int v = edge.From;
            if ( w != edge.To )
            {
               return false;
            }
            if ( _distanceTo[v] + edge.Weight != _distanceTo[w] )
            {
               return false;
            }
         }
         return true;
      }
   }

   // http://algs4.cs.princeton.edu/24pq/IndexMinPQ.java.html
   public class IndexMinPriorityQueue<T> where T : IComparable<T>
   {
      private readonly T[] _keys;
      private readonly int _maxSize;
      private readonly int[] _pq;
      private readonly int[] _qp;

      public IndexMinPriorityQueue( int maxSize )
      {
         _maxSize = maxSize;
         Size = 0;
         _keys = new T[_maxSize + 1];
         _pq = new int[_maxSize + 1];
         _qp = new int[_maxSize + 1];
         for ( int i = 0; i < _maxSize; i++ )
         {
            _qp[i] = -1;
         }
      }

      public int Size { get; private set; }

      public bool IsEmpty()
      {
         return Size == 0;
      }

      public bool Contains( int i )
      {
         return _qp[i] != -1;
      }

      public void Insert( int index, T key )
      {
         Size++;
         _qp[index] = Size;
         _pq[Size] = index;
         _keys[index] = key;
         Swim( Size );
      }

      public int MinIndex()
      {
         return _pq[1];
      }

      public T MinKey()
      {
         return _keys[_pq[1]];
      }

      public int DeleteMin()
      {
         int min = _pq[1];
         Exchange( 1, Size-- );
         Sink( 1 );
         _qp[min] = -1;
         _keys[_pq[Size + 1]] = default( T );
         _pq[Size + 1] = -1;
         return min;
      }

      public T KeyAt( int i )
      {
         return _keys[i];
      }

      public void ChangeKey( int i, T key )
      {
         _keys[i] = key;
         Swim( _qp[i] );
         Sink( _qp[i] );
      }

      public void DecreaseKey( int i, T key )
      {
         _keys[i] = key;
         Swim( _qp[i] );
      }

      public void IncreaseKey( int i, T key )
      {
         _keys[i] = key;
         Sink( _qp[i] );
      }

      public void Delete( int i )
      {
         int index = _qp[i];
         Exchange( index, Size-- );
         Swim( index );
         Sink( index );
         _keys[i] = default( T );
         _qp[i] = -1;
      }

      private bool Greater( int i, int j )
      {
         return _keys[_pq[i]].CompareTo( _keys[_pq[j]] ) > 0;
      }

      private void Exchange( int i, int j )
      {
         int swap = _pq[i];
         _pq[i] = _pq[j];
         _pq[j] = swap;
         _qp[_pq[i]] = i;
         _qp[_pq[j]] = j;
      }

      private void Swim( int k )
      {
         while ( k > 1 && Greater( k / 2, k ) )
         {
            Exchange( k, k / 2 );
            k = k / 2;
         }
      }

      private void Sink( int k )
      {
         while ( 2 * k <= Size )
         {
            int j = 2 * k;
            if ( j < Size && Greater( j, j + 1 ) )
            {
               j++;
            }
            if ( !Greater( k, j ) )
            {
               break;
            }
            Exchange( k, j );
            k = j;
         }
      }
   }
}