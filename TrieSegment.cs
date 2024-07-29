﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Node = Minerva.Module.Trie.Node;
using FirstLayerKeyCollection = Minerva.Module.Trie.FirstLayerKeyCollection;
using KeyCollection = Minerva.Module.Trie.KeyCollection;

namespace Minerva.Module
{
    public interface ITrie : ITrieBase, IEnumerable<string>, IEnumerable, ICollection<string>
    {
        internal Node Root { get; }
        internal bool TryGetNode<T>(T prefix, out Node currentNode) where T : IList<string>;
    }

    public readonly struct TrieSegment : ITrie, IEnumerable<string>, IEnumerable, ICollection<string>
    {
        readonly char separator;
        readonly Node root;

        public readonly bool this[string key]
        {
            get { return Contains(key); }
            set { Set(key, value); }
        }

        public readonly int Count => root.count;

        public readonly FirstLayerKeyCollection FirstLayerKeys => root?.LocalKeys ?? FirstLayerKeyCollection.Empty;

        public readonly KeyCollection Keys => new KeyCollection(root, separator);

        public readonly bool IsReadOnly => false;

        Node ITrie.Root => root;
        public readonly char Separator => separator;


        internal TrieSegment(Node root, char separator)
        {
            this.separator = separator;
            this.root = root;
        }


        void ICollection<string>.Add(string item) => Add(item);

        public readonly bool Add(string s) => Add(Split(s));

        public readonly bool Add<T>(T prefix) where T : IList<string> => root?.Add(prefix) ?? false;

        public readonly void AddRange(IEnumerable<string> ts) { foreach (var item in ts) Add(item); }

        public readonly bool Contains(string s) => Contains(Split(s));

        public readonly bool Contains<T>(T prefix) where T : IList<string> => root?.ContainsKey(prefix) ?? false;

        public readonly bool ContainsPartialKey(string s) => ContainsPartialKey(Split(s));

        public readonly bool ContainsPartialKey<T>(T prefix) where T : IList<string> => root?.ContainsPartialKey(prefix) ?? false;

        public readonly bool Remove(string s) => Remove(Split(s));

        public readonly bool Remove<T>(T prefix) where T : IList<string> => root?.Remove(prefix) ?? false;

        public readonly bool Set(string s, bool value) => value ? Add(s) : Remove(s);

        public readonly bool Set<T>(T s, bool value) where T : IList<string> => value ? Add(s) : Remove(s);

        public readonly void Clear() => root.Clear();

        public readonly bool Clear(string key) => Clear(Split(key));

        public readonly bool Clear<T>(T prefix) where T : IList<string>
        {
            if (!TryGetNode(prefix, out var node)) return false;
            return node.Clear();
        }

        public readonly void Shrink() => root?.Shrink();






        bool ITrie.TryGetNode<T>(T prefix, out Node node) => TryGetNode(prefix, out node);
        private readonly bool TryGetNode<T>(T prefix, out Node node) where T : IList<string> => root?.TryGetNode(prefix, out node) ?? ((node = default) == null && false);





        public readonly IEnumerator<string> GetEnumerator()
        {
            string[] arr = new string[Count];
            CopyTo(arr, 0);
            foreach (var item in arr)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string[] Split(string s) => Split(s, separator);




        public readonly void CopyTo(string[] array, int index)
        {
            int arrayIndex = index;
            root?.TraverseCopyKey(new(), separator, array, ref arrayIndex);
        }

        public readonly string[] ToArray()
        {
            string[] arry = new string[Count];
            CopyTo(arry, 0);
            return arry;
        }





        public static string[] Split(string s, char separator)
        {
            if (s.EndsWith(separator)) return s.Split(separator)[..^1];
            return string.IsNullOrEmpty(s) ? new string[0] : s.Split(separator);
        }
    }
}