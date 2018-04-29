using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork8
{
    public class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable
    {
        #region TreeItem_InerDictionary
        private class TreeItem
        {
            public KeyValuePair<TKey, TValue> _pair;
            public TreeItem _left = null;
            public TreeItem _right = null;

            public TreeItem(TKey key, TValue value)
            {
                _pair = new KeyValuePair<TKey, TValue>(key, value);
            }

            public TreeItem(KeyValuePair<TKey, TValue> pair)
            {
                _pair = pair;
            }
        }
        #endregion

        #region Fields
        TreeItem _root = null;
        int _counter = 0;
        bool _allowDuplicateKeys;
        #endregion

        #region Ctor
        public MyDictionary(bool allowDubplicateKeys = false)
        {
            _allowDuplicateKeys = allowDubplicateKeys;
        }
        #endregion

        #region Properties and indexer
        public TValue this[TKey key]
        {
            get
            {
                bool flag = true;
                TValue value = default(TValue);
                foreach (var item in this)
                {
                    if (item.Key.CompareTo(key) == 0)
                    {
                        value = item.Value;
                        flag = false;
                        break;
                    }
                }
                if(flag)
                {
                    throw new ArgumentException("Key doesn't exist");
                }
                return value;
            }
            set
            {
                Stack<TreeItem> stack = new Stack<TreeItem>();
                TreeItem tempNode = _root;

                while (tempNode != null || stack.Count != 0)
                {
                    if (stack.Count != 0)
                    {
                        tempNode = stack.Pop();

                        if (tempNode._pair.Key.CompareTo(key) == 0)
                        {
                            tempNode._pair = new KeyValuePair<TKey, TValue>(key, value);
                            break;
                        }

                        if (tempNode._right != null)
                        {
                            tempNode = tempNode._right;
                        }
                        else
                        {
                            tempNode = null;
                        }
                    }
                    while (tempNode != null)
                    {
                        stack.Push(tempNode);
                        tempNode = tempNode._left;
                    }
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> list = new List<TKey>();
                foreach (var item in this)
                {
                    list.Add(item.Key);
                }
                return list;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> list = new List<TValue>();
                foreach (var item in this)
                {
                    list.Add(item.Value);
                }
                return list;
            }
        }

        public int Count { get => _counter; private set => _counter = value; }

        public bool IsReadOnly { get => false; }
        #endregion

        #region Utility
        private TreeItem FindWithParentByKey(TKey key, out TreeItem parent)
        {
            TreeItem current = _root;
            parent = null;

            while (current != null)
            {
                int result = current._pair.Key.CompareTo(key);

                if (result > 0) // Go to left
                {
                    parent = current;
                    current = current._left;
                }
                else if (result < 0) // Go to right
                {
                    parent = current;
                    current = current._right;
                }
                else
                {
                    break; // have found
                }
            }
            return current;
        }

        private TreeItem FindWithParentByItem(KeyValuePair<TKey, TValue> item, out TreeItem parent)
        {
            TreeItem current = _root;
            parent = null;

            while (current != null)
            {
                int result = current._pair.Key.CompareTo(item.Key);

                if (result > 0) // Go to left
                {
                    parent = current;
                    current = current._left;
                }
                else if (result < 0) // Go to right
                {
                    parent = current;
                    current = current._right;
                }
                else
                {
                    if (current._pair.Value.Equals(item.Value))
                    {
                        break; // have found
                    }
                    else
                    {
                        parent = current;
                        current = current._right;
                    }
                }
            }
            return current;
        }

        private void RemoveRightNull(TreeItem current, TreeItem parent)
        {
            if (parent == null)
            {
                _root = current._left;
            }
            else
            {
                if (parent._pair.Key.CompareTo(current._pair.Key) > 0)
                {
                    parent._left = current._left;
                }
                else
                {
                    parent._right = current._left;
                }
            }
        }

        private void RemoveRightLeftNull(TreeItem current, TreeItem parent)
        {
            current._right._left = current._left;

            if (parent == null)
            {
                _root = current._right;
            }
            else
            {
                if (parent._pair.Key.CompareTo(current._pair.Key) > 0)
                {
                    parent._left = current._right;
                }
                else 
                {
                    parent._right = current._right;
                }
            }
        }

        private void RemoveRightLeftAre(TreeItem current, TreeItem parent)
        {
            TreeItem leftmost = current._right._left;
            TreeItem leftmostParent = current._right;

            while (leftmost._left != null)
            {
                leftmostParent = leftmost;
                leftmost = leftmost._left;
            }

            leftmostParent._left = leftmost._right;

            leftmost._left = current._left;
            leftmost._right = current._right;

            if (parent == null)
            {
                _root = leftmost;
            }
            else
            {
                if (parent._pair.Key.CompareTo(current._pair.Key) > 0)
                {
                    parent._left = leftmost;
                }
                else 
                {
                    parent._right = leftmost;
                }
            }
        }

        #endregion

        #region Methods
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_root == null)
            {
                _root = new TreeItem(item);
                _counter++;
            }
            else
            {
                Add(item, _root);
            }
        }

        void Add(KeyValuePair<TKey, TValue> pair, TreeItem parent)
        {
            if (!_allowDuplicateKeys && pair.Key.CompareTo(parent._pair.Key) == 0)
            {
                parent._pair = pair;
            }
            else if (parent._pair.Key.CompareTo(pair.Key) > 0) // go to left
            {
                if (parent._left == null)
                {
                    parent._left = new TreeItem(pair);
                    _counter++;
                }
                else
                {
                    Add(pair, parent._left);
                }
            }
            else // go to right
            {
                if (parent._right == null)
                {
                    parent._right = new TreeItem(pair);
                    _counter++;
                }
                else
                {
                    Add(pair, parent._right);
                }
            }
        }

        public void Clear()
        {
            _root = null;
            _counter = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            foreach (var pair in this)
            {
                if (item.Value.Equals(pair.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var pair in this)
            {
                if (pair.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(TKey key)
        {
            TreeItem current;
            TreeItem parent;

            current = FindWithParentByKey(key, out parent); // Search for a node to remove

            if (current == null)
            {
                return false;
            }
            Count--;

            if (current._right == null)  //If tree does not have right node
            {
                RemoveRightNull(current, parent);
            }

            else if (current._right._left == null)   //If tree has right node which does not have left node
            {
                RemoveRightLeftNull(current, parent);
            }

            else  //If tree has right node which has left node
            {
                RemoveRightLeftAre(current, parent);
            }
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_allowDuplicateKeys == false)
            {
                Remove(item.Key);
            }
            TreeItem current;
            TreeItem parent;

            current = FindWithParentByItem(item, out parent); // Search for a node to remove

            if (current == null)
            {
                return false;
            }
            Count--;

            if (current._right == null)  //If tree does not have right node
            {
                RemoveRightNull(current, parent);
            }

            else if (current._right._left == null)   //If tree has right node which does not have left node
            {
                RemoveRightLeftNull(current, parent);
            }

            else  //If tree has right node which has left node
            {
                RemoveRightLeftAre(current, parent);
            }
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var item in this)
            {
                if (item.Key.CompareTo(key) == 0)
                {
                    value = item.Value;
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Stack<TreeItem> stack = new Stack<TreeItem>();
            TreeItem tempNode = _root;

            while (tempNode != null || stack.Count != 0)
            {
                if (stack.Count != 0)
                {
                    tempNode = stack.Pop();

                    yield return tempNode._pair;

                    if (tempNode._right != null)
                    {
                        tempNode = tempNode._right;
                    }
                    else
                    {
                        tempNode = null;
                    }
                }
                while (tempNode != null)
                {
                    stack.Push(tempNode);
                    tempNode = tempNode._left;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #endregion
    }
}
