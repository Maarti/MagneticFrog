using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    public int dataVersion = 1;
    public int bestScore = -1;
    public int coins = 0;
    public float? magnetControllerHeight = null;
    public float? magnetControllerAlpha = .8f;
    public SystemLanguage lang = SystemLanguage.English;
    public bool isTutorialDone = false;
    public Dictionary<CharacterId, CharacterSavedData> characters = new Dictionary<CharacterId, CharacterSavedData>();
    public CharacterId currentCharacater = CharacterSelector.DEFAULT_CHARACTER;
    public bool isPremium = false;  // no Ads
    public DateTime bonusesActivationTime = DateTime.MinValue;

    public override string ToString() {
        return string.Format("score={0} coins={1} tuto={2} chars={3}",bestScore,coins,isTutorialDone,characters.Count);
    }
}
