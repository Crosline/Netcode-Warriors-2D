using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] private string assemblyQualifiedName = string.Empty;

        public Type Type { get; private set; }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Type == null) return;

            assemblyQualifiedName = Type.AssemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!TryGetType(out var type)) return;
            Type = type;
        }

        private bool TryGetType(out Type type)
        {
            type = Type.GetType(assemblyQualifiedName);

            return type != null || !string.IsNullOrWhiteSpace(assemblyQualifiedName);
        }

        // Implicit conversion from SerializableType to Type
        public static implicit operator Type(SerializableType sType)
        {
            return sType.Type;
        }

        // Implicit conversion from Type to SerializableType
        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType { Type = type };
        }
    }
}