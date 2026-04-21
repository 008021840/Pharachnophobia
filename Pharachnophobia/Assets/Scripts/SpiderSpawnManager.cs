using UnityEngine;
using System.Collections;

public class SpiderSpawnManager : MonoBehaviour
{
    [Header("Spider Setup")]
    [SerializeField] private GameObject spiderPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Target (Bed)")]
    [SerializeField] private Transform bedTarget;

    [Header("Managers")]
    [SerializeField] private SanityManager sanityManager;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 2.5f;

    [Header("Spawn Limits")]
    [SerializeField] private int maxSpidersAlive = 10;

    private int currentSpidersAlive = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            if (spiderPrefab == null || spawnPoints.Length == 0 || bedTarget == null || sanityManager == null)
            {
                Debug.LogWarning("Missing spider prefab, spawn points, bed target, or sanity manager!");
                continue;
            }

            if (currentSpidersAlive >= maxSpidersAlive)
            {
                continue;
            }

            SpawnSpider();
        }
    }

    private void SpawnSpider()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawnPoint = spawnPoints[randomIndex];

        GameObject newSpider = Instantiate(
            spiderPrefab,
            chosenSpawnPoint.position,
            Quaternion.identity
        );

        Spider spiderScript = newSpider.GetComponent<Spider>();

        if (spiderScript != null)
        {
            spiderScript.SetTarget(bedTarget);
            spiderScript.SetSpawnManager(this);
            spiderScript.SetSanityManager(sanityManager);
        }
        else
        {
            Debug.LogWarning("Spawned spider prefab does not have Spider.cs attached!");
        }

        currentSpidersAlive++;
    }

    public void SpiderDestroyed()
    {
        currentSpidersAlive--;

        if (currentSpidersAlive < 0)
        {
            currentSpidersAlive = 0;
        }
    }
}