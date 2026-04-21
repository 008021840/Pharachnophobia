using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int sanityGainOnClick = 1;
    [SerializeField] private int sanityLossOnReachTarget = 5;
    [SerializeField] private float reachDistance = 0.05f;

    private Transform target;
    private SpiderSpawnManager spawnManager;
    private SanityManager sanityManager;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetSpawnManager(SpiderSpawnManager manager)
    {
        spawnManager = manager;
    }

    public void SetSanityManager(SanityManager manager)
    {
        sanityManager = manager;
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) <= reachDistance)
        {
            if (sanityManager != null)
            {
                sanityManager.RemoveSanity(sanityLossOnReachTarget);
            }

            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (sanityManager != null)
        {
            sanityManager.AddSanity(sanityGainOnClick);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (spawnManager != null)
        {
            spawnManager.SpiderDestroyed();
        }
    }
}