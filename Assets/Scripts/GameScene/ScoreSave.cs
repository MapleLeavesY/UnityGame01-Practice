using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSave : MonoBehaviour
{
    public static ScoreSave Instance
    {
        private set;
        get;
    }
    private const float TOTALSCORENORMAL = 0;
    private static float totalScore = TOTALSCORENORMAL;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void AddScore(float score)
    {
        totalScore += score;
    }
    public float GetTotalScore()
    {
        return totalScore;
    }

    public static void InitTotalScore()
    {
        totalScore = TOTALSCORENORMAL;
    }
}
