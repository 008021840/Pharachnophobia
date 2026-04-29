using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    //audio
    public AudioSource source;
    public AudioClip deathsound;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 1.5f;

    [Header("Sanity")]
    [SerializeField] private int sanityLossAmount = 10;
    [SerializeField] private float sanityLossInterval = 3f;

    private SanityManager sanityManager;
    private GhostSpawnManager ghostSpawnManager;
    private Vector2 moveDirection;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        PickNewDirection();

        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(SanityDrainRoutine());
    }

    public void SetSanityManager(SanityManager manager)
    {
        sanityManager = manager;
    }

    public void SetSpawnManager(GhostSpawnManager manager)
    {
        ghostSpawnManager = manager;
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        KeepInsideScreen();
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);
            PickNewDirection();
        }
    }

    private IEnumerator SanityDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(sanityLossInterval);

            if (sanityManager != null)
            {
                sanityManager.RemoveSanity(sanityLossAmount);
            }
        }
    }

    private void PickNewDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;

        if (moveDirection == Vector2.zero)
        {
            moveDirection = Vector2.right;
        }
    }

    private void KeepInsideScreen()
    {
        if (mainCamera == null) return;

        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);

        if (screenPos.x <= 0f || screenPos.x >= 1f)
        {
            moveDirection.x *= -1f;
        }

        if (screenPos.y <= 0f || screenPos.y >= 1f)
        {
            moveDirection.y *= -1f;
        }

        moveDirection = moveDirection.normalized;
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (ghostSpawnManager != null)
        {
            source.PlayOneShot(deathsound);
            ghostSpawnManager.GhostDestroyed();
        }
    }
}