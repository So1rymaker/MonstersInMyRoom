using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimeController : MonoBehaviour
{
    public float countdownTime = 60f;
    public Text countdownText;

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

        SceneManager.LoadScene("GameOverUI");
    }

    void UpdateCountdownDisplay()
    {
        countdownText.text = "Time: " + Mathf.RoundToInt(countdownTime).ToString();
    }
}

