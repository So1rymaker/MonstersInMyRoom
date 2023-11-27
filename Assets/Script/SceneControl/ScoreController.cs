using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewBehaviourScript : MonoBehaviour
{
    public Text scoreText;
    public MonsterGenerate monsterGenerator;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Please assign the Text component in the inspector.");
        }

        if (monsterGenerator == null)
        {
            Debug.LogError("Please assign the MonsterGenerate component in the inspector.");
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
