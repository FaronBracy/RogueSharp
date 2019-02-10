using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class StringGeneratorTest
   {
      [TestMethod]
      public void AddWordPool_WhenWordPoolIsNull_ThrowsArgumentNullException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentNullException>( () => stringGenerator.AddWordPool( "color", null ) );
      }

      [TestMethod]
      public void AddWordPool_WhenPoolKeyAlreadyExists_ThrowsInvalidOperationException()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool1 = new WeightedPool<string>( new DotNetRandom(), x => x );
         colorPool1.Add( "blue", 1 );
         WeightedPool<string> colorPool2 = new WeightedPool<string>( new DotNetRandom(), x => x );
         colorPool2.Add( "red", 1 );
         stringGenerator.AddWordPool( "color", colorPool1 );

         Assert.ThrowsException<InvalidOperationException>( () => stringGenerator.AddWordPool( "color", colorPool2 ) );
      }

      [TestMethod]
      public void Generate_WhenPoolHasOneItem_WillGenerateStringWithReplacedField()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), x => x );
         colorPool.Add( "blue", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color}." );

         Assert.AreEqual( "The room was painted blue.", generated );
      }

      [TestMethod]
      public void Generate_WhenStringHasParameterTwiceButPoolHasOneItem_WillGenerateStringWithBothParametersReplacedBySameString()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), x => x );
         colorPool.Add( "blue", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes." );

         Assert.AreEqual( "The room was painted blue with blue stripes.", generated );
      }

      [TestMethod]
      public void Generate_WhenStringHasParameterTwiceWithKnownSeriesRandom_WillGenerateExpectedString()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new KnownSeriesRandom( 1, 2 ), x => x );
         colorPool.Add( "blue", 1 );
         colorPool.Add( "red", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes." );

         Assert.AreEqual( "The room was painted blue with red stripes.", generated );
      }
   }
}
