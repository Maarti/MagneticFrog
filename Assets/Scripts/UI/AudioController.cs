using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    [SerializeField] Image audioButtonImage;
    [SerializeField] Sprite muteImage;
    [SerializeField] Sprite unmuteImage;

    public void ToggleMuteAudio() {
        if (isMuted())
            Unmute();
        else
            Mute();
    }

    bool isMuted() {
        return AudioListener.volume == 0f;
    }

    public void Mute() {
        AudioListener.volume = 0f;
        audioButtonImage.sprite = muteImage;
    }

    public void Unmute() {
        AudioListener.volume = 1f;
        audioButtonImage.sprite = unmuteImage;
    }
}
