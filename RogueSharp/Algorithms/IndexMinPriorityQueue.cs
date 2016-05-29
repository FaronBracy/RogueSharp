using System;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The IndexMinPriorityQueue class represents an indexed priority queue of generic keys.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/24pq/IndexMinPQ.java.html">IndexMinPQ class from Princeton University's Java Algorithms</seealso>
   /// <typeparam name="T">Type must implement IComparable interface</typeparam>
   public class IndexMinPriorityQueue<T> where T : IComparable<T>
   {
      private readonly T[] _keys;
      private readonly int _maxSize;
      private readonly int[] _pq;
      private readonly int[] _qp;

      /// <summary>
      /// Constructs an empty indexed priority queue with indices between 0 and the specified maxSize - 1
      /// </summary>
      /// <param name="maxSize">The maximum size of the indexed priority queue</param>
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

      /// <summary>
      /// The number of keys on this indexed priority queue
      /// </summary>
      public int Size { get; private set; }

      /// <summary>
      /// Is the indexed priority queue empty?
      /// </summary>
      /// <returns>True if the indexed priority queue is empty, false otherwise</returns>
      public bool IsEmpty()
      {
         return Size == 0;
      }

      /// <summary>
      /// Is the specified parameter i an index on the priority queue?
      /// </summary>
      /// <param name="i">An index to check for on the priority queue</param>
      /// <returns>True if the specified parameter i is an index on the priority queue, false otherwise</returns>
      public bool Contains( int i )
      {
         return _qp[i] != -1;
      }

      /// <summary>
      /// Associates the specified key with the specified index
      /// </summary>
      /// <param name="index">The index to associate the key with</param>
      /// <param name="key">The key to associate with the index</param>
      public void Insert( int index, T key )
      {
         Size++;
         _qp[index] = Size;
         _pq[Size] = index;
         _keys[index] = key;
         Swim( Size );
      }

      /// <summary>
      /// Returns an index associated with a minimum key
      /// </summary>
      /// <returns>An index associated with a minimum key</returns>
      public int MinIndex()
      {
         return _pq[1];
      }

      /// <summary>
      /// Returns a minimum key
      /// </summary>
      /// <returns>A minimum key</returns>
      public T MinKey()
      {
         return _keys[_pq[1]];
      }

      /// <summary>
      /// Removes a minimum key and returns its associated index
      /// </summary>
      /// <returns>An index associated with a minimum key that was removed</returns>
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

      /// <summary>
      /// Returns the key associated with the specified index
      /// </summary>
      /// <param name="index">The index of the key to return</param>
      /// <returns>The key associated with the specified index</returns>
      public T KeyAt( int index )
      {
         return _keys[index];
      }

      /// <summary>
      /// Change the key associated with the specified index to the specified value
      /// </summary>
      /// <param name="index">The index of the key to change</param>
      /// <param name="key">Change the key associated with the specified index to this key</param>
      public void ChangeKey( int index, T key )
      {
         _keys[index] = key;
         Swim( _qp[index] );
         Sink( _qp[index] );
      }

      /// <summary>
      /// Decrease the key associated with the specified index to the specified value
      /// </summary>
      /// <param name="index">The index of the key to decrease</param>
      /// <param name="key">Decrease the key associated with the specified index to this key</param>
      public void DecreaseKey( int index, T key )
      {
         _keys[index] = key;
         Swim( _qp[index] );
      }

      /// <summary>
      /// Increase the key associated with the specified index to the specified value
      /// </summary>
      /// <param name="index">The index of the key to increase</param>
      /// <param name="key">Increase the key associated with the specified index to this key</param>
      public void IncreaseKey( int index, T key )
      {
         _keys[index] = key;
         Sink( _qp[index] );
      }

      /// <summary>
      /// Remove the key associated with the specified index
      /// </summary>
      /// <param name="index">The index of the key to remove</param>
      public void Delete( int index )
      {
         int i = _qp[index];
         Exchange( i, Size-- );
         Swim( i );
         Sink( i );
         _keys[index] = default( T );
         _qp[index] = -1;
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