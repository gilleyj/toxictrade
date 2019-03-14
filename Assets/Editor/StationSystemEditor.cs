using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (StationsManager))]
public class StationSystemDataEditor : Editor {

    private readonly GUIContent randomButtonContent = new GUIContent (
        "Randomize Data",
        "Randomizes every item in the allocated station list within the World bounds.");
    private readonly GUIContent loadButtonContent = new GUIContent (
        "Load Data Into Scene",
        "Loads the station data into the scene.");

    public override void OnInspectorGUI () {
        StationsManager mgr = (StationsManager) target;

        Editor delegateEditor = Editor.CreateEditor (mgr);
        delegateEditor.DrawDefaultInspector ();

        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button (randomButtonContent)) {
            Randomize (ref mgr.data, mgr.GetComponent<Bounded> ().bounds);
        }
        if (GUILayout.Button (loadButtonContent)) {
            mgr.Load ();
        }
        EditorGUILayout.EndHorizontal ();

        if (mgr.data != null) {
            Editor dataEditor = Editor.CreateEditor (mgr.data);
            dataEditor.DrawDefaultInspector ();
        }
    }

    private static void Randomize (ref StationsData data, Vector3 bounds) {
        for (int i = 0; i < data.stations.Count; i++) {
            float newX = Random.Range (-bounds.x / 2, bounds.x / 2);
            float newY = Random.Range (-bounds.y / 2, bounds.y / 2);
            float newZ = Random.Range (-bounds.z / 2, bounds.z / 2);
            float newRotX = Random.Range (-180.0f, 180.0f);
            float newRotY = Random.Range (-180.0f, 180.0f);
            float newRotZ = Random.Range (-180.0f, 180.0f);
            Vector3 newPos = new Vector3 (newX, newY, newZ);
            Vector3 newRot = new Vector3 (newRotX, newRotY, newRotZ);
            data.stations[i].position = newPos;
            data.stations[i].rotation = newRot;
            data.stations[i].name = $"station #{i}";
        }
    }
}