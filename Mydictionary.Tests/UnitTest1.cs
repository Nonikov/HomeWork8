using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using HomeWork8;

namespace Mydictionary.Tests
{
    [TestClass]
    public class UnitTest1
    {
        MyDictionary<int, int> dict;

        [TestInitialize]
        public void Init()
        {
            dict = new MyDictionary<int, int>();
        }

        [TestMethod]
        public void Ctor_Test()
        {
            Assert.AreNotEqual(null, dict);
        }

        //[TestMethod]  //// To test it, change the access modifiers to public
        //public void FindWithParent_Test()
        //{
        //    KeyValuePair<int, int> pair = new KeyValuePair<int, int>(3, 33);
        //    MyDictionary<int, int>.TreeItem parent;
        //    MyDictionary<int, int>.TreeItem expected;

        //    dict.Add(1, 11);
        //    dict.Add(2, 22);
        //    dict.Add(pair);
        //    dict.Add(4, 44);

        //    expected = dict.FindWithParent(3, out parent);

        //    Assert.AreEqual(22, parent._pair.Value);
        //}

        [TestMethod]
        public void Indexer_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(2, 22);

            dict[2] = 33;

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(11, dict[1]);
            Assert.AreEqual(33, dict[2]);
        }

        [TestMethod]
        public void AddItems_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(2, 22);

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(true, dict.ContainsKey(0));
            Assert.AreEqual(true, dict.ContainsKey(1));
            Assert.AreEqual(true, dict.ContainsKey(2));
        }

        [TestMethod]
        public void AddPair_Test()
        {
            KeyValuePair<int, int> pair1 = new KeyValuePair<int, int>(0, 00);
            KeyValuePair<int, int> pair2 = new KeyValuePair<int, int>(1, 11);
            KeyValuePair<int, int> pair3 = new KeyValuePair<int, int>(2, 22);

            dict.Add(pair1);
            dict.Add(pair2);
            dict.Add(pair3);

            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(true, dict.Contains(pair1));
            Assert.AreEqual(true, dict.Contains(pair2));
            Assert.AreEqual(true, dict.Contains(pair3));
        }

        [TestMethod]
        public void GetKeys_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);

            ICollection<int> collect = dict.Keys;

            Assert.AreEqual(dict.Count, collect.Count);
            Assert.AreEqual(true, collect.Contains(0));
            Assert.AreEqual(true, collect.Contains(1));
        }

        [TestMethod]
        public void GetValues_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);

            ICollection<int> collect = dict.Values;

            Assert.AreEqual(dict.Count, collect.Count);
            Assert.AreEqual(true, collect.Contains(00));
            Assert.AreEqual(true, collect.Contains(11));
        }

        [TestMethod]
        public void Enumerable_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);

            Dictionary<int, int> tempDict = new Dictionary<int, int>();

            foreach (var item in dict)
            {
                tempDict.Add(item.Key, item.Value);
            }

            Assert.AreEqual(00, tempDict[0]);
            Assert.AreEqual(11, tempDict[1]);
        }

        [TestMethod]
        public void Contains_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(2, 22);

            Assert.AreEqual(true, dict.Contains(new KeyValuePair<int, int>(1, 11)));
            Assert.AreEqual(false, dict.Contains(new KeyValuePair<int, int>(1, 77)));
        }

        [TestMethod]
        public void ContainsKey_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(2, 22);

            Assert.AreEqual(true, dict.ContainsKey(1));
            Assert.AreEqual(false, dict.ContainsKey(3));
        }

        [TestMethod]
        public void CopyTo_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);

            KeyValuePair<int, int>[] pair = new KeyValuePair<int, int>[dict.Count];
            dict.CopyTo(pair, 0);

            Assert.AreEqual(new KeyValuePair<int, int>(0, 00), pair[0]);
            Assert.AreEqual(new KeyValuePair<int, int>(1, 11), pair[1]);
        }

        [TestMethod]
        public void RemoveByKey_Test()
        {
            dict = new MyDictionary<int, int>(true);

            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(1, 12);
            dict.Add(2, 22);

            dict.Remove(1);


            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(true, dict.ContainsKey(1));
            Assert.AreEqual(false, dict.Contains(new KeyValuePair<int, int>(1, 11)));
            Assert.AreEqual(true, dict.Contains(new KeyValuePair<int, int>(1, 12)));
            Assert.AreEqual(12, dict[1]);
        }

        [TestMethod]
        public void RemoveByItem_Test()
        {
            dict = new MyDictionary<int, int>(true);

            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(5, 23);
            dict.Add(7, 7);
            dict.Add(9, 77);
            dict.Add(6, 77);
            dict.Add(5, 22);
            dict.Add(5, 24);
            
            dict.Remove(new KeyValuePair<int, int>(5, 24));
            dict.Remove(new KeyValuePair<int, int>(5, 22));
            dict.Remove(new KeyValuePair<int, int>(5, 23));

            Assert.AreEqual(5, dict.Count);
            Assert.AreEqual(false, dict.ContainsKey(5), "Contains");
            Assert.AreEqual(true, dict.Contains(new KeyValuePair<int, int>(0, 00)));
            Assert.AreEqual(true, dict.Contains(new KeyValuePair<int, int>(1, 11)));
            Assert.AreEqual(false, dict.Contains(new KeyValuePair<int, int>(5, 22)), "5, 22");
            Assert.AreEqual(false, dict.Contains(new KeyValuePair<int, int>(5, 23)), "5, 23");
            Assert.AreEqual(false, dict.Contains(new KeyValuePair<int, int>(5, 24)), "5, 24");
        }

        [TestMethod]
        public void TryGetValue_Test()
        {
            dict.Add(0, 00);
            dict.Add(1, 11);
            dict.Add(5, 23);

            int expected;

            Assert.AreNotEqual(true, dict.TryGetValue(77, out expected));
            Assert.AreEqual(true, dict.TryGetValue(1, out expected));
            Assert.AreEqual(11, expected);
        }
    }
}
