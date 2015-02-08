﻿namespace RogueSharp.Random
 {
    public static class Singleton
    {
       public static readonly DotNetRandom DefaultRandom = new DotNetRandom();
    }
 }
