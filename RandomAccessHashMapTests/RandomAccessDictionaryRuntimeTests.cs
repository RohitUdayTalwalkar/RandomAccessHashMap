using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomAccessHashMap;
using System.Diagnostics;
using System.Collections.Generic;

namespace RandomAccessHashMapTests
{
    [TestClass]
    public class RandomAccessDictionaryRuntimeTests
    {
        /// <summary>
        /// Tolerate up to a 70 milisecond difference between min and max operation times.
        /// </summary>
        private const int operationToleranceInMS = 70;

        [TestMethod]
        public void TestPutIsContanstTime()
        {
            CreateCollectionAndCheckOperationTimesAreConstant(
                ()=> (new RandomAccessDictionary<string, int>(100000, new Random())),
                (collection, s,i) => collection.Put(s,i));
        }

        private void CreateCollectionAndCheckOperationTimesAreConstant(
            Func<RandomAccessDictionary<string,int>> initialize,
            Action<RandomAccessDictionary<string,int>,string, int> operation)
        {
            var collection = initialize.Invoke();
            var minOperationTime = long.MaxValue;
            var maxOperationTime = long.MinValue;

            var stopWatch = new Stopwatch();
            for (int i = 0; i < 100000; i++)
            {
                stopWatch.Reset();
                stopWatch.Start();
                operation.Invoke(collection,i.ToString(), i);
                stopWatch.Stop();

                minOperationTime = Math.Min(minOperationTime, stopWatch.ElapsedMilliseconds);
                maxOperationTime = Math.Max(maxOperationTime, stopWatch.ElapsedMilliseconds);
            }
            Assert.IsTrue(maxOperationTime - minOperationTime < operationToleranceInMS,
                string.Format("Expected min and max between {0} ms. Min:{1} max:{2}",
                operationToleranceInMS, minOperationTime, maxOperationTime));
        }

        [TestMethod]
        public void TestDeleteIsContanstTime()
        {
            CreateCollectionAndCheckOperationTimesAreConstant(
                () => (CreateTestCollection()),
                (collection, s, i) => collection.Delete(s));
        }

        private static RandomAccessDictionary<string, int> CreateTestCollection()
        {
            var collection = new RandomAccessDictionary<string, int>(10000, new Random());
            for (int i = 0; i < 100000; i++)
            {
                collection.Put(i.ToString(), i);
            }
            return collection;
        }

        [TestMethod]
        public void TestGetIsContanstTime()
        {
            CreateCollectionAndCheckOperationTimesAreConstant(
                () => (CreateTestCollection()),
                (collection, s, i) => collection.Get(s));
        }

        [TestMethod]
        public void TestGetRandomIsContanstTime()
        {
            CreateCollectionAndCheckOperationTimesAreConstant(
                () => (CreateTestCollection()),
                (collection, s, i) => collection.GetRandom());
        }
    }
}
