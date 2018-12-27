using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    public int dataVersion = 1;
    public int bestScore = 0;
    public int coins = 0;
    public float magnetControllerHeight = -1f;
    public SystemLanguage lang = SystemLanguage.English;
}
