using UnityEngine;
using UnityEngine.UIElements;

namespace Voxell.Util.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
        private UnityEditor.Editor _editor;

        public UnityEditor.Editor InitializeInspector(Object obj) => InitializeInspector(obj, null);
        public UnityEditor.Editor InitializeInspector(Object obj, System.Type editorType)
        {
            ClearEditor();

            _editor = CreateEditor(obj, editorType);
            IMGUIContainer container = new IMGUIContainer(_editor.OnInspectorGUI);
            ScrollView scrollView = new ScrollView();
            scrollView.Add(container);
            Add(scrollView);

            return _editor;
        }

        public UnityEditor.Editor InitializeInspector(Object[] objs) => InitializeInspector(objs, null);
        public UnityEditor.Editor InitializeInspector(Object[] objs, System.Type editorType)
        {
            ClearEditor();

            _editor = CreateEditor(objs, editorType);
            IMGUIContainer container = new IMGUIContainer(_editor.OnInspectorGUI);
            ScrollView scrollView = new ScrollView();
            scrollView.Add(container);
            Add(scrollView);

            return _editor;
        }

        public void ClearEditor()
        {
            Clear();
            Object.DestroyImmediate(_editor);
        }

        private static UnityEditor.Editor CreateEditor(Object[] objs, System.Type editorType)
        {
            if (editorType == null) return UnityEditor.Editor.CreateEditor(objs);
            return UnityEditor.Editor.CreateEditor(objs, editorType);
        }

        private static UnityEditor.Editor CreateEditor(Object obj, System.Type editorType)
        {
            if (editorType == null) return UnityEditor.Editor.CreateEditor(obj);
            return UnityEditor.Editor.CreateEditor(obj, editorType);
        }
    }
}
