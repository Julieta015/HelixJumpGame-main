using UnityEngine;

public class LevelsColor : MonoBehaviour
{
    public Transform mapManager;

    [Header("Level Colors")]
    public Color completedLevelColor = new Color32(255, 225, 0, 255);
    public Color currentLevelColor = Color.yellow;

    private int lastProcessedCompletedLevel = -1;
    private int lastProcessedChildCount = -1;

    void Start()
    {
        RefreshLevelStates(force: true);
    }

    void LateUpdate()
    {
        RefreshLevelStates(force: false);

    }

    void RefreshLevelStates(bool force)
    {
        if (mapManager == null)
            return;

        int completedLevel = BallBounce.completLevel;
        int childCount = mapManager.childCount;
        Debug.Log("completedLevel");
        Debug.Log(completedLevel);
        if (!force &&
            completedLevel == lastProcessedCompletedLevel &&
            childCount == lastProcessedChildCount)
        {
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform levelRow = mapManager.GetChild(i);
            int levelNumber = i + 1;

            if (TryGetLevelRenderer(levelRow, out MeshRenderer renderer))
            {
                if (levelNumber < completedLevel)
                {
                    renderer.material.color = completedLevelColor;
                }
                else if (levelNumber == completedLevel)
                {
                    renderer.material.color = currentLevelColor;
                }
            }

            bool shouldBeLocked = levelNumber > completedLevel;

            SetLocksActive(levelRow, shouldBeLocked);

        }

        lastProcessedCompletedLevel = completedLevel;
        lastProcessedChildCount = childCount;
    }

    bool TryGetLevelRenderer(Transform levelRow, out MeshRenderer renderer)
    {
        renderer = null;

        if (levelRow == null || levelRow.childCount == 0)
            return false;

        Transform firstChild = levelRow.GetChild(0);
        if (firstChild.childCount <= 1)
            return false;

        Transform secondChild = firstChild.GetChild(1);
        if (secondChild.childCount <= 1)
            return false;

        Transform targetChild = secondChild.GetChild(1);
        renderer = targetChild.GetComponent<MeshRenderer>();
        return renderer != null;

    }

    void SetLocksActive(Transform levelTransform, bool isLocked)
    {
        if (levelTransform == null)
            return;

        Lock[] allLocks = levelTransform.GetComponentsInChildren<Lock>(true);
        foreach (Lock levelLock in allLocks)
        {
            if (levelLock != null && levelLock.locks != null)
            {
                levelLock.locks.SetActive(isLocked);
            }
        }

        Lock2[] allDoubleLocks = levelTransform.GetComponentsInChildren<Lock2>(true);
        foreach (Lock2 levelLock in allDoubleLocks)
        {
            if (levelLock == null)
                continue;

            if (levelLock.lock1 != null)
            {
                levelLock.lock1.SetActive(isLocked);
            }

            if (levelLock.lock2 != null)
            {
                levelLock.lock2.SetActive(isLocked);
            }
        }
    }
}
