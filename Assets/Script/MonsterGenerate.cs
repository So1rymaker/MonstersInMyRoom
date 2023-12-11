using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MonsterGenerate : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    [SerializeField]
    GameObject Monster;
    Camera arCamera;

    [SerializeField]
    ARPlaneManager m_PlaneManager;

    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public static int score1 = 0;
    float monsterLifetime = 5f;
    float rushDuration = 1f;
    float rushDistance = -0.1f;

    public ProgressBar HealthBar;
    public Canvas RedCanvas;

    public ShootLine bulletzero;

    void Start()
    {
        score1 = 0;
        RedCanvas.gameObject.SetActive(false);
        arCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        HealthBar.BarValue = 100;


        StartCoroutine(GenerateMonsterWithDelay(3f));
    }
    IEnumerator GenerateMonsterWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (TryGetRandomPositionOnPlane(out Vector3 spawnPosition))
            {
                GameObject monsterInstance = Instantiate(Monster, spawnPosition, Quaternion.identity);
                monsterInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Vector3 lookAtCamera = arCamera.transform.position - spawnPosition;
                monsterInstance.transform.rotation = Quaternion.LookRotation(lookAtCamera.normalized);


                spawnedMonsters.Add(monsterInstance);
                StartCoroutine(MonsterLifecycle(monsterInstance));
            }

        }
    }

    bool TryGetRandomPositionOnPlane(out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;
        Ray ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (m_RaycastManager.Raycast(ray, new List<ARRaycastHit>()))
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            m_RaycastManager.Raycast(ray, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            foreach (var hit in hits) {
                ARPlane MonsterPlane = m_PlaneManager.GetPlane(hit.trackableId);
                if (MonsterPlane != null)
                {
                    Debug.Log("Detected plane with ID: " + MonsterPlane.trackableId);
                    Vector3 randomPositionInPlane = GenerateRandomPositionInPlane(MonsterPlane);
                    spawnPosition = randomPositionInPlane;
                    return true;
                }
            }
                
        }
        return false;
    }

    Vector3 GenerateRandomPositionInPlane(ARPlane plane)
    {
        Vector3[] boundaryPoints = new Vector3[plane.boundary.Length];
        for (int i = 0; i < plane.boundary.Length; i++)
        {
            boundaryPoints[i] = plane.transform.TransformPoint(new Vector3(plane.boundary[i].x, 0f, plane.boundary[i].y));
        }

        Bounds planeBounds = new Bounds(boundaryPoints[0], Vector3.zero);
        for (int i = 1; i < boundaryPoints.Length; i++)
        {
            planeBounds.Encapsulate(boundaryPoints[i]);
        }

        float randomX = Random.Range(planeBounds.min.x, planeBounds.max.x);
        float randomZ = Random.Range(planeBounds.min.z, planeBounds.max.z);

        return new Vector3(randomX, plane.transform.position.y, randomZ);
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckAndDestroyMonster(hit.point);
            }
        }
    }

    void CheckAndDestroyMonster(Vector3 touchedPosition)
    {
        Ray rayFromCamera = new Ray(arCamera.transform.position, touchedPosition - arCamera.transform.position);

        LayerMask monsterLayerMask = LayerMask.GetMask("MonsterLayer");

        foreach (var monster in spawnedMonsters)
        {
            RaycastHit hit;
            int bulletnumber = bulletzero.GetBulletNumber();
            if (Physics.Raycast(rayFromCamera, out hit, Mathf.Infinity, monsterLayerMask) && hit.collider == monster.GetComponent<Collider>() && bulletnumber!=0)
            {
                Destroy(monster);
                spawnedMonsters.Remove(monster);
                score1 += 1;
           
                break;
            }
        }
    }

    IEnumerator MonsterLifecycle(GameObject monster)
    {
        yield return new WaitForSeconds(monsterLifetime);

        float elapsedTime = 0f;
        float startRushTime = Random.Range(1.5f, 2.5f);
        while (elapsedTime < rushDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / rushDuration);

            float speedMultiplier = Mathf.SmoothStep(0f, 1f, progress);

            Vector3 currentPosition = monster.transform.position;
            Vector3 targetPosition = arCamera.transform.position + arCamera.transform.forward * rushDistance;

            monster.transform.position = Vector3.Lerp(currentPosition, targetPosition, speedMultiplier);

            float rotationSpeed = 1800f;
            float rotationAngle = rotationSpeed * Time.deltaTime;
            monster.transform.Rotate(Vector3.up, rotationAngle);

            yield return null;
        }
        RedCanvas.gameObject.SetActive(true);
        HealthBar.BarValue -= 10;
        if (HealthBar.BarValue <= 0)
        {
            HealthZeroOut.SetFailOrNot(1);
        }
        else
        {
            HealthZeroOut.SetFailOrNot(0);
        }
        yield return new WaitForSeconds(0.2f);
        RedCanvas.gameObject.SetActive(false);
        Destroy(monster);
        spawnedMonsters.Remove(monster);
    }
    public int GetScore()
    {
        return score1;
    }

}
