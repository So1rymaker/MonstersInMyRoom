using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public MonsterGenerate monsterGenerator;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("scoreText == null");
        }

        if (monsterGenerator == null)
        {
            Debug.LogError("monsterGenerator == null");
        }
    }

    void Update()
    {
        if (scoreText != null && monsterGenerator != null)
        {
            scoreText.text = "Score: " + monsterGenerator.GetScore().ToString();
        }
    }

}
