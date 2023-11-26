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

    int score = 0;
    float monsterLifetime = 5f;
    float rushDuration = 1f;
    float rushDistance = -0.1f;


    void Start()
    {
        arCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        // Get the boundary points of the plane in local space
        Vector3[] boundaryPoints = new Vector3[plane.boundary.Length];
        for (int i = 0; i < plane.boundary.Length; i++)
        {
            boundaryPoints[i] = plane.transform.TransformPoint(new Vector3(plane.boundary[i].x, 0f, plane.boundary[i].y));
        }

        // Calculate the bounds of the plane
        Bounds planeBounds = new Bounds(boundaryPoints[0], Vector3.zero);
        for (int i = 1; i < boundaryPoints.Length; i++)
        {
            planeBounds.Encapsulate(boundaryPoints[i]);
        }

        // Generate a random position within the bounds of the plane
        float randomX = Random.Range(planeBounds.min.x, planeBounds.max.x);
        float randomZ = Random.Range(planeBounds.min.z, planeBounds.max.z);

        // Return the random position with the Y coordinate set to the plane's height
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
                // Check if the touched position is on any of the spawned monsters
                CheckAndDestroyMonster(hit.point);
            }
        }
    }

    void CheckAndDestroyMonster(Vector3 touchedPosition)
    {
        // Create a ray from arCamera position towards touchedPosition
        Ray rayFromCamera = new Ray(arCamera.transform.position, touchedPosition - arCamera.transform.position);

        LayerMask monsterLayerMask = LayerMask.GetMask("MonsterLayer");

        foreach (var monster in spawnedMonsters)
        {
            // Check if the ray hits the monster's collider, considering only the specified layer
            RaycastHit hit;
            if (Physics.Raycast(rayFromCamera, out hit, Mathf.Infinity, monsterLayerMask) && hit.collider == monster.GetComponent<Collider>())
            {
                // Destroy the monster
                Destroy(monster);
                // Remove the reference from the list
                spawnedMonsters.Remove(monster);
                score += 1;
                break; // Exit the loop after destroying one monster
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

            yield return null;
        }

        Destroy(monster);
        spawnedMonsters.Remove(monster);
    }

}
