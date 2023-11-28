using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GunLocation : MonoBehaviour
{

    private Transform arCameraTransform;
    [SerializeField]
    GameObject Gun;
    Camera arCamera;
    GameObject GunObject;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        GunObject = null;
        arCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        arCameraTransform = arCamera.transform;
        GunObject = Instantiate(Gun, arCameraTransform);

        GunObject.transform.localPosition = new Vector3(0.4f, -0.26f, 0.6f);
        GunObject.transform.localRotation = Quaternion.identity;
        GunObject.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
    }

}
