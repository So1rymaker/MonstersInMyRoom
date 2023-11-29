using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas canvasRank;
    public Canvas canvasOrigin;

    void Start()
    {
        canvasRank.gameObject.SetActive(false);
        canvasOrigin.gameObject.SetActive(true);
    }

    public void ShowCanvasRank()
    {
        canvasRank.gameObject.SetActive(true);
        canvasOrigin.gameObject.SetActive(false);
    }
    public void ShowCanvasOrigin()
    {
        canvasRank.gameObject.SetActive(false);
        canvasOrigin.gameObject.SetActive(true);
    }
}
