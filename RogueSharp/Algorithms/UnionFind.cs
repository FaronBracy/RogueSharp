using System;

namespace RogueSharp.Algorithms
{
   /// <see>
   ///    http://algs4.cs.princeton.edu/15uf/UF.java.html
   ///    http://algs4.cs.princeton.edu/15uf
   ///    Algorithms, 4th Edition by Robert Sedgewick and Kevin Wayne
   /// </see>
   public class UnionFind
   {
      private readonly int[] _id;
      private readonly int[] _size;

      /// <summary>
      ///    Create an empty union-find data structure with "count" isolated sets
      /// </summary>
      /// <param name="count">The number of isolated sets in the data strucutre</param>
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

      public int Count { get; private set; }

      /// <summary>
      ///    Return the id of the component corresponding to object p.
      /// </summary>
      /// <param name="p"></param>
      /// <returns></returns>
      public int Find( int p )
      {
         if ( p < 0 || p >= _id.Length )
         {
            throw new IndexOutOfRangeException();
         }
         while ( p != _id[p] )
         {
            p = _id[p];
         }
         return p;
      }

      /// <summary>
      ///    Are objects p and q in the same set?
      /// </summary>
      /// <param name="p"></param>
      /// <param name="q"></param>
      /// <returns></returns>
      public bool Connected( int p, int q )
      {
         return Find( p ) == Find( q );
      }

      /// <summary>
      ///    Replace sets containing p and q with their union
      /// </summary>
      /// <param name="p"></param>
      /// <param name="q"></param>
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