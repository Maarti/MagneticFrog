using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour {

    [SerializeField] AudioSource audioSource;
    [SerializeField] float pitchGain = .5f;
    [SerializeField] float pitchLossPerSecond = .25f;
    [SerializeField] float pitchMin = 1f;
    [SerializeField] float pitchMax = 3f;
    [SerializeField] bool randomPitch = false;
    float pitch = 1f;
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
        audioSource.PlayOneShot(audioClip);
        if (!randomPitch) {
            pitch += pitchGain;
            lastTimePlay = Time.time;
        }
    }

    private void SetPitch() {
        if (randomPitch) {
            pitch = Random.Range(pitchMin, pitchMax);
        }
        else {
            float pitchLoss = pitchLossPerSecond * (Time.time - lastTimePlay);
            pitch = Mathf.Clamp(pitch - pitchLoss, pitchMin, pitchMax);
        }
        audioSource.pitch = pitch;
    }
}