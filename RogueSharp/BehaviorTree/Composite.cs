using System.Collections.Generic;

namespace RogueSharp.BehaviorTree
{
   public class Composite : Behavior
   {
      protected readonly List<Behavior> _children = new List<Behavior>();

      public void AddChild( Behavior behavior )
      {
         _children.Add( behavior );
      }

      public void RemoveChild( Behavior behavior )
      {
         _children.Remove( behavior );
      }

      public void ClearChildren()
      {
         _children.Clear();
      }
   }
}