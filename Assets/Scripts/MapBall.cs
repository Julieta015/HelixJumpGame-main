using System.Collections;
using UnityEngine;

public class BallLogic : MonoBehaviour
{
    [Header("References")]
    public Transform levelsHolder;
    public GameObject splashPrefab;

    [Header("Hover Motion")]
    public float moveSpeed = 8f;
    public float hoverHeight = 0.9f;
    public float bounceHeight = 0.35f;
    public float bounceSpeed = 3.5f;
    public Vector3 worldOffset = Vector3.zero;

    [Header("Splash")]
    public float splashFadeDelay = 0.8f;
    public float splashFadeTime = 0.3f;
    public float splashSurfaceOffset = 0.001f;
    public float splashScaleMultiplier = 8f;

    private Rigidbody rb;
    private MeshRenderer ballRenderer;
    private Vector3[] levelPositions = new Vector3[0];
    private Transform[] levelSplashParents = new Transform[0];
    private Vector3 smoothedAnchorPosition;
    private int lastTargetLevel = -1;
    private float bounceTimer;
    private bool hasAnchor;
    private int cachedChildCount = -1;
    private float previousBounceSine;
    private bool hasBounceSample;

    void Awake()
    {
        ballRenderer = GetComponent<MeshRenderer>();
        ConfigureRigidbody();
    }

    void Start()
    {
        ConfigureRigidbody();
        RefreshLevelPositions();
        SnapToCurrentLevel();
    }

    void LateUpdate()
    {
        ConfigureRigidbody();
        RefreshLevelPositions();

        if (!TryGetCurrentAnchor(out Vector3 anchorPosition, out int targetIndex))
            return;

        if (!hasAnchor)
        {
            smoothedAnchorPosition = anchorPosition;
            hasAnchor = true;
        }

        float followT = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
        smoothedAnchorPosition = Vector3.Lerp(smoothedAnchorPosition, anchorPosition, followT);

        bounceTimer += Time.deltaTime * bounceSpeed;
        float bounceSine = Mathf.Sin(bounceTimer);
        float bounceOffset = Mathf.Abs(bounceSine) * bounceHeight;

        Vector3 targetPosition = smoothedAnchorPosition + worldOffset + Vector3.up * (hoverHeight + bounceOffset);
        MoveBall(targetPosition);
        TrySpawnSplash(smoothedAnchorPosition, targetIndex, bounceSine);

        if (targetIndex != lastTargetLevel)
        {
            lastTargetLevel = targetIndex;
            Debug.Log("Գնդակը տեղափոխվում է լեվել: " + (targetIndex + 1));
        }
    }

    void SnapToCurrentLevel()
    {
        RefreshLevelPositions();

        if (!TryGetCurrentAnchor(out Vector3 anchorPosition, out int targetIndex))
            return;

        smoothedAnchorPosition = anchorPosition;
        hasAnchor = true;
        lastTargetLevel = targetIndex;

        Vector3 startPosition = smoothedAnchorPosition + worldOffset + Vector3.up * hoverHeight;

        if (rb != null)
        {
            rb.position = startPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = startPosition;
    }

    bool TryGetCurrentAnchor(out Vector3 anchorPosition, out int targetIndex)
    {
        anchorPosition = Vector3.zero;
        targetIndex = 0;

        if (levelPositions == null || levelPositions.Length == 0)
            return false;

        targetIndex = Mathf.Clamp(Level.level - 1, 0, levelPositions.Length - 1);
        anchorPosition = levelPositions[targetIndex];
        return true;
    }

    void RefreshLevelPositions()
    {
        if (levelsHolder == null)
            return;

        if (levelsHolder.childCount == cachedChildCount && levelPositions.Length == cachedChildCount)
            return;

        cachedChildCount = levelsHolder.childCount;
        levelPositions = new Vector3[cachedChildCount];
        levelSplashParents = new Transform[cachedChildCount];

        for (int i = 0; i < cachedChildCount; i++)
        {
            Transform levelRoot = levelsHolder.GetChild(i);
            levelPositions[i] = CalculateLevelAnchor(levelRoot);
            levelSplashParents[i] = levelRoot;
        }
    }

    Vector3 CalculateLevelAnchor(Transform levelRoot)
    {
        Transform modelRoot = levelRoot.childCount > 0 ? levelRoot.GetChild(0) : levelRoot;
        Renderer[] renderers = modelRoot.GetComponentsInChildren<Renderer>(true);

        Bounds combinedBounds = default;
        bool hasBounds = false;

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null || renderer.gameObject.layer == 5)
                continue;

            if (!hasBounds)
            {
                combinedBounds = renderer.bounds;
                hasBounds = true;
            }
            else
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
        }

        if (!hasBounds)
            return modelRoot.position;

        return new Vector3(combinedBounds.center.x, combinedBounds.max.y, combinedBounds.center.z);
    }

    void MoveBall(Vector3 targetPosition)
    {
        if (rb != null)
        {
            rb.position = targetPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = targetPosition;
    }

    void ConfigureRigidbody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (rb == null)
            return;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void TrySpawnSplash(Vector3 anchorPosition, int targetIndex, float bounceSine)
    {
        if (splashPrefab == null)
            return;

        if (hasBounceSample && previousBounceSine < 0f && bounceSine >= 0f)
        {
            Vector3 spawnPosition = anchorPosition + worldOffset + Vector3.up * splashSurfaceOffset;
            float randomY = Random.Range(0f, 360f);
            Quaternion splashRotation = Quaternion.Euler(90f, randomY, 0f);

            GameObject splash = Instantiate(splashPrefab, spawnPosition, splashRotation);
            splash.transform.localScale *= splashScaleMultiplier;

            if (targetIndex >= 0 && targetIndex < levelSplashParents.Length && levelSplashParents[targetIndex] != null)
            {
                splash.transform.SetParent(levelSplashParents[targetIndex]);
            }

            SpriteRenderer splashRenderer = splash.GetComponent<SpriteRenderer>();
            if (splashRenderer != null && ballRenderer != null)
            {
                splashRenderer.color = ballRenderer.material.color;
            }

            StartCoroutine(FadeAndDestroySplash(splash));
        }

        previousBounceSine = bounceSine;
        hasBounceSample = true;
    }

    IEnumerator FadeAndDestroySplash(GameObject splash)
    {
        yield return new WaitForSeconds(splashFadeDelay);

        if (splash == null)
            yield break;

        SpriteRenderer splashRenderer = splash.GetComponent<SpriteRenderer>();
        if (splashRenderer != null)
        {
            float elapsed = 0f;
            Color startColor = splashRenderer.color;

            while (elapsed < splashFadeTime)
            {
                if (splash == null)
                    yield break;

                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / splashFadeTime);
                splashRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        if (splash != null)
        {
            Destroy(splash);
        }
    }
}
