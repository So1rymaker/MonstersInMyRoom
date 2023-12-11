using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GameOverUIController : MonoBehaviour
{
    public Text allScoresText;



    void Start()
    {
        string scoresString;

        if (PlayerPrefs.GetInt("FAIL") == 1)
        {
            scoresString = "GAME OVER!\nYOU LOSE!";
        }
        else
        {
            scoresString = "GAME OVER!\nYOU FINAL SCORE:" + PlayerPrefs.GetString("LastTimeScore", "");
        }
        allScoresText.text = scoresString;
    }
}
