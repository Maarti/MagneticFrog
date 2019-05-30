using UnityEngine;

public class LevelSettingsController : MonoBehaviour {

    public static LevelSettings currentLevelSettings;
    public static int nextBurstIndex = 0;
    public delegate void LevelSettingsChange(LevelSettings levelSettings);
    public static event LevelSettingsChange OnLevelSettingsChange;
    [SerializeField] MeterCounter meterCounter;
    [SerializeField] CoinSpawner coinSpawner;
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;
    [SerializeField] RockSpawner rockSpawner;

    void OnEnable() {
        currentLevelSettings = LevelSettings.GetLevelSettingsScore(LevelSettings.LEVEL_10_SCORE);
        nextBurstIndex = 0;
        if (OnLevelSettingsChange != null)
            OnLevelSettingsChange(currentLevelSettings);
    }

    void Update() {
        if (meterCounter.Value >= currentLevelSettings.scoreMax) {
            currentLevelSettings = LevelSettings.GetLevelSettingsScore(meterCounter.Value);
            if (OnLevelSettingsChange != null)
                OnLevelSettingsChange(currentLevelSettings);
        }
        Manageburst();
    }

    void Manageburst() {
        if (nextBurstIndex >= LevelSettings.spawningBursts.Length) return;        
        if(meterCounter.Value >= LevelSettings.spawningBursts[nextBurstIndex].score) {
        
            SpawningBurst burst = LevelSettings.spawningBursts[nextBurstIndex++];
            switch (burst.type) {
                case BurstType.Coin:
                    coinSpawner.StartBurst(burst.quantity, burst.time, burst.type);                    
                    break;
                case BurstType.Bubble:
                case BurstType.RedBubble:
                case BurstType.BlueBubble:
                    bubbleSpawner.StartBurst(burst.quantity, burst.time, burst.type);
                    break;
                case BurstType.Mine:
                case BurstType.BlueMine:
                case BurstType.RedMine:
                    mineSpawner.StartBurst(burst.quantity, burst.time, burst.type);
                    break;
                case BurstType.Rock:
                    rockSpawner.StartBurst(burst.quantity, burst.time, burst.type);
                    break;
            }
        }
    }

}
