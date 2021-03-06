﻿using System.Collections;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour {

    [SerializeField] protected float minPosX = -2f;
    [SerializeField] protected float maxPosX = 2f;
    [SerializeField] protected LevelSettings levelSettings;
    protected bool isSpwaningDuringThisLevel = true;

    void OnEnable() {
        LevelSettingsController.OnLevelSettingsChange += UpdateLevelSettings;
        UpdateLevelSettings(LevelSettingsController.currentLevelSettings);
        StartCoroutine(SpawningRoutine());
    }

    void OnDisable() {
        LevelSettingsController.OnLevelSettingsChange -= UpdateLevelSettings;
        StopAllCoroutines();
    }

    void UpdateLevelSettings(LevelSettings newLevelSettings) {
        levelSettings = newLevelSettings;
        UpdateIsSpwaningDuringThisLevel();
    }

    protected abstract void UpdateIsSpwaningDuringThisLevel();

    protected abstract IEnumerator SpawningRoutine();

    protected abstract IEnumerator Burst(int quantity, float timeInSeconds, BurstType burstType);

    public virtual void StartBurst(int quantity, float timeInSeconds, BurstType burstType) {
        StartCoroutine(Burst(quantity, timeInSeconds, burstType));
    }

}
