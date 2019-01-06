using System;
using UnityEngine;

[Serializable]
public class CharacterSettings {
    public GameObject skin;
    public CharacterId id;
    public string name;
    public int agility;
    public int stamina;
    public int breath;
    public bool isUnlocked;

    public CharacterSettings(GameObject skin, CharacterId id, string name, int agility, int stamina, int breath, bool isUnlocked) {
        this.skin = skin;
        this.id = id;
        this.name = name;
        this.agility = agility;
        this.stamina = stamina;
        this.breath = breath;
        this.isUnlocked = isUnlocked;
    }
}

[Serializable]
public class CharacterSavedData {
    public int agility;
    public int stamina;
    public int breath;

    public CharacterSavedData(int agility, int stamina, int breath) {
        this.agility = agility;
        this.stamina = stamina;
        this.breath = breath;
    }
}

public enum CharacterId { GREEN, SKINNY }