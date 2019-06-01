using System;
using System.Collections.Generic;

[Serializable]
public class LevelSettings {
    public readonly static float LEVEL_10_SCORE = 0f;
    public readonly static float LEVEL_20_SCORE = 20f;
    public readonly static float LEVEL_30_SCORE = 100f;
    public readonly static float LEVEL_40_SCORE = 200f;
    public readonly static float LEVEL_50_SCORE = 300f;
    public readonly static float LEVEL_60_SCORE = 320f;
    public readonly static float LEVEL_70_SCORE = 400f;
    public readonly static float LEVEL_80_SCORE = 450f;
    public readonly static float LEVEL_90_SCORE = 500f;
    public readonly static float LEVEL_100_SCORE = 560f;
    public readonly static float[] DESCENDING_LEVEL_SCORES = { LEVEL_90_SCORE, LEVEL_80_SCORE, LEVEL_70_SCORE, LEVEL_60_SCORE, LEVEL_50_SCORE, LEVEL_40_SCORE, LEVEL_30_SCORE, LEVEL_20_SCORE, LEVEL_10_SCORE };
    public readonly static Dictionary<float, LevelSettings> levelSettings = new Dictionary<float, LevelSettings> {
//                                                                              coin        bubble      mineWait    mineSize    mineSpeed   rockWait    rockSize    rockSpeed
            { LEVEL_10_SCORE, new LevelSettings(LEVEL_10_SCORE, LEVEL_20_SCORE, 15f,16f,    .5f,2f,     -1f,-1f,    -1f,-1f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,1f  ) },
            { LEVEL_20_SCORE, new LevelSettings(LEVEL_20_SCORE, LEVEL_30_SCORE, 8f,12f,     1f,2.5f,    2f,4f,      .5f,.8f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,1f  ) },
            { LEVEL_30_SCORE, new LevelSettings(LEVEL_30_SCORE, LEVEL_40_SCORE, 6f, 10f,    1.5f, 3f,   2f,3f,      .75f,1.25f, 1f,2f,      -1f,-1f,    -1f,-1f,    1f,1f  ) },
            { LEVEL_40_SCORE, new LevelSettings(LEVEL_40_SCORE, LEVEL_50_SCORE, 6f, 9f,     2f, 3f,     2f,3f,      1.2f,2f,    1f,1.5f,    -1f,-1f,    -1f,-1f,    1f,1f  ) }, // slow big mines
            { LEVEL_50_SCORE, new LevelSettings(LEVEL_50_SCORE, LEVEL_60_SCORE, 15f, 16f,   0.25f,.5f, -1f,-1f,     -1f,-1f,    1f,1f,      -1f,-1f,    -1f,-1f,    1f,1f  ) }, // bubbles burst
            { LEVEL_60_SCORE, new LevelSettings(LEVEL_60_SCORE, LEVEL_70_SCORE, 5f, 8f,     1f, 3f,     1f,3f,      1f,1.5f,    1f,2f,      2f,4f,      .5f,1f,     1f,1f ) },  // rocks
            { LEVEL_70_SCORE, new LevelSettings(LEVEL_70_SCORE, LEVEL_80_SCORE, 5f, 8f,     1f, 2f,     1f,1.5f,    .25f,.5f,   1f,1f,      4f,6f,      .8f,1f,     1f,1f ) },  // lot of small mines
            { LEVEL_80_SCORE, new LevelSettings(LEVEL_80_SCORE, LEVEL_90_SCORE, 5f, 8f,     1f, 3f,     1f,3f,      1f,1.5f,    1f,2f,      2f,4f,      .5f,1f,     1f,1f ) },  // rocks
            { LEVEL_90_SCORE, new LevelSettings(LEVEL_90_SCORE, LEVEL_100_SCORE,4f, 7f,     2f, 3f,     2.5f,4f,    .75f,1f,    1f,2f,      .5f,1.5f,   .5f,2f,     1f,2f ) },  // lot of rocks
        };
    public readonly static SpawningBurst[] spawningBursts = {
        new SpawningBurst(20f ,BurstType.BlueBubble, 30, 0f),
        new SpawningBurst(100f ,BurstType.Coin, 5, 5f),
        new SpawningBurst(150f ,BurstType.RedMine, 15, 0f),
     /*   new SpawningBurst(30f ,BurstType.RedBubble, 10, 0f),
        new SpawningBurst(40f ,BurstType.Bubble, 10, 0f),
        new SpawningBurst(60f ,BurstType.BlueMine, 10, 0f),
        new SpawningBurst(70f ,BurstType.Mine, 10, 0f),
        new SpawningBurst(80f ,BurstType.Rock, 10, 0f),*/
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

    public LevelSettings(float scoreMin, float scoreMax, float coinMinWait, float coinMaxWait, float bubbleMinWait, float bubbleMaxWait,
        float mineMinWait, float mineMaxWait, float mineMinSize, float mineMaxSize, float mineMinSpeed, float mineMaxSpeed, float rockMinWait,
        float rockMaxWait, float rockMinSize, float rockMaxSize, float rockMinSpeed, float rockMaxSpeed) {
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

public enum BurstType { Mine, RedMine, BlueMine, Bubble, BlueBubble, RedBubble, Coin, Rock }