using System;
using UnityEngine;

[Serializable]
public class CharacterSettings {
    public static int MAX_STAT = 3;                     // Max points in a stat
    public static int[] STATS_COST = { 10, 40, 100 };   // Cost of the 1st, 2nd, 3rd... point in a stat
    public GameObject skin;
    public CharacterId id;
    public string name;
    [Range(0, 3)] public int agility;
    [Range(0, 3)] public int stamina;
    [Range(0, 3)] public int breath;
    public int cost;
    public bool isUnlocked;

    public CharacterSettings(GameObject skin, CharacterId id, string name, int agility, int stamina, int breath, int cost, bool isUnlocked) {
        this.skin = skin;
        this.id = id;
        this.name = name;
        this.agility = agility;
        this.stamina = stamina;
        this.breath = breath;
        this.cost = cost;
        this.isUnlocked = isUnlocked;
    }
}

[Serializable]
public struct CharacterSavedData {
    public int agility;
    public int stamina;
    public int breath;
}

public enum CharacterId { HYLA, GOLIATH, PACMAN, POISON_PYRO, POISON_AQUA, POISON_VOLT, GOTTLEBEI, GOLDEN, DARWIN, DOUGLASS, FROBOT, MEGOPHRYS }