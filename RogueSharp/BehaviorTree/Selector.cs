namespace RogueSharp.BehaviorTree
{
   public class Selector : Composite
   {
      private int _currentIndex;

      protected override void OnInitialize()
      {
         _currentIndex = 0;
      }

      protected override Status Update()
      {
         while ( true )
         {
            Status status = _children[_currentIndex].Tick();

            if ( Status != Status.Failure )
            {
               return status;
            }

            if ( ++_currentIndex >= _children.Count )
            {
               return Status.Success;
            }
         }
      }
   }
}