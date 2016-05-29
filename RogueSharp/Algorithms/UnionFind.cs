using System;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The UnionFind class represents a union-find data type also known as the disjoint-sets data type.
   /// It models connectivity among a set of N sites named 0 through N - 1.
   /// </summary>
   /// <seealso href="http://algs4.cs.princeton.edu/15uf/UF.java.html">
   /// UF class from Princeton University's Java Algorithms
   /// </seealso>
   public class UnionFind
   {
      private readonly int[] _id;
      private readonly int[] _size;

      /// <summary>
      /// Create an empty union-find data structure with "count" isolated sets
      /// </summary>
      /// <param name="count">The number of isolated sets in the data structure</param>
      public UnionFind( int count )
      {
         if ( count < 0 )
         {
            throw new ArgumentException( "count must be positive", "count" );
         }
         Count = count;
         _id = new int[count];
         _size = new int[count];
         for ( int i = 0; i < count; i++ )
         {
            _id[i] = i;
            _size[i] = 1;
         }
      }

      /// <summary>
      /// Returns the number of components in this data structure
      /// </summary>
      /// <returns>The number of components in this data structure</returns>
      public int Count { get; private set; }

      /// <summary>
      /// Returns the component identifier of the component containing site p
      /// </summary>
      /// <param name="p">An integer representing one object</param>
      /// <returns>The component identifier of the component containing site p</returns>
      /// <exception cref="ArgumentOutOfRangeException"></exception>
      public int Find( int p )
      {
         if ( p < 0 || p >= _id.Length )
         {
            throw new ArgumentOutOfRangeException( "p", "Index out of bounds" );
         }
         while ( p != _id[p] )
         {
            p = _id[p];
         }
         return p;
      }

      /// <summary>
      /// Are objects p and q in the same set?
      /// </summary>
      /// <param name="p">An integer representing one site</param>
      /// <param name="q">An integer representing the other site</param>
      /// <returns>true if the two sites p and q are in the same component; false otherwise</returns>
      public bool Connected( int p, int q )
      {
         return Find( p ) == Find( q );
      }

      /// <summary>
      /// Merges the component containing site p with the component containing site q
      /// </summary>
      /// <param name="p">An integer representing one site</param>
      /// <param name="q">An integer representing the other site</param>
      public void Union( int p, int q )
      {
         int i = Find( p );
         int j = Find( q );
         if ( i == j )
         {
            return;
         }
         if ( _size[i] < _size[j] )
         {
            _id[i] = j;
            _size[j] += _size[i];
         }
         else
         {
            _id[j] = i;
            _size[i] += _size[j];
         }
         Count--;
      }
   }
}