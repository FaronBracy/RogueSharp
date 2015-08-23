using System;
using System.Collections.Generic;

namespace RogueSharp
{
   /// <summary>
   /// 
   /// </summary>
   public class Path
   {
      private readonly LinkedList<Cell> _steps;
      private LinkedListNode<Cell> _currentStep;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="steps"></param>
      /// <exception cref="Exception"></exception>
      public Path( IEnumerable<Cell> steps )
      {
         _steps = new LinkedList<Cell>( steps );

         if ( _steps.Count < 1 )
         {
            throw new ArgumentException( "Path must have steps", "steps" );
         }

         _currentStep = _steps.First;
      }
      /// <summary>
      /// 
      /// </summary>
      public Cell Start
      {
         get
         {
            return _steps.First.Value;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      public Cell End
      {
         get
         {
            return _steps.Last.Value;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      public int Length
      {
         get
         {
            return _steps.Count;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      public Cell CurrentStep
      {
         get
         {
            return _currentStep.Value;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      public IEnumerable<Cell> Steps
      {
         get
         {
            return _steps;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public Cell StepForward()
      {
         LinkedListNode<Cell> nextStep = _currentStep.Next;
         if ( nextStep == null )
         {
            throw new NoMoreStepsException( "Cannot take a step foward when at the end of the path" );
         }
         _currentStep = nextStep;
         return nextStep.Value;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public Cell TakeStepBackward()
      {
         LinkedListNode<Cell> previousStep = _currentStep.Previous;
         if ( previousStep == null )
         {
            throw new NoMoreStepsException( "Cannot take a step backward when at the start of the path" );
         }
         _currentStep = previousStep;
         return previousStep.Value;
      }
   }
   /// <summary>
   /// 
   /// </summary>
   public class NoMoreStepsException : Exception
   {
      /// <summary>
      /// 
      /// </summary>
      public NoMoreStepsException()
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      public NoMoreStepsException( string message )
         : base( message )
      {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      /// <param name="inner"></param>
      public NoMoreStepsException( string message, Exception inner )
         : base( message, inner )
      {
      }
   }
}

