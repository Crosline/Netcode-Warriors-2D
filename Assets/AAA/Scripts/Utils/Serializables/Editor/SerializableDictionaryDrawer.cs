using UnityEditor;
using UnityEngine;

namespace Serializables.Editor
{
	[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
	public class SerializableDictionaryDrawer : PropertyDrawer
	{
		private int _expandedIndex = -1;
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		    var fieldRect = new Rect(position)
		    {
		        height = EditorGUIUtility.singleLineHeight
		    };
		    
		    EditorGUI.PropertyField(fieldRect, property);
		    property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);
		    
		    if (!property.isExpanded)
			    return;
		    
		    var itemsProperty = property.FindPropertyRelative("Items");
		    
		    var marchingRect = new Rect(fieldRect);
		    
		    for (int i = 0; i < itemsProperty.arraySize; i++)
		    {
			    var element = itemsProperty.GetArrayElementAtIndex(i);
			    
			    var keyProperty = element.FindPropertyRelative("Key");
			    var valueProperty = element.FindPropertyRelative("Value");
			    
			    marchingRect.y += EditorGUIUtility.singleLineHeight;
			    
                var keyRect = new Rect(marchingRect);
                keyRect.height = EditorGUI.GetPropertyHeight(keyProperty, true);

                var valueRect = new Rect(keyRect);
                valueRect.y += EditorGUIUtility.standardVerticalSpacing;
                valueRect.height = EditorGUI.GetPropertyHeight(valueProperty, true);

                EditorGUI.PropertyField(keyRect, keyProperty, true);
                
                var isExpanded = EditorGUI.Foldout(valueRect, _expandedIndex == i, valueProperty.displayName, true);
                
                if (isExpanded)
                {
	                _expandedIndex = i;
					marchingRect.y += EditorGUIUtility.standardVerticalSpacing;
					marchingRect.height = valueRect.height + EditorGUIUtility.standardVerticalSpacing + keyRect.height;
					EditorGUI.PropertyField(marchingRect, valueProperty, true);
				}
		    }
		}
		
		// public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		// 	var totalHeight = 0.0f;
		//
		// 	totalHeight += EditorGUIUtility.singleLineHeight;
		//
		// 	if (!property.isExpanded)
		// 		return totalHeight;
		// 	
		// 	var itemsProperty = property.FindPropertyRelative("Items");
		// 	
		// 	for (int i = 0; i < itemsProperty.arraySize; i++)
		// 	{
		// 		
		// 		var element = itemsProperty.GetArrayElementAtIndex(i);
		// 		
		// 		totalHeight += EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(element);
		// 	    // EditorGUI.PropertyField(fieldRect, element, GUIContent.none);
		// 	}
		//
		// 	return totalHeight;
		// }
	}
}