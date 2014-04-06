using System.Collections.Generic;
using RogueSharp.Algorithms;

namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public class PathFinder
   {
      private readonly EdgeWeightedDigraph _graph;
      private readonly IMap _map;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="map"></param>
      public PathFinder( IMap map )
      {
         _map = map;
         _graph = new EdgeWeightedDigraph( _map.Width * _map.Height );
         foreach ( Cell cell in _map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               int v = IndexFor( cell );
               foreach ( Cell neighbor in _map.GetBorderCellsInRadius( cell.X, cell.Y, 1 ) )
               {
                  if ( neighbor.IsWalkable )
                  {
                     int w = IndexFor( neighbor );
                     _graph.AddEdge( new DirectedEdge( v, w, 1.0 ) );
                     _graph.AddEdge( new DirectedEdge( w, v, 1.0 ) );
                  }
               }
            }
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="source"></param>
      /// <param name="destination"></param>
      /// <returns></returns>
      public IEnumerable<Cell> ShortestPath( Cell source, Cell destination )
      {
         var dsp = new DijkstraShortestPath( _graph, IndexFor( source ) );
         IEnumerable<DirectedEdge> path = dsp.PathTo( IndexFor( destination ) );
         if ( path == null )
         {
            yield return null;
         }
         else
         {
            foreach ( DirectedEdge edge in path )
            {
               yield return CellFor( edge.To );
            }
         }
      }
      private int IndexFor( Cell cell )
      {
         return ( cell.Y * _map.Width ) + cell.X;
      }
      private Cell CellFor( int index )
      {
         int x = index % _map.Width;
         int y = index / _map.Width;

         return _map.GetCell( x, y );
      }
   }
}