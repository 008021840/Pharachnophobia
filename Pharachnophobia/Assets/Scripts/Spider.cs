using UnityEngine;

public class Spider : MonoBehaviour
{
    private SpiderSpawnManager spawnManager;

    public void SetSpawnManager(SpiderSpawnManager manager)
    {
        spawnManager = manager;
    }

    private void OnDestroy()
    {
        if (spawnManager != null)
        {
            spawnManager.SpiderDestroyed();
        }
    }
}