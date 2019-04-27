using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class OxygenController : MonoBehaviour {

    public delegate void BubbleCollectDelegate();
    public static event BubbleCollectDelegate OnBubbleCollect;
    [SerializeField] PlayerController playerCtrlr;
    [SerializeField] Slider oxygenBar;
    [SerializeField] Animator oxygenBarAnimator;
    [SerializeField] RectTransform lockedArea;
    public float oxygenMax = 35f;
    public float oxygenConsumption = 1f;    // oxygen consumption per second
    float oxygen;
    public float Oxygen {
        get { return oxygen; }
        set {
            oxygen = Mathf.Clamp(value, 0f, oxygenMax);
            oxygenBar.value = oxygen;
        }
    }
    [SerializeField] Animator audioAnim;
    [SerializeField] AudioSource mainTheme;
    [SerializeField] AudioSource stressfulMusic;
    const float MAX_ANIM_SPEED = 3f;
    const float ANIM_START_WHEN_OXYGEN_REACHES = 7.5f;
    const float OXYGEN_LEVEL_WHEN_STRESSFUL_MUSIC_START = 15f;
    float maxMainThemeVolume = 1f;
    float maxStressfulMusicVolume = 1f;

    public void Start() {
        maxMainThemeVolume = mainTheme.volume;
        maxStressfulMusicVolume = stressfulMusic.volume;
    }

    public void Init() {
        InitBreath();
        oxygenBar.minValue = 0f;
        oxygenBar.maxValue = 35f;
        oxygenBar.value = Oxygen = oxygenMax;
    }

    void Update() {
        Oxygen -= oxygenConsumption * Time.deltaTime;
        oxygenBarAnimator.SetFloat("oxygen", Oxygen);
        float animSpeed = Mathf.Lerp(MAX_ANIM_SPEED, 1f, Oxygen / ANIM_START_WHEN_OXYGEN_REACHES);
        oxygenBarAnimator.SetFloat("speed", animSpeed);
        MixMusic();
        if (Oxygen <= 0)
            playerCtrlr.Die();
    }

    public void AddOxygen(float oxygenAmount) {
        Oxygen += oxygenAmount;
        if (OnBubbleCollect != null)
            OnBubbleCollect();
    }

    void InitBreath() {
        switch (ApplicationController.ac.characters[CharacterSelector.currentCharacter].breath) {
            case 0:
                oxygenMax = 20f;
                lockedArea.sizeDelta = new Vector2(lockedArea.sizeDelta.x, 422);
                break;
            case 1:
                oxygenMax = 25f;
                lockedArea.sizeDelta = new Vector2(lockedArea.sizeDelta.x, 297);
                break;
            case 2:
                oxygenMax = 30f;
                lockedArea.sizeDelta = new Vector2(lockedArea.sizeDelta.x, 170);
                break;
            case 3:
                oxygenMax = 35f;
                lockedArea.sizeDelta = new Vector2(lockedArea.sizeDelta.x, 0);
                break;
        }
    }

    // Blend between normal music and stressful music depending on the oxygen level
    void MixMusic() {
        if (playerCtrlr.isPlayingTutorial || Time.timeScale <= 0f) return;

        if (Oxygen > OXYGEN_LEVEL_WHEN_STRESSFUL_MUSIC_START || Oxygen <= 0f) {
            if (audioAnim.GetBool("isStressful"))
                audioAnim.SetBool("isStressful", false);
            if (stressfulMusic.isPlaying)
                stressfulMusic.Stop();
            /*   if (mainTheme.volume < maxMainThemeVolume)
                   mainTheme.volume = maxMainThemeVolume;*/
            return;
        }
        else {
            if (!audioAnim.GetBool("isStressful"))
                audioAnim.SetBool("isStressful", true);
            if (!stressfulMusic.isPlaying)
                stressfulMusic.Play();
            if (Oxygen <= 5f)
                stressfulMusic.pitch = 1.5f;
            else if (Oxygen <= 10f)
                stressfulMusic.pitch = 1.25f;
            else
                stressfulMusic.pitch = 1f;
            /*  float mainThemeVolumeRatio = Oxygen / OXYGEN_LEVEL_WHEN_STRESSFUL_MUSIC_START;
              float stressfulMusicVolumeRatio = 1f - mainThemeVolumeRatio;
              mainTheme.volume = mainThemeVolumeRatio * maxMainThemeVolume;
              stressfulMusic.volume = stressfulMusicVolumeRatio * maxStressfulMusicVolume;*/
        }
    }

    public void ResetMusic() {
        audioAnim.SetBool("isStressful", false);
        stressfulMusic.pitch = 1f;
        stressfulMusic.Stop();
    }
}
