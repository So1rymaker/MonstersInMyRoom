using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GameOverUIController : MonoBehaviour
{
    public Text allScoresText;

    void Start()
    {
        List<int> allScores = LoadScores();

        allScoresText.text = "All Scores: " + string.Join(", ", allScores.Select(x => x.ToString()).ToArray());
    }

    List<int> LoadScores()
    {
        string scoresString = PlayerPrefs.GetString("AllScores", "");
        List<int> allScores = new List<int>();

        if (!string.IsNullOrEmpty(scoresString))
        {
            string[] scoreStrings = scoresString.Split(',');
            foreach (string scoreStr in scoreStrings)
            {
                int score;
                if (int.TryParse(scoreStr, out score))
                {
                    allScores.Add(score);
                }
            }
        }

        return allScores;
    }
}
