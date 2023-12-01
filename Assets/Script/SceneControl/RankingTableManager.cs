using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RankingTableManager : MonoBehaviour
{
    public GameObject tableCellPrefab;

    void Start()
    {
        CreateTable();
    }

    void CreateTable()
    {
        string[] headers = { "NO", "1", "2", "3", "4", "5",};
        string[] scores = GetTop5Scores();
        for (int i = 0; i < scores.Length; i++)
        {
            GameObject cell = Instantiate(tableCellPrefab, transform);

            Text cellText = cell.GetComponent<Text>();
            cellText.text = headers[i];

            GameObject cellscore = Instantiate(tableCellPrefab, transform);

            Text cellscoreText = cellscore.GetComponent<Text>();
            cellscoreText.text = scores[i];
        }
    }
    string[] GetTop5Scores()
    {
        string allScores = PlayerPrefs.GetString("AllScores", "");
        string[] scoreArray = allScores.Split(',');

        if (scoreArray.Length > 1)
        {
            List<int> intScores = scoreArray.Skip(1).Select(int.Parse).ToList();

            List<int> top5Scores = intScores.OrderByDescending(x => x).Take(5).ToList();

            string[] top5ScoresArray = new string[top5Scores.Count + 1];
            top5ScoresArray[0] = "SCORE";
            for (int i = 0; i < top5Scores.Count; i++)
            {
                top5ScoresArray[i + 1] = top5Scores[i].ToString();
            }

            return top5ScoresArray;
        }
        else
        {
            return new string[] { "SCORE" };
        }
    }
}

