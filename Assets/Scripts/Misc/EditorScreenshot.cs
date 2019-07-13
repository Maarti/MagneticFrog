using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorScreenshot : MonoBehaviour {
    [MenuItem("Screenshot/Take screenshot")]
    static void Screenshot() {
        ScreenCapture.CaptureScreenshot("screenshot.png");
    }
}
