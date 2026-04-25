using UnityEngine;
using System.Collections;

public class SpiderSpawnManager : MonoBehaviour
{
    [Header("Spider Setup")]
    [SerializeField] private GameObject spiderPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Target")]
    [SerializeField] private Transform bedTarget;

    [Header("Managers")]
    [SerializeField] private SanityManager sanityManager;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float maxSpawnDelay = 2.5f;

    [Header("Spawn Limits")]
    [SerializeField] private int maxSpidersAlive = 10;

    [Header("Ceiling Spiders")]
    [SerializeField] private float ceilingY = 5f;
    [SerializeField] private float dropStopY = 2f;

    [Range(0f, 1f)]
    [SerializeField] private float ceilingSpiderChance = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float fastDropChance = 0f;

    private int currentSpidersAlive = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            if (spiderPrefab == null || spawnPoints.Length == 0 || bedTarget == null || sanityManager == null)
            {
                Debug.LogWarning("Missing references in SpiderSpawnManager!");
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
        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPosition = chosenSpawnPoint.position;

        bool isCeilingSpider = Random.value < ceilingSpiderChance;
        bool isFastDropSpider = isCeilingSpider && Random.value < fastDropChance;

        Spider.SpiderMoveType chosenMoveType = Spider.SpiderMoveType.CrawlToTarget;

        if (isCeilingSpider)
        {
            spawnPosition.y = ceilingY;

            chosenMoveType = isFastDropSpider
                ? Spider.SpiderMoveType.FastDropAttack
                : Spider.SpiderMoveType.DropThenCrawl;
        }

        GameObject newSpider = Instantiate(spiderPrefab, spawnPosition, Quaternion.identity);

        Spider spider = newSpider.GetComponent<Spider>();

        if (spider != null)
        {
            spider.Initialize(
                bedTarget,
                this,
                sanityManager,
                chosenMoveType,
                dropStopY
            );
        }
        else
        {
            Debug.LogWarning("Spider prefab missing Spider.cs!");
        }

        Debug.Log("Spawned spider type: " + chosenMoveType);

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