using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ShootLine : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    Camera arCamera;

    public Material ShootlineMaterial;
    private bool isGeneratingLine = false;
    Vector3 StartPosition;
    Vector3 EndPosition;

    void Start()
    {
        arCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;
        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        StartPosition = arCamera.transform.position + arCamera.transform.TransformDirection(new Vector3(0.4f, -0.028f, 1.35f));
        if (m_RaycastManager.Raycast(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f), m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    EndPosition = m_Hits[0].pose.position;
                    StartCoroutine(GenerateLine());
                }
            }
        }
    }
    IEnumerator GenerateLine()
    {
        isGeneratingLine = true;

        LineRenderer ShootlineRenderer = gameObject.AddComponent<LineRenderer>();
        ShootlineRenderer.material = ShootlineMaterial;

        ShootlineRenderer.positionCount = 2;
        ShootlineRenderer.SetPosition(0, StartPosition);
        ShootlineRenderer.SetPosition(1, EndPosition);

        ShootlineRenderer.startColor = Color.yellow;
        ShootlineRenderer.endColor = Color.yellow;

        ShootlineRenderer.startWidth = 0.005f;
        ShootlineRenderer.endWidth = 0.005f;
        yield return new WaitForSeconds(0.05f);

        Destroy(ShootlineRenderer);

        isGeneratingLine = false;
    }
}
