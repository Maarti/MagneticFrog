using UnityEngine;

public class UrlManager : MonoBehaviour {

    public void OpenLink(string url) {
#if UNITY_ANDROID
        if (url == "URL_PLAY_STORE")
            url = "URL_PLAY_STORE_ANDROID";
#endif
        Debug.Log("Opening url " + url);
        Application.OpenURL(url);
    }
}
