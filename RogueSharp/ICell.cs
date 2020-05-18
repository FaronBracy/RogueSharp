using System;

namespace RogueSharp
{
   /// <summary>
   /// A class that defines a square on a Map with all of its associated properties
   /// </summary>
   public interface ICell : IEquatable<ICell>
   {
      /// <summary>
      /// Gets the X location of the Cell starting with 0 as the farthest left
      /// </summary>
      int X { get; set; }

      /// <summary>
      /// Y location of the Cell starting with 0 as the top
      /// </summary>
      int Y { get; set; }

      /// <summary>
      /// Get the transparency of the Cell i.e. if line of sight would be blocked by this Cell
      /// </summary>
      /// <example>
      /// A Cell representing an empty stone floor would be transparent
      /// A Cell representing a glass wall could be transparent (even though it may not be walkable)
      /// A Cell representing a solid stone wall would not be transparent
      /// </example>
      bool IsTransparent { get; set; }

      /// <summary>
      /// Get the walkability of the Cell i.e. if a character could normally move across the Cell without difficulty
      /// </summary>
      /// <example>
      /// A Cell representing an empty stone floor would be walkable
      /// A Cell representing a glass wall may not be walkable (even though it could be transparent)
      /// A Cell representing a solid stone wall would not be walkable
      /// </example>
      bool IsWalkable { get; set; }
   }
}