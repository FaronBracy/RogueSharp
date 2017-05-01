using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   public class MockComposite : Composite
   {
      public int ChildrenCount
      {
         get
         {
            return _children.Count;
         }
      }
   }
}
