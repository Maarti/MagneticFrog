using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    public int dataVersion = 1;
    public int bestScore = -1;
    public int coins = 0;
    public float magnetControllerHeight = -1f;
    public SystemLanguage lang = SystemLanguage.English;
    public bool isTutorialDone = false;
    public Dictionary<CharacterId, CharacterSavedData> characters;
}
