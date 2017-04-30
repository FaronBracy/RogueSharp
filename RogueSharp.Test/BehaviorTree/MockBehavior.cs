using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   public class MockBehavior : Behavior
   {
      public int InitializeCalled
      {
         get;
         private set;
      }
      public int TerminateCalled
      {
         get;
         private set;
      }
      public int UpdateCalled
      {
         get;
         private set;
      }
      public Status ReturnStatus
      {
         get;
         set;
      }
      public Status TerminateStatus
      {
         get;
         private set;
      }

      public MockBehavior()
      {
         InitializeCalled = 0;
         TerminateCalled = 0;
         UpdateCalled = 0;
         ReturnStatus = Status.Running;
         TerminateStatus = Status.Invalid;
      }

      public override Status Update()
      {
         UpdateCalled++;
         return ReturnStatus;
      }

      public override void OnInitialize()
      {
         InitializeCalled++;
      }

      public override void OnTerminate( Status status )
      {
         TerminateCalled++;
         TerminateStatus = status;
      }
   }
}
