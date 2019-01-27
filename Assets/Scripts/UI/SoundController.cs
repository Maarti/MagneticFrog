using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour {

    [SerializeField] AudioSource audioSource;
    [SerializeField] float pitch = 1f;
    [SerializeField] float pitchGain = .5f;
    [SerializeField] float pitchLossPerSecond = .25f;
    [SerializeField] float pitchMin = 1f;
    [SerializeField] float pitchMax = 3f;
    float lastTimePlay = -10f;
    AudioClip audioClip;

    void Start() {
        audioClip = audioSource.clip;
        GameController.OnGameStart += Init;
    }

    void Init() {
        pitch = 1f;
    }

    public void Play() {
        SetPitch();
        // audioSource.Play();
        audioSource.PlayOneShot(audioClip);
        pitch += pitchGain;
        lastTimePlay = Time.time;
    }

    private void SetPitch() {
        float pitchLoss = pitchLossPerSecond * (Time.time - lastTimePlay);
        pitch = Mathf.Clamp(pitch - pitchLoss, pitchMin, pitchMax);
        audioSource.pitch = pitch;
    }
}