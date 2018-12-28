using System;
using System.Collections.Generic;

[Serializable]
public class LevelSettings {
    public readonly static float LEVEL_10_SCORE = 0f;
    public readonly static float LEVEL_20_SCORE = 50f;
    public readonly static float LEVEL_30_SCORE = 150f;
    public readonly static float LEVEL_40_SCORE = 250f;
    public readonly static float[] DESCENDING_LEVEL_SCORES = { LEVEL_30_SCORE, LEVEL_20_SCORE, LEVEL_10_SCORE };
    public readonly static Dictionary<float, LevelSettings> levelSettings = new Dictionary<float, LevelSettings> {
            { LEVEL_10_SCORE, new LevelSettings(LEVEL_10_SCORE, LEVEL_20_SCORE, .5f,2f,     -1f,-1f,    -1f,-1f) },
            { LEVEL_20_SCORE, new LevelSettings(LEVEL_20_SCORE, LEVEL_30_SCORE, 1.5f,3f,    2f,4f,      .5f,1f) },
            { LEVEL_30_SCORE, new LevelSettings(LEVEL_30_SCORE, LEVEL_40_SCORE, 2f, 4f,     1f,3f,      .75f,2f) },
        };

    public float scoreMin;
    public float scoreMax;
    public float bubbleMinWait;
    public float bubbleMaxWait;
    public float mineMinWait;
    public float mineMaxWait;
    public float mineMinSize;
    public float mineMaxSize;

    /* t minWait = 2f, maxWait = 4f;
     public float minSize = .5f, maxSize = 2f;*/

    public LevelSettings(float scoreMin, float scoreMax, float bubbleMinWait, float bubbleMaxWait, float mineMinWait,
        float mineMaxWait, float mineMinSize, float mineMaxSize) {
        this.scoreMin = scoreMin;
        this.scoreMax = scoreMax;
        this.bubbleMinWait = bubbleMinWait;
        this.bubbleMaxWait = bubbleMaxWait;
        this.mineMinWait = mineMinWait;
        this.mineMaxWait = mineMaxWait;
        this.mineMinSize = mineMinSize;
        this.mineMaxSize = mineMaxSize;
    }

    public static LevelSettings GetLevelSettingsScore(float score) {
        foreach (float levelScore in DESCENDING_LEVEL_SCORES) {
            if (score >= levelScore)
                return levelSettings[levelScore];
        }
        return levelSettings[LEVEL_10_SCORE];
    }

}
