using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serializables.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType), true)]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        private string[] typeNames, typeFullNames;

        private void Initialize()
        {
            if (typeFullNames != null) return;

            var parentType = fieldInfo.FieldType;
            if (parentType.IsArray)
                parentType = parentType.GetElementType();

            var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => ParentFilter(t, parentType))
                .ToArray();


            typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : "t.ReflectedType.Name + t.Name")
                .ToArray();
            typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }

        private static bool ParentFilter(Type type, Type parentType)
        {
            return !type.IsAbstract &&
                   !type.IsInterface &&
                   !type.IsGenericType &&
                   type != parentType &&
                   type.InheritsOrImplements(parentType);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize();
            if (property.isArray)
            {
                SerializeList(position, property, label);
                return;
            }

            SerializeType(position, property, label);
        }

        private void SerializeList(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.isArray) return;

            for (int i = 0; i < property.arraySize; i++)
            {
                var element = property.GetArrayElementAtIndex(i);

                if (i > 0) position.y += EditorGUIUtility.singleLineHeight;

                SerializeType(position, element, label);
            }
        }

        private void SerializeType(Rect position, SerializedProperty property, GUIContent label)
        {
            var assemblyQualifiedNameProperty = property.FindPropertyRelative("assemblyQualifiedName");

            var currentIndex = Array.IndexOf(typeFullNames, assemblyQualifiedNameProperty?.stringValue ?? "");

            var selectedIndex = EditorGUI.Popup(position, "", currentIndex, typeNames);

            if (assemblyQualifiedNameProperty == null) return;

            if (selectedIndex >= 0 && selectedIndex != currentIndex)
            {
                assemblyQualifiedNameProperty.stringValue = typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}