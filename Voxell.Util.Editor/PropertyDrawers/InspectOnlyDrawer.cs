using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Voxell.Util.Editor
{
    [CustomPropertyDrawer(typeof(InspectOnly))]
    public class InspectOnlyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyField = new PropertyField(property);
            propertyField.SetEnabled(false);
            return propertyField;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(rect, property, label, true);
            GUI.enabled = guiEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}
