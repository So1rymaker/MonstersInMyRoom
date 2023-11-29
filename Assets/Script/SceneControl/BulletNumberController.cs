using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BulletNumberController : MonoBehaviour
{
    public Text bulletText;
    public ShootLine shootline;

    void Start()
    {
        if (bulletText == null)
        {
            Debug.LogError("bulletText == null");
        }

        if (shootline == null)
        {
            Debug.LogError("shootline == null");
        }
    }

    void Update()
    {
        if (bulletText != null && shootline != null)
        {
            bulletText.text = shootline.GetBulletNumber().ToString();
        }
    }
}
