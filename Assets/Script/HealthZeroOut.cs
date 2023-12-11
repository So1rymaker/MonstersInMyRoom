using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class HealthZeroOut : MonoBehaviour
{
    public static int FailOrNot = 0;

    public static void SetFailOrNot(int value)
    {
        FailOrNot = value;
        if (FailOrNot == 1)
        {
            PlayerPrefs.SetInt("FAIL", FailOrNot);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameOverUI");
        }
        else
        {
            PlayerPrefs.SetInt("FAIL", FailOrNot);
            PlayerPrefs.Save();
        }
    }
}
