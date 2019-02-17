using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RogueSharp.Random;

namespace RogueSharp.Test.Random
{
   [TestClass]
   public class StringGeneratorTest
   {
      [TestMethod]
      public void AddWordsToPool_WhenPoolNameIsNull_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordsToPool( null, "red" ) );
      }

      [TestMethod]
      public void AddWordsToPool_WhenPoolNameIsWhiteSpace_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordsToPool( "  ", "red" ) );
      }

      [TestMethod]
      public void AddWordsToPool_WhenWordsParamIsNull_WillThrowArgumentNullException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentNullException>( () => stringGenerator.AddWordsToPool( "color", null ) );
      }

      [TestMethod]
      public void AddWordsToPool_When3WordsAddedWithKnownSeriesRandom_WillUseAll3Words()
      {
         StringGenerator stringGenerator = new StringGenerator( new KnownSeriesRandom( 1, 2, 3 ) );

         stringGenerator.AddWordsToPool( "color", "red", "green", "blue" );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes and {color} spots." );
         Assert.AreEqual( "The room was painted red with green stripes and blue spots.", generated );
      }

      [TestMethod]
      public void AddWordsToPool_WhenPoolAlreadyExists_WillExpandPoolWithNewWords()
      {
         StringGenerator stringGenerator = new StringGenerator( new KnownSeriesRandom( 1, 2, 3 ) );

         stringGenerator.AddWordsToPool( "color", "red" );
         stringGenerator.AddWordsToPool( "color", "green", "blue" );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes and {color} spots." );
         Assert.AreEqual( "The room was painted red with green stripes and blue spots.", generated );
      }

      [TestMethod]
      public void AddWordsToPool_UsingEnumerableWhenPoolNameIsNull_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordsToPool( null, new List<string> { "red" } ) );
      }

      [TestMethod]
      public void AddWordsToPool_UsingEnumerableWhenPoolNameIsWhiteSpace_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordsToPool( "  ", new List<string> { "red" } ) );
      }

      [TestMethod]
      public void AddWordsToPool_UsingEnumerableWhenWordsParamIsNull_WillThrowArgumentNullException()
      {
         List<string> words = null;
         StringGenerator stringGenerator = new StringGenerator();

         Assert.ThrowsException<ArgumentNullException>( () => stringGenerator.AddWordsToPool( "color", words ) );
      }

      [TestMethod]
      public void AddWordsToPool_UsingEnumerableWhen3WordsAddedWithKnownSeriesRandom_WillUseAll3Words()
      {
         StringGenerator stringGenerator = new StringGenerator( new KnownSeriesRandom( 1, 2, 3 ) );

         stringGenerator.AddWordsToPool( "color", new List<string> { "red", "green", "blue" } );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes and {color} spots." );
         Assert.AreEqual( "The room was painted red with green stripes and blue spots.", generated );
      }

      [TestMethod]
      public void AddWordPool_WhenPoolNameIsNull_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool.Add( "blue", 1 );

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordPool( null, colorPool ) );
      }

      [TestMethod]
      public void AddWordPool_WhenPoolNameIsWhiteSpace_WillThrowArgumentException()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool.Add( "blue", 1 );

         Assert.ThrowsException<ArgumentException>( () => stringGenerator.AddWordPool( "  ", colorPool ) );
      }

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
         WeightedPool<string> colorPool1 = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool1.Add( "blue", 1 );
         WeightedPool<string> colorPool2 = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool2.Add( "red", 1 );
         stringGenerator.AddWordPool( "color", colorPool1 );

         Assert.ThrowsException<InvalidOperationException>( () => stringGenerator.AddWordPool( "color", colorPool2 ) );
      }

      [TestMethod]
      public void Generate_WhenPoolHasOneItem_WillGenerateStringWithReplacedField()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool.Add( "blue", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color}." );

         Assert.AreEqual( "The room was painted blue.", generated );
      }

      [TestMethod]
      public void Generate_WhenStringHasParameterTwiceButPoolHasOneItem_WillGenerateStringWithBothParametersReplacedBySameString()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool.Add( "blue", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes." );

         Assert.AreEqual( "The room was painted blue with blue stripes.", generated );
      }

      [TestMethod]
      public void Generate_WhenStringHasParameterTwiceWithKnownSeriesRandom_WillGenerateExpectedString()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new KnownSeriesRandom( 1, 2 ), string.Copy );
         colorPool.Add( "blue", 1 );
         colorPool.Add( "red", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.Generate( "The room was painted {color} with {color} stripes." );

         Assert.AreEqual( "The room was painted blue with red stripes.", generated );
      }

      [TestMethod]
      public void GenerateUnique_WhenStringHasParameterTwiceButPoolHasOneItem_WillThrowInvalidOperationException()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new DotNetRandom(), string.Copy );
         colorPool.Add( "blue", 1 );
         stringGenerator.AddWordPool( "color", colorPool );

         Assert.ThrowsException<InvalidOperationException>( () => stringGenerator.GenerateUnique( "The room was painted {color} with {color} stripes." ) );
      }

      [TestMethod]
      public void GenerateUnique_WhenStringHasParameterTwiceAndPoolHasTwoItems_WillGenerateExpectedString()
      {
         StringGenerator stringGenerator = new StringGenerator();
         WeightedPool<string> colorPool = new WeightedPool<string>( new KnownSeriesRandom( 1 ), string.Copy );
         colorPool.Add( "blue", 1 );
         colorPool.Add( "red", 5 );  
         stringGenerator.AddWordPool( "color", colorPool );

         string generated = stringGenerator.GenerateUnique( "The room was painted {color} with {color} stripes." );

         Assert.AreEqual( "The room was painted blue with red stripes.", generated );
      }
   }
}
