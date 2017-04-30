using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.BehaviorTree;

namespace RogueSharp.Test.BehaviorTree
{
   [TestClass]
   public class RepeatTest
   {
      [TestMethod]
      public void Tick_RepeatWithLimit3_MethodsCalledExpectedNumberOfTimes()
      {
         MockBehavior mockBehavior = new MockBehavior();
         mockBehavior.ReturnStatus = Status.Success;
         Repeat repeat = new Repeat( mockBehavior );
         repeat.Limit = 3;

         Status status = repeat.Tick();
         Assert.AreEqual( 3, repeat.Counter );
         Assert.AreEqual( 3, mockBehavior.InitializeCalled );
         Assert.AreEqual( 3, mockBehavior.UpdateCalled );
         Assert.AreEqual( 3, mockBehavior.TerminateCalled );
      }
   }
}
