using UnityEngine;
using System.Collections;

public class SpiderSpawnManager : MonoBehaviour
{
    [Header("Spider Setup")]
    [SerializeField] private GameObject spiderPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

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

            if (spiderPrefab == null || spawnPoints.Length == 0)
            {
                Debug.LogWarning("Spider prefab or spawn points are missing!");
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

        GameObject newSpider = Instantiate(spiderPrefab, chosenSpawnPoint.position, Quaternion.identity);

        Spider spiderScript = newSpider.GetComponent<Spider>();
        if (spiderScript != null)
        {
            spiderScript.SetSpawnManager(this);
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