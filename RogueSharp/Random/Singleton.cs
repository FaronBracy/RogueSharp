﻿namespace RogueSharp.Random
 {
    /// <summary>
    /// The Singleton class is a public static class that holds the DefaultRandom generator.
    /// </summary>
    public static class Singleton
    {
       /// <summary>
       /// The DefaultRandom generator is DotNetRandom from System.Random
       /// </summary>
       /// CA2104 is thrown incorrectly on this line. Singleton.DefaultRandom is immutable.
       public static readonly DotNetRandom DefaultRandom = new DotNetRandom();
    }
 }
