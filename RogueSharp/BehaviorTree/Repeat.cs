namespace RogueSharp.BehaviorTree
{
   public class Repeat : Decorator
   {
      public int Limit
      {
         get;
         private set;
      }

      public int Counter
      {
         get;
         private set;
      }

      public Repeat( Behavior child, int limit )
         : base( child )
      {
         Limit = limit;
      }

      protected override Status Update()
      {
         while ( true )
         {
            Status status = Child.Tick();
            if ( status == Status.Running )
            {
               break;
            }
            if ( status == Status.Failure )
            {
               return Status.Failure;
            }
            if ( ++Counter == Limit )
            {
               return Status.Success;
            }
            Child.Reset();
         }
         return Status.Invalid;
      }

      protected override void OnInitialize()
      {
         Counter = 0;
      }
   }
}
