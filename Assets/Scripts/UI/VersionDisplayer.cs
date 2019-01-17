using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionDisplayer : MonoBehaviour {

    void Start() {
        GetComponent<TextMeshProUGUI>().text = "Version " + Application.version.ToString();
    }
}
