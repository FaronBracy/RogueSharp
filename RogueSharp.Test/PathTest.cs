using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RogueSharp.Test
{
   [TestClass]
   public class PathTest
   {
      private readonly List<ICell> _pathFromX1Y1ToX1Y4 = new List<ICell>
      {
         new Cell( 1, 1, true, true, true ), 
         new Cell( 1, 2, true, true, true ), 
         new Cell( 1, 3, true, true, true ), 
         new Cell( 1, 4, true, true, true )
      };

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_Null_ThrowsArgumentNullException()
      {
         var path = new Path( null );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentException ) )]
      public void Constructor_EmptyList_ThrowsArgumentException()
      {
         var emptyList = new List<ICell>();

         var path = new Path( emptyList );
      }

      [TestMethod]
      public void Start_PathFromX1Y1ToX1Y4_CellX1Y1()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         Assert.AreEqual( new Cell( 1, 1, true, true, true ), path.Start );
      }

      [TestMethod]
      public void End_PathFromX1Y1ToX1Y4_CellX1Y4()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         Assert.AreEqual( new Cell( 1, 4, true, true, true ), path.End );
      }

      [TestMethod]
      public void Length_PathFromX1Y1ToX1Y4_4()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         Assert.AreEqual( 4, path.Length );
      }

      [TestMethod]
      public void CurrentStep_NewPathFromX1Y1ToX1Y4_CellX1Y1()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         Assert.AreEqual( new Cell( 1, 1, true, true, true ), path.CurrentStep );
      }

      [TestMethod]
      public void StepForward_NewPathFromX1Y1ToX1Y4_CellX1Y2()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         ICell nextStep = path.StepForward();

         Assert.AreEqual( new Cell( 1, 2, true, true, true ), nextStep );
         Assert.AreEqual( new Cell( 1, 2, true, true, true ), path.CurrentStep );
      }

      [TestMethod]
      public void StepForward_ThreeTimesOnNewPathFromX1Y1ToX1Y4_CellX1Y4()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         path.StepForward();
         path.StepForward();
         ICell nextStep = path.StepForward();

         Assert.AreEqual( new Cell( 1, 4, true, true, true ), nextStep );
         Assert.AreEqual( new Cell( 1, 4, true, true, true ), path.CurrentStep );
      }

      [TestMethod]
      [ExpectedException( typeof( NoMoreStepsException ) )]
      public void StepBackward_NewPathFromX1Y1ToX1Y4_NoMoreStepsException()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );

         ICell previousStep = path.StepBackward();
      }

      [TestMethod]
      public void StepBackward_NewPathFromX1Y1ToX1Y4_AfterNoMoreStepsExceptionCurrentStepDoesNotChange()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );
         ICell currentStep = path.CurrentStep;

         try
         {
            path.StepBackward();
         }
         catch ( NoMoreStepsException )
         {
            Assert.AreEqual( currentStep, path.CurrentStep );
         }
      }

      [TestMethod]
      public void Steps_NewPathFromX1Y1ToX1Y4_IteratesThrough4StepsInOrder()
      {
         var path = new Path( _pathFromX1Y1ToX1Y4 );
         int numberOfSteps = 0;
         foreach ( var step in path.Steps )
         {
            numberOfSteps++;
            Assert.AreEqual( numberOfSteps, step.Y );
         }

         Assert.AreEqual( 4, numberOfSteps );
      }
   }
}
