using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SurroundingAwareness))]
public class SurroundingAwarenessEditor : Editor
{
    SurroundingAwareness sa;

    public void OnSceneGUI()
    {
        sa = target as SurroundingAwareness;
        if (!sa) return;

        Handles.color = Color.red;
        var transform = sa.transform;
        Handles.DrawWireDisc(transform.position, transform.forward, sa.SightRadius);
    }
}
