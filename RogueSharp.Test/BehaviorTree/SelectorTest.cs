using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class SelectorTest
   {
      [TestMethod]
      public void Tick_WhenSelectorHasTwoChildrenReturningSuccess_WillOnlyTickFirstChildAndReturnSuccess()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Success;
         mockBehavior2.ReturnStatus = Status.Success;
         Selector selector = new Selector();
         selector.AddChild( mockBehavior1 );
         selector.AddChild( mockBehavior2 );

         Status actualStatus = selector.Tick();

         Assert.AreEqual( Status.Success, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 0, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 0, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 0, mockBehavior2.TerminateCalled );
      }

      [TestMethod]
      public void Tick_WhenSelectorHasTwoChildrenFirstReturningFailureAndSecondReturningSuccess_WillReturnSuccess()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Failure;
         mockBehavior2.ReturnStatus = Status.Success;
         Selector selector = new Selector();
         selector.AddChild( mockBehavior1 );
         selector.AddChild( mockBehavior2 );

         Status actualStatus = selector.Tick();

         Assert.AreEqual( Status.Success, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior2.TerminateCalled );
      }

      [TestMethod]
      public void Tick_WhenSelectorHasTwoChildrenAndBothReturnFailure_WillReturnFailure()
      {
         MockBehavior mockBehavior1 = new MockBehavior();
         MockBehavior mockBehavior2 = new MockBehavior();
         mockBehavior1.ReturnStatus = Status.Failure;
         mockBehavior2.ReturnStatus = Status.Failure;
         Selector selector = new Selector();
         selector.AddChild( mockBehavior1 );
         selector.AddChild( mockBehavior2 );

         Status actualStatus = selector.Tick();

         Assert.AreEqual( Status.Failure, actualStatus );
         Assert.AreEqual( 1, mockBehavior1.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior1.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior1.TerminateCalled );
         Assert.AreEqual( 1, mockBehavior2.InitializeCalled );
         Assert.AreEqual( 1, mockBehavior2.UpdateCalled );
         Assert.AreEqual( 1, mockBehavior2.TerminateCalled );
      }
   }
}
