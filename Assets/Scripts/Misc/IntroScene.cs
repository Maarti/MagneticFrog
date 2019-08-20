using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {
    [SerializeField] PlayableDirector director;

    void OnEnable() {
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Update() {
        if (Input.touchCount > 0) {
            SceneManager.LoadScene("main_scene", LoadSceneMode.Single);
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
