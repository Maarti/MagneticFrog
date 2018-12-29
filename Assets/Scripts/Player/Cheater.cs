using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cheater))]
public class CheaterEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Cheater cheatScript = (Cheater)target;
        if (GUILayout.Button("UP!")) {
            cheatScript.GoUp();
        }
    }
}

public class Cheater : MonoBehaviour {

    public void GoUp() {
        Vector3 pos = transform.position;
        pos.y += 50f;
        transform.position = pos;
    }
}