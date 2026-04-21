using UnityEngine;
using System.Collections;

public class GhostSpawnManager : MonoBehaviour
{
    [Header("Ghost Setup")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private SanityManager sanityManager;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 5f;
    [SerializeField] private float maxSpawnDelay = 10f;

    [Header("Spawn Limits")]
    [SerializeField] private int maxGhostsAlive = 1;

    [Header("Screen Spawn Padding")]
    [SerializeField] private float screenPadding = 1f;

    private Camera mainCamera;
    private int currentGhostsAlive = 0;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            if (ghostPrefab == null || sanityManager == null || mainCamera == null)
            {
                Debug.LogWarning("Missing ghost prefab, sanity manager, or main camera!");
                continue;
            }

            if (currentGhostsAlive >= maxGhostsAlive)
            {
                continue;
            }

            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        Vector3 spawnPosition = GetRandomScreenPosition();

        GameObject newGhost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);

        Ghost ghostScript = newGhost.GetComponent<Ghost>();
        if (ghostScript != null)
        {
            ghostScript.SetSanityManager(sanityManager);
            ghostScript.SetSpawnManager(this);
        }
        else
        {
            Debug.LogWarning("Spawned ghost prefab does not have Ghost.cs attached!");
        }

        currentGhostsAlive++;
    }

    private Vector3 GetRandomScreenPosition()
    {
        float zDistance = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, zDistance));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, zDistance));

        float randomX = Random.Range(bottomLeft.x + screenPadding, topRight.x - screenPadding);
        float randomY = Random.Range(bottomLeft.y + screenPadding, topRight.y - screenPadding);

        return new Vector3(randomX, randomY, 0f);
    }

    public void GhostDestroyed()
    {
        currentGhostsAlive--;

        if (currentGhostsAlive < 0)
        {
            currentGhostsAlive = 0;
        }
    }
}