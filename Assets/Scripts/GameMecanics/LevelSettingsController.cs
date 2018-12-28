using UnityEngine;

public class LevelSettingsController : MonoBehaviour {

    [SerializeField] MeterCounter meterCounter;
    public static LevelSettings currentLevelSettings;
    public delegate void LevelSettingsChange(LevelSettings levelSettings);
    public static event LevelSettingsChange OnLevelSettingsChange;

    void OnEnable() {
        currentLevelSettings = LevelSettings.GetLevelSettingsScore(LevelSettings.LEVEL_10_SCORE);
        if (OnLevelSettingsChange != null)
            OnLevelSettingsChange(currentLevelSettings);
    }

    void Update() {
        if (meterCounter.Value >= currentLevelSettings.scoreMax) {
            currentLevelSettings = LevelSettings.GetLevelSettingsScore(meterCounter.Value);
            OnLevelSettingsChange(currentLevelSettings);
        }
    }


}
