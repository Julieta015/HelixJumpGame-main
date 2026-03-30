using UnityEngine;

public class ArrowVisiblity : MonoBehaviour
{
    public GameObject arrowButton; // Քաշիր քո UI սլաքը այստեղ
    public GameObject playButton;
    public Transform levelsHolder; // Լեվելների պապկան
    public float hideDistance = 2f; // Ինչքան մոտ լինի, որ սլաքը կորի
    public float targetYOffset = 5f;

    bool hasManualLevelSelection;

    void Awake()
    {
        if (playButton == null)
        {
            GameObject foundPlayButton = GameObject.Find("play");
            if (foundPlayButton != null)
            {
                playButton = foundPlayButton;
            }
        }
    }

    void OnEnable()
    {
        LevelButton3D.LevelSelected += HandleLevelSelected;
    }

    void OnDisable()
    {
        LevelButton3D.LevelSelected -= HandleLevelSelected;
        hasManualLevelSelection = false;
    }

    void Update()
    {
        if (arrowButton == null || levelsHolder == null)
            return;

        if (hasManualLevelSelection)
        {
            arrowButton.SetActive(false);

            if (playButton != null)
            {
                playButton.SetActive(true);
            }

            return;
        }

        Transform targetLevel = GetTargetLevel();
        if (targetLevel == null)
            return;

        Vector3 targetAnchor = CalculateLevelAnchor(targetLevel);
        float deltaY = (targetAnchor.y + targetYOffset) - transform.position.y;
        float distance = Mathf.Abs(deltaY);

        if (distance < hideDistance)
        {
            arrowButton.SetActive(false);

            if (playButton != null)
            {
                playButton.SetActive(true);
            }

            return;
        }

        arrowButton.SetActive(true);
        arrowButton.transform.localRotation = Quaternion.Euler(0f, 0f, deltaY >= 0f ? 180f : -180f);

        if (playButton != null)
        {
            playButton.SetActive(false);
        }
    }

    Transform GetTargetLevel()
    {
        if (levelsHolder == null || levelsHolder.childCount == 0)
            return null;

        int unlockedLevel = Mathf.Max(1, BallBounce.completLevel);

        for (int i = 0; i < levelsHolder.childCount; i++)
        {
            Transform child = levelsHolder.GetChild(i);
            LevelButton3D levelButton = child.GetComponent<LevelButton3D>();
            if (levelButton != null && levelButton.levelToLoad == unlockedLevel)
            {
                return child;
            }
        }

        int fallbackIndex = Mathf.Clamp(unlockedLevel - 1, 0, levelsHolder.childCount - 1);
        return levelsHolder.GetChild(fallbackIndex);
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

    void HandleLevelSelected(int selectedLevel)
    {
        hasManualLevelSelection = true;

        if (arrowButton != null)
        {
            arrowButton.SetActive(false);
        }

        if (playButton != null)
        {
            playButton.SetActive(true);
        }
    }
}
