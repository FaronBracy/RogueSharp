namespace RogueSharp.BehaviorTree
{
   public class Repeat : Decorator
   {
      public int Limit
      {
         get;
         set;
      }

      public int Counter
      {
         get;
         private set;
      }

      public Repeat( Behavior child )
         : base( child )
      {
      }

      public override Status Update()
      {
         while ( true )
         {
            Status status = Child.Tick();
            if ( status == Status.Running )
            {
               break;
               //return Status.Running;
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

      public override void OnInitialize()
      {
         Counter = 0;
      }
   }
}
