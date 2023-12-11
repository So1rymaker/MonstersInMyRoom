using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public MonsterGenerate monsterGenerator;
    public FirableMonsterGenerator FirablemonsterGenerator;

    public static int totalscore = 0;
    void Start()
    {
        totalscore = 0;
        if (scoreText == null)
        {
            Debug.LogError("scoreText == null");
        }

        if (monsterGenerator == null)
        {
            Debug.LogError("monsterGenerator == null");
        }
        if (FirablemonsterGenerator == null)
        {
            Debug.LogError("FirablemonsterGenerator == null");
        }
    }

    void Update()
    {
        if (scoreText != null && monsterGenerator != null && FirablemonsterGenerator != null)
        {
            totalscore = monsterGenerator.GetScore() + FirablemonsterGenerator.GetScore();
            scoreText.text = "Score: " + totalscore.ToString();
        }
    }
    public int GetTotalScore()
    {
        return totalscore;
    }

}
