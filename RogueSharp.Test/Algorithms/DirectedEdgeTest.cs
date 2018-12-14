using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Algorithms;

namespace RogueSharp.Test.Algorithms
{
   [TestClass]
   public class DirectedEdgeTest
   {
      [TestMethod]
      public void From_WhenCreatingDirectedEdgeFrom4To5_WillBe4()
      {
         DirectedEdge directedEdge = new DirectedEdge( 4, 5, 1.12 );

         Assert.AreEqual( 4, directedEdge.From );
      }

      [TestMethod]
      public void To_WhenCreatingDirectedEdgeFrom4To5_WillBe5()
      {
         DirectedEdge directedEdge = new DirectedEdge( 4, 5, 1.12 );

         Assert.AreEqual( 5, directedEdge.To );
      }

      [TestMethod]
      public void Weight_WhenCreatingDirectedEdgeWithWeight1Point12_WillBe1Point12()
      {
         DirectedEdge directedEdge = new DirectedEdge( 4, 5, 1.12 );

         Assert.AreEqual( 1.12, directedEdge.Weight );
      }

      [TestMethod]
      public void ToString_WhenCreatingDirectedEdgeFrom4To5WithWeight1Point12_WillBeExpectedString()
      {
         DirectedEdge directedEdge = new DirectedEdge( 4, 5, 1.12 );

         Assert.AreEqual( "From: 4, To: 5, Weight: 1.12", directedEdge.ToString() );
      }
   }
}
