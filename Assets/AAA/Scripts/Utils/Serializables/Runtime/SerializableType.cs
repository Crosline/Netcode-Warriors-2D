using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Serializables
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]

    public class SerializableType<T> : ISerializationCallbackReceiver
    {
        [SerializeField]
        [FormerlySerializedAs("assemblyQualifiedName")] 
        private string AssemblyQualifiedName = string.Empty;

        public TypeInfo TypeInfo;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            TypeInfo ??= typeof(T).GetTypeInfo();

            AssemblyQualifiedName = TypeInfo.AssemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrWhiteSpace(AssemblyQualifiedName)) return;

            var type = Type.GetType(AssemblyQualifiedName);
            
            if (type == null) return;

            TypeInfo = type.GetTypeInfo();
        }
    }
}