using UnityEngine;

public class Spider : MonoBehaviour
{
    public enum SpiderMoveType
    {
        CrawlToTarget,
        DropThenCrawl,
        FastDropAttack
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dropSpeed = 2f;
    [SerializeField] private float fastDropSpeed = 8f;
    [SerializeField] private float pauseBeforeDrop = 1f;

    [Header("Web Line")]
    [SerializeField] private LineRenderer webLine;
    [SerializeField] private float webTopOffset = 2f;

    [Header("Sanity")]
    [SerializeField] private int sanityGainOnClick = 1;
    [SerializeField] private int sanityLossOnReachTarget = 5;
    [SerializeField] private float reachDistance = 0.05f;

    private Transform target;
    private SpiderSpawnManager spawnManager;
    private SanityManager sanityManager;

    private SpiderMoveType moveType;
    private bool initialized = false;
    private bool finishedDropping = false;
    private bool waitingToDrop = false;
    private float dropTimer = 0f;
    private float dropStopY;
    private Vector3 webStartPosition;

    public void Initialize(
        Transform newTarget,
        SpiderSpawnManager newSpawnManager,
        SanityManager newSanityManager,
        SpiderMoveType newMoveType,
        float newDropStopY
    )
    {
        target = newTarget;
        spawnManager = newSpawnManager;
        sanityManager = newSanityManager;
        moveType = newMoveType;
        dropStopY = newDropStopY;
        initialized = true;

        if (webLine == null)
        {
            webLine = GetComponent<LineRenderer>();
        }

        if (moveType == SpiderMoveType.DropThenCrawl || moveType == SpiderMoveType.FastDropAttack)
        {
            finishedDropping = false;
            waitingToDrop = true;
            dropTimer = pauseBeforeDrop;
            webStartPosition = transform.position + Vector3.up * webTopOffset;

            if (webLine != null)
            {
                webLine.enabled = true;
                webLine.positionCount = 2;
            }
        }
        else
        {
            finishedDropping = true;

            if (webLine != null)
            {
                webLine.enabled = false;
            }
        }

        Debug.Log(gameObject.name + " initialized as: " + moveType);
    }

    private void Update()
    {
        if (!initialized || target == null) return;

        UpdateWebLine();

        if ((moveType == SpiderMoveType.DropThenCrawl || moveType == SpiderMoveType.FastDropAttack) && !finishedDropping)
        {
            if (waitingToDrop)
            {
                dropTimer -= Time.deltaTime;

                if (dropTimer <= 0f)
                {
                    waitingToDrop = false;
                }

                return;
            }

            DropDown();
            return;
        }

        CrawlToTarget();
    }

    private void DropDown()
    {
        float speed = moveType == SpiderMoveType.FastDropAttack ? fastDropSpeed : dropSpeed;

        Vector2 dropTarget = new Vector2(transform.position.x, dropStopY);

        transform.position = Vector2.MoveTowards(
            transform.position,
            dropTarget,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, dropTarget) <= 0.05f)
        {
            finishedDropping = true;

            if (webLine != null)
            {
                webLine.enabled = false;
            }
        }
    }

    private void CrawlToTarget()
    {
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

    private void UpdateWebLine()
    {
        if (webLine == null || !webLine.enabled) return;

        webLine.SetPosition(0, webStartPosition);
        webLine.SetPosition(1, transform.position);
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