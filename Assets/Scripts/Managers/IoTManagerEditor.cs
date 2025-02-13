#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IoTManager))]
public class IoTManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IoTManager manager = (IoTManager)target;
        if (GUILayout.Button("Simulate Command"))
        {
            manager.SimulateCommand();
        }
    }
}
#endif
