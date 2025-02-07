using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace Serializables
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        internal struct KeyValuePair
        {
            public TKey Key;
            public TValue Value;
        }

        [SerializeField]
        private KeyValuePair[] Items;

        public void CopyTo(SerializableDictionary<TKey, TValue> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            other.Clear();
            foreach (var kvp in this)
            {
                other.Add(kvp.Key, kvp.Value);
            }
        }

        public void OnBeforeSerialize()
        {
            Items = new KeyValuePair[Count];
            var index = 0;
            foreach (var (key, value) in this)
            {
                Items[index] = new KeyValuePair
                {
                    Key = key,
                    Value = value
                };
                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (Items == null || Items.Length <= 0) return;

            foreach (var keyValuePair in Items)
            {
                Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}