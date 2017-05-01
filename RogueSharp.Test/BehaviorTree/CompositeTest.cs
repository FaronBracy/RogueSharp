using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class CompositeTest
   {
      [TestMethod]
      public void AddChild_WhenStartingWithNoChildren_WillHaveOneChild()
      {
         MockComposite mockComposite = new MockComposite();

         mockComposite.AddChild( new MockBehavior() );

         Assert.AreEqual( 1, mockComposite.ChildrenCount );
      }

      [TestMethod]
      public void AddChild_WhenCalledTwice_WillHaveTwoChildren()
      {
         MockComposite mockComposite = new MockComposite();

         mockComposite.AddChild( new MockBehavior() );
         mockComposite.AddChild( new MockBehavior() );  

         Assert.AreEqual( 2, mockComposite.ChildrenCount );
      }

      [TestMethod]
      public void RemoveChild_WhenStartingWithNoChildren_WillHaveNoChildren()
      {
         MockComposite mockComposite = new MockComposite();

         mockComposite.RemoveChild( new MockBehavior() );

         Assert.AreEqual( 0, mockComposite.ChildrenCount );
      }

      [TestMethod]
      public void RemoveChild_WhenStartingWithTwoChildren_WillHaveOneChild()
      {
         MockBehavior mockBehaviorToRemove = new MockBehavior();
         MockComposite mockComposite = new MockComposite();
         mockComposite.AddChild( mockBehaviorToRemove );
         mockComposite.AddChild( new MockBehavior() );

         mockComposite.RemoveChild( mockBehaviorToRemove );

         Assert.AreEqual( 1, mockComposite.ChildrenCount );
      }

      [TestMethod]
      public void ClearChildren_WhenStartingWithTwoChildren_WillHaveNoChildren()
      {
         MockComposite mockComposite = new MockComposite();
         mockComposite.AddChild( new MockBehavior() );
         mockComposite.AddChild( new MockBehavior() );

         mockComposite.ClearChildren();

         Assert.AreEqual( 0, mockComposite.ChildrenCount );
      }
   }
}
