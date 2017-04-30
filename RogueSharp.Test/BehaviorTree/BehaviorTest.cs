using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class BehaviorTest
   {
      [TestMethod]
      public void Tick_CalledOnce_InitializeCalledOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.InitializeCalled );

         mockBehavior.Tick();

         Assert.AreEqual( 1, mockBehavior.InitializeCalled );
      }

      [TestMethod]
      public void Tick_CalledOnce_UpdateCalledOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.InitializeCalled );

         mockBehavior.Tick();

         Assert.AreEqual( 1, mockBehavior.UpdateCalled );
      }

      [TestMethod]
      public void Tick_CalledOnceWhenStatusInvalid_TerminateNeverCalled()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.InitializeCalled );

         mockBehavior.Tick();

         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_CalledOnceWhenStatusSuccess_TerminateCalledOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         Assert.AreEqual( 0, mockBehavior.InitializeCalled );

         mockBehavior.Tick();

         Assert.AreEqual( 1, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenStatusInvalid_InitializeCalledOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.Tick();

         Assert.AreEqual( 1, mockBehavior.InitializeCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenStatusSuccess_InitializeCalledTwice()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.Tick();

         Assert.AreEqual( 2, mockBehavior.InitializeCalled );
      }

      [TestMethod]
      public void Tick_CalledTwice_UpdateCalledTwice()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.Tick();

         Assert.AreEqual( 2, mockBehavior.UpdateCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenStatusSuccess_TerminateCalledTwice()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.Tick();

         Assert.AreEqual( 2, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenStatusInvalid_TerminateNeverCalled()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.Tick();

         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_CalledTwiceWhenStatusRunningThenSuccess_TerminateCalledOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Running;
         Assert.AreEqual( 0, mockBehavior.UpdateCalled );

         mockBehavior.Tick();
         mockBehavior.ReturnStatus = Status.Success;
         mockBehavior.Tick();

         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_ReturnStatusSuccess_TerminateCalled()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
         mockBehavior.ReturnStatus = Status.Success;

         mockBehavior.Tick();

         Assert.AreEqual( 1, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_ReturnStatusRunning_TerminateNotCalled()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
         mockBehavior.ReturnStatus = Status.Running;

         Status status = mockBehavior.Tick();

         Assert.AreEqual( status, Status.Running );
         Assert.AreEqual( 1, mockBehavior.TerminateCalled );
      }
   }
}
