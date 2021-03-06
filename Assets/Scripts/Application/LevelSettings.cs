﻿using System;
using System.Collections.Generic;

[Serializable]
public class LevelSettings {
    public readonly static float LEVEL_10_SCORE = 0f;
    public readonly static float LEVEL_20_SCORE = 20f;
    public readonly static float LEVEL_30_SCORE = 100f;
    public readonly static float LEVEL_40_SCORE = 200f;
    // public readonly static float LEVEL_50_SCORE = 300f;
    public readonly static float LEVEL_60_SCORE = 320f;
    public readonly static float LEVEL_70_SCORE = 400f;
    public readonly static float LEVEL_80_SCORE = 450f;
    public readonly static float LEVEL_90_SCORE = 500f;
    public readonly static float LEVEL_100_SCORE = 560f;
    public readonly static float LEVEL_110_SCORE = 620f;
    public readonly static float LEVEL_120_SCORE = 700f;
    public readonly static float LEVEL_130_SCORE = 760f;
    public readonly static float LEVEL_140_SCORE = 900f;
    public readonly static float LEVEL_150_SCORE = 1000f;
    public readonly static float LEVEL_160_SCORE = 1050f;
    public readonly static float LEVEL_170_SCORE = 1100f;
    public readonly static float LEVEL_180_SCORE = 1200f;
    public readonly static float LEVEL_190_SCORE = 1300f;
    public readonly static float[] DESCENDING_LEVEL_SCORES = { LEVEL_180_SCORE, LEVEL_170_SCORE, LEVEL_160_SCORE, LEVEL_150_SCORE, LEVEL_140_SCORE, LEVEL_130_SCORE, LEVEL_120_SCORE, LEVEL_110_SCORE, LEVEL_100_SCORE, LEVEL_90_SCORE, LEVEL_80_SCORE, LEVEL_70_SCORE, LEVEL_60_SCORE, LEVEL_40_SCORE, LEVEL_30_SCORE, LEVEL_20_SCORE, LEVEL_10_SCORE };
    public readonly static Dictionary<float, LevelSettings> levelSettings = new Dictionary<float, LevelSettings> {
//                                                                              coin        bubble      mineWait    mineSize    mineSpeed   rockWait    rockSize    rockSpeed   trashes
            { LEVEL_10_SCORE, new LevelSettings(LEVEL_10_SCORE, LEVEL_20_SCORE, 15f,16f,    .5f,2f,     -1f,-1f,    -1f,-1f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,1f,      0f, 0f  ) },
            { LEVEL_20_SCORE, new LevelSettings(LEVEL_20_SCORE, LEVEL_30_SCORE, 8f,12f,     1f,2.5f,    2f,4f,      .5f,.8f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,1f,      0f, 0f  ) },
            { LEVEL_30_SCORE, new LevelSettings(LEVEL_30_SCORE, LEVEL_40_SCORE, 6f, 10f,    1.5f, 3f,   2f,3f,      .75f,1.25f, 1f,2f,      -1f,-1f,    -1f,-1f,    1f,1f,      0f, 0f  ) },
            { LEVEL_40_SCORE, new LevelSettings(LEVEL_40_SCORE, LEVEL_60_SCORE, 6f, 9f,     2f, 3f,     2f,3f,      1.2f,2f,    1f,1.5f,    -1f,-1f,    -1f,-1f,    1f,1f,      0f, 0f  ) }, // slow big mines         
            { LEVEL_60_SCORE, new LevelSettings(LEVEL_60_SCORE, LEVEL_70_SCORE, 5f, 8f,     1f, 3f,     1f,3f,      1f,1.5f,    1f,2f,      2f,4f,      .5f,1f,     1f,1f,      3f, 5f ) },  // rocks + trashes
            { LEVEL_70_SCORE, new LevelSettings(LEVEL_70_SCORE, LEVEL_80_SCORE, 5f, 8f,     1f, 2f,     1f,1.5f,    .25f,.5f,   1f,1f,      4f,6f,      .8f,1f,     1f,1f,      0f, 0f ) },  // lot of small mines
            { LEVEL_80_SCORE, new LevelSettings(LEVEL_80_SCORE, LEVEL_90_SCORE, 5f, 8f,     1f, 3f,     1f,3f,      1f,1.5f,    1f,2f,      2f,4f,      .5f,1f,     1f,1f,      0f, 2f ) },  // rocks
            { LEVEL_90_SCORE, new LevelSettings(LEVEL_90_SCORE, LEVEL_100_SCORE,4f, 7f,     2f, 3f,     2.5f,4f,    .75f,1f,    1f,2f,      .5f,1.5f,   .5f,2f,     1f,2f,      0f, 0f ) },  // lot of rocks
            { LEVEL_100_SCORE,new LevelSettings(LEVEL_100_SCORE,LEVEL_110_SCORE,3f, 5f,     1f, 3f,     0.5f,1.5f,  .75f,1.25f, 1f,2f,      -1f,-1f,    -1f,-1f,    1f,1f,      1f, 3f ) },  // lot of mines
            { LEVEL_110_SCORE,new LevelSettings(LEVEL_110_SCORE,LEVEL_120_SCORE,3f, 5f,     4f, 4f,     -1f,-1f,    .75f,1.25f, 1f,2f,      .5f,.75f,   .8f,1.25f,  1f,2f,      1f, 3f ) },  // low bubble + rocks
            { LEVEL_120_SCORE,new LevelSettings(LEVEL_120_SCORE,LEVEL_130_SCORE,3f, 5f,     4f, 4f,     .5f,1.5f,   .75f,1.25f, 1f,2f,      -1f,-1f,    -1f,-1f,    1f,2f,      1f, 3f ) },  // low bubble + mines
            // Mines are doubled from 800
            { LEVEL_130_SCORE,new LevelSettings(LEVEL_130_SCORE,LEVEL_140_SCORE,3f, 4f,     1f, 3f,     1f,2f,      .75f,1.25f, 1f,1f,      -1f,-1f,    -1f,-1f,    1f,2f,      1f, 3f ) },  // calm double mines discovery
            { LEVEL_140_SCORE,new LevelSettings(LEVEL_140_SCORE,LEVEL_150_SCORE,3f, 4f,     2f, 3f,     1f,2f,      1.2f,2f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,2f,      1f, 3f ) },  // big mines
            { LEVEL_150_SCORE,new LevelSettings(LEVEL_150_SCORE,LEVEL_160_SCORE,1f, 4f,     2f, 3f,     1f,2f,      1.2f,1.5f,  3f,3f,      -1f,-1f,    -1f,-1f,    1f,2f,      1f, 3f ) },  // fast big mines
            { LEVEL_160_SCORE,new LevelSettings(LEVEL_160_SCORE,LEVEL_170_SCORE,2f, 3f,     2f, 3f,     1f,2f,      1f,1.5f,    2f,2f,      2f,3f,      .8f,1f,     1f,2f,      1f, 3f ) },  // rock + mines
            { LEVEL_170_SCORE,new LevelSettings(LEVEL_170_SCORE,LEVEL_180_SCORE,2f, 3f,     2f, 3f,     1f,2f,      1f,1f,      2f,2f,      2f,3f,      .8f,1f,     1f,2f,      0f, 1f ) },  // rock + mines + lot of thrash
            { LEVEL_180_SCORE,new LevelSettings(LEVEL_180_SCORE,LEVEL_190_SCORE,0f, 1f,     2f, 3f,     1f,2f,      1f,1f,      2f,2f,      0.5f,1f,    .5f,1f,     1f,2f,      0f, 1f ) },  // lot of rocks + mines + lot of thrash
            // Mines are tripled from 1600
            // Mines are quadrupled from 2500
        };
    public readonly static SpawningBurst[] spawningBursts = {
        new SpawningBurst(20f ,BurstType.BlueBubble, 12, 2f),
        new SpawningBurst(85f ,BurstType.Bottle, 3, 1f),
        new SpawningBurst(100f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(150f ,BurstType.RedMine, 6, 2f),
        new SpawningBurst(200f ,BurstType.Trashes, 10, 1f),
        new SpawningBurst(210f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(300f ,BurstType.BlueBubble, 20, 0f),
        new SpawningBurst(310f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(350f ,BurstType.Rock, 3, 5f),
        new SpawningBurst(410f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(510f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(610f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(690f ,BurstType.RedBubble, 20, 1f),
        new SpawningBurst(710f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(800f ,BurstType.Trashes, 30, 3f),
        new SpawningBurst(810f ,BurstType.Coin, 5, 7f),
        new SpawningBurst(910f ,BurstType.Coin, 5, 7f),     
    };

    public float scoreMin;
    public float scoreMax;
    public float coinMinWait;
    public float coinMaxWait;
    public float bubbleMinWait;
    public float bubbleMaxWait;
    public float mineMinWait;
    public float mineMaxWait;
    public float mineMinSize;
    public float mineMaxSize;
    public float mineMinSpeed;
    public float mineMaxSpeed;
    public float rockMinWait;
    public float rockMaxWait;
    public float rockMinSize;
    public float rockMaxSize;
    public float rockMinSpeed;
    public float rockMaxSpeed;
    public float trashMinWait;
    public float trashMaxWait;

    public LevelSettings(float scoreMin, float scoreMax, float coinMinWait, float coinMaxWait, float bubbleMinWait, float bubbleMaxWait,
        float mineMinWait, float mineMaxWait, float mineMinSize, float mineMaxSize, float mineMinSpeed, float mineMaxSpeed, float rockMinWait,
        float rockMaxWait, float rockMinSize, float rockMaxSize, float rockMinSpeed, float rockMaxSpeed, float trashMinWait, float trashMaxWait ) {
        this.scoreMin = scoreMin;
        this.scoreMax = scoreMax;
        this.coinMinWait = coinMinWait;
        this.coinMaxWait = coinMaxWait;
        this.bubbleMinWait = bubbleMinWait;
        this.bubbleMaxWait = bubbleMaxWait;
        this.mineMinWait = mineMinWait;
        this.mineMaxWait = mineMaxWait;
        this.mineMinSize = mineMinSize;
        this.mineMaxSize = mineMaxSize;
        this.mineMinSpeed = mineMinSpeed;
        this.mineMaxSpeed = mineMaxSpeed;
        this.rockMinWait = rockMinWait;
        this.rockMaxWait = rockMaxWait;
        this.rockMinSize = rockMinSize;
        this.rockMaxSize = rockMaxSize;
        this.rockMinSpeed = rockMinSpeed;
        this.rockMaxSpeed = rockMaxSpeed;
        this.trashMinWait = trashMinWait;
        this.trashMaxWait = trashMaxWait;
    }

    public static LevelSettings GetLevelSettingsScore(float score) {
        foreach (float levelScore in DESCENDING_LEVEL_SCORES) {
            if (score >= levelScore)
                return levelSettings[levelScore];
        }
        return levelSettings[LEVEL_10_SCORE];
    }

}

[Serializable]
public class SpawningBurst {
    public BurstType type = BurstType.Bubble;
    public float score = -1;
    public int quantity = 10;
    public float time = 0f;

    public SpawningBurst(float score, BurstType type, int quantity, float time) {
        this.type = type;
        this.score = score;
        this.quantity = quantity;
        this.time = time;
    }
}

public enum BurstType { Mine, RedMine, BlueMine, Bubble, BlueBubble, RedBubble, Coin, Rock, Trashes, Bottle  }