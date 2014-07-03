using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomAccessHashMap;
using System.Collections.Generic;
using Moq;


namespace RandomAccessHashMapTests
{
    [TestClass]
    public class RandomAccessDictionaryTests
    {
        [TestMethod, ExpectedException(typeof (ArgumentException))]
        public void TestStartCapacityMustBe_10000_OrMore()
        {
            var collection = new RandomAccessDictionary<string, int>(100, new Random());
        }

        [TestMethod]
        public void TestPutAddsElement()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            
            var key = "sample_key";
            var item = 12345;

            collection.Put(key, item);

            Assert.AreEqual(item, collection._data[key]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestPutThrowsArgumentExceptionForDuplicateKeys()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            collection.Put("key", 10);
            collection.Put("key", 123);
        }

        [TestMethod]
        public void TestDeleteRemovesElement()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            collection.Put("key", 100);
            collection.Delete("key");
            Assert.IsFalse(collection._data.ContainsKey("key"));
        }

        [TestMethod]
        public void TestDeleteReturnsTrueOnRemove()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            collection.Put("key", 100);
            Assert.IsTrue(collection.Delete("key"));
        }

        [TestMethod]
        public void TestDeleteReturnsFalseIfNothingRemoved()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            Assert.IsFalse(collection.Delete("missing_key"));
        }

        [TestMethod]
        public void TestGetReturnsElement()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            var key = "sample_key";
            var item = 12345;

            collection.Put(key, item);

            Assert.AreEqual(item, collection.Get(key));
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void TestGetMissingKeyThrowsException()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            collection.Get("missing_key");
        }

        [TestMethod]
        public void TestGetRandomUsesRandomIndex()
        {
            var mockRandom = new Mock<Random>();

            var collection = new RandomAccessDictionary<string, int>(10000, mockRandom.Object);
            mockRandom.Setup((random) => random.Next(It.IsAny<int>())).Returns(0);
            collection.Put("item1", 0);
            collection.Put("item2", 1);
            collection.Put("item3", 2);

            Assert.AreEqual(0, collection.GetRandom());
            Assert.AreEqual(1, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());
            Assert.AreEqual(0, collection.GetRandom());
            Assert.AreEqual(1, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());

            collection.Delete("item1");
            Assert.AreEqual(1, collection.GetRandom());
            Assert.AreEqual(1, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());
            Assert.AreEqual(1, collection.GetRandom());

            collection.Delete("item2");
            Assert.AreEqual(2, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());
            Assert.AreEqual(2, collection.GetRandom());
        }
    }
}
