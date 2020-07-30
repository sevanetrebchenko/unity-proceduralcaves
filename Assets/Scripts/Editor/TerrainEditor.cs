using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WorldGenerator worldGenerator = (WorldGenerator)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            Debug.Log("Generating mesh...");
            worldGenerator.Generate();
        }

        if (GUILayout.Button("Clear Chunks"))
        {
            worldGenerator.ClearChunks();
        }
    }
}
