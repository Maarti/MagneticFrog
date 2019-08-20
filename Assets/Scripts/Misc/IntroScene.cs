using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {
    [SerializeField] PlayableDirector director;

    void OnEnable() {
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Update() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Escape)){
#else
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)) {
#endif        
            director.Stop();
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector) {
        if (director == aDirector)
            SceneManager.LoadScene("main_scene", LoadSceneMode.Single);
    }

    void OnDisable() {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
