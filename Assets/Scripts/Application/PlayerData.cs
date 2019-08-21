using System;
using System.Collections.Generic;
using UnityEngine;

// Don't remove field or players with older Data version wont be able to deserialize
[Serializable]
public class PlayerData {
    public int dataVersion = 1;
    public int bestScore = -1;
    public int coins = 0;
    public float? magnetControllerHeight = null;
    public float? magnetControllerAlpha = .8f;
    public SystemLanguage lang = SystemLanguage.English;
    public bool isTutorialDone = true; // TODO Switch to FALSE in production
    public Dictionary<CharacterId, CharacterSavedData> characters = new Dictionary<CharacterId, CharacterSavedData>();
    public CharacterId currentCharacater = CharacterSelector.DEFAULT_CHARACTER;
    public bool isPremium = false;  // no Ads
    public int nbPowerStart = 0;
    public DateTime lastMenuCoin = DateTime.MinValue;

    public override string ToString() {
        return string.Format("score={0} coins={1} tuto={2} chars={3}",bestScore,coins,isTutorialDone,characters.Count);
    }
}
