using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Voxell.Inspector
{
  public class InspectorView : VisualElement
  {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}
    private Editor _editor;

    public void InitializeInspector(Object obj) => InitializeInspector(obj, null);
    public void InitializeInspector(Object obj, System.Type editorType)
    {
      ClearEditor();

      if (editorType == null) _editor = Editor.CreateEditor(obj);
      else _editor = Editor.CreateEditor(obj, editorType);
      IMGUIContainer container = new IMGUIContainer(_editor.OnInspectorGUI);
      ScrollView scrollView = new ScrollView();
      scrollView.Add(container);
      Add(scrollView);
    }

    public void InitializeInspector(Object[] objs) => InitializeInspector(objs, null);
    public void InitializeInspector(Object[] objs, System.Type editorType)
    {
      ClearEditor();

      if (editorType == null) _editor = Editor.CreateEditor(objs);
      else _editor = Editor.CreateEditor(objs, editorType);
      IMGUIContainer container = new IMGUIContainer(_editor.OnInspectorGUI);
      ScrollView scrollView = new ScrollView();
      scrollView.Add(container);
      Add(scrollView);
    }

    public void ClearEditor()
    {
      Clear();
      Object.DestroyImmediate(_editor);
    }
  }
}