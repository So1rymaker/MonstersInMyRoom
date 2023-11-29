using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameTimeController : MonoBehaviour
{
    public float countdownTime = 60f;
    public Text countdownText;

    public ScoreController scorecontroller;

    public static string scores;

    void Start()
    {
        StartCountdown();
    }

    void Update()
    {
        UpdateCountdownDisplay();
    }

    void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime -= 1f;

            UpdateCountdownDisplay();
        }
        AddScore(scorecontroller.GetTotalScore());
        SortAndSaveScores();
        SceneManager.LoadScene("GameOverUI");
    }

    void UpdateCountdownDisplay()
    {
        countdownText.text = "Time: " + Mathf.RoundToInt(countdownTime).ToString();
    }

    void AddScore(int newScore)
    {
        string savedScores = PlayerPrefs.GetString("AllScores", "");
        if (!string.IsNullOrEmpty(savedScores))
        {
            scores = newScore.ToString() + "," + savedScores;
        }
        else
        {
            scores = newScore.ToString();
        }
    }

    void SortAndSaveScores()
    {
        int[] scoreArray = Array.ConvertAll(scores.Split(','), int.Parse);

        Array.Sort(scoreArray, (a, b) => b.CompareTo(a));

        scores = string.Join(",", scoreArray.Select(x => x.ToString()).ToArray());

        PlayerPrefs.SetString("AllScores", scores);
        PlayerPrefs.Save();
    }

}

