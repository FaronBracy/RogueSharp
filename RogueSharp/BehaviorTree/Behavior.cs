namespace RogueSharp.BehaviorTree
{
   public class Behavior
   {
      public Status Status
      {
         get;
         private set;
      }

      public bool IsTerminated
      {
         get
         {
            if ( Status == Status.Success || Status == Status.Failure )
            {
               return true;
            }
            return false;
         }
      }

      public bool IsRunning
      {
         get
         {
            return Status == Status.Running;

         }
      }

      public Behavior()
      {
         Status = Status.Invalid;
      }

      public virtual Status Update()
      {
         return Status;
      }

      public virtual void OnInitialize()
      {
      }

      public virtual void OnTerminate( Status status )
      {
      }

      public Status Tick()
      {
         if ( Status != Status.Running )
         {
            OnInitialize();
         }

         Status = Update();

         if ( Status != Status.Running )
         {
            OnTerminate( Status );
         }

         return Status;
      }

      public void Reset()
      {
         Status = Status.Invalid;
      }

      public void Abort()
      {
         OnTerminate( Status.Aborted );
         Status = Status.Aborted;
      }
   }
}
