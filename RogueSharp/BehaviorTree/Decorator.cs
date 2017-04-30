namespace RogueSharp.BehaviorTree
{
   public class Decorator : Behavior
   {
      public Behavior Child
      {
         get;
         private set;
      }

      public Decorator( Behavior child )
      {
         Child = child;
      }
   }
}
