using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class BehaviorTest
   {
      [TestMethod]
      public void Status_AfterContstructing_IsInvalid()
      {
         MockBehavior mockBehavior = new MockBehavior();

         Assert.AreEqual( Status.Invalid, mockBehavior.Status );
      }

      [TestMethod]
      public void IsTerminated_WhenStatusSuccess_WillReturnTrue()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         mockBehavior.Tick();

         Assert.IsTrue( mockBehavior.IsTerminated );
      }

      [TestMethod]
      public void IsTerminated_WhenStatusFailure_WillReturnTrue()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Failure;
         mockBehavior.Tick();

         Assert.IsTrue( mockBehavior.IsTerminated );
      }

      [TestMethod]
      public void IsTerminated_WhenStatusInvalid_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Invalid;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsTerminated );
      }

      [TestMethod]
      public void IsTerminated_WhenStatusRunning_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Running;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsTerminated );
      }

      [TestMethod]
      public void IsTerminated_WhenStatusAborted_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Aborted;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsTerminated );
      }

      [TestMethod]
      public void IsRunning_WhenStatusSuccess_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsRunning );
      }

      [TestMethod]
      public void IsRunning_WhenStatusFailure_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Failure;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsRunning );
      }

      [TestMethod]
      public void IsRunning_WhenStatusInvalid_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Invalid;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsRunning );
      }

      [TestMethod]
      public void IsRunning_WhenStatusRunning_WillReturnTrue()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Running;
         mockBehavior.Tick();

         Assert.IsTrue( mockBehavior.IsRunning );
      }

      [TestMethod]
      public void IsRunning_WhenStatusAborted_WillReturnFalse()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Aborted;
         mockBehavior.Tick();

         Assert.IsFalse( mockBehavior.IsRunning );
      }

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
      public void Tick_CalledOnceWhenStatusRunning_TerminateNeverCalled()
      {
         MockBehavior mockBehavior = new MockBehavior();
         Assert.AreEqual( 0, mockBehavior.TerminateCalled );
         mockBehavior.ReturnStatus = Status.Running;

         Status status = mockBehavior.Tick();

         Assert.AreEqual( status, Status.Running );
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

         Assert.AreEqual( 1, mockBehavior.TerminateCalled );
      }

      [TestMethod]
      public void Tick_WhenBehaviorStatusIsSuccess_WillReturnSuccessStatus()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;

         Status actualStatus = mockBehavior.Tick();

         Assert.AreEqual( Status.Success, actualStatus );
      }

      [TestMethod]
      public void Abort_AfterConstructing_WillSetStatusAborted()
      {
         MockBehavior mockBehavior = new MockBehavior();

         mockBehavior.Abort();

         Assert.AreEqual( Status.Aborted, mockBehavior.Status );
      }

      [TestMethod]
      public void Abort_AfterConstructing_CallTerminateOnce()
      {
         MockBehavior mockBehavior = new MockBehavior();

         mockBehavior.Abort();

         Assert.AreEqual( 1, mockBehavior.TerminateCalled );
      }
   }
}
