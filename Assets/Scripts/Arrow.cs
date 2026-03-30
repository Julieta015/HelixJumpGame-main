using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    public static bool IsAutoMoving { get; private set; }

    public Transform mapManager; 
    public float speed = 5f;    
    public Vector3 offset = new Vector3(0, 5, -10);

    public void GoToLevel()
    {
        Transform targetLevel = GetTargetLevel();
        if (targetLevel == null)
            return;

        Vector3 targetAnchor = CalculateLevelAnchor(targetLevel);
        Vector3 targetPosition = new Vector3(
            transform.position.x,
            targetAnchor.y + offset.y,
            targetAnchor.z + offset.z);

        IsAutoMoving = false;
        StopAllCoroutines();
        StartCoroutine(SmoothMove(targetPosition));
    }

    void OnDisable()
    {
        IsAutoMoving = false;
    }

    IEnumerator SmoothMove(Vector3 target)
    {
        IsAutoMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            // Lerp-ը ապահովում է սահուն անցումը
            Vector3 nextPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            transform.position = new Vector3(transform.position.x, nextPosition.y, nextPosition.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, target.y, target.z);
        IsAutoMoving = false;
    }

    Transform GetTargetLevel()
    {
        if (mapManager == null || mapManager.childCount == 0)
            return null;

        int unlockedLevel = Mathf.Max(1, BallBounce.completLevel);

        for (int i = 0; i < mapManager.childCount; i++)
        {
            Transform child = mapManager.GetChild(i);
            LevelButton3D levelButton = child.GetComponent<LevelButton3D>();
            if (levelButton != null && levelButton.levelToLoad == unlockedLevel)
            {
                return child;
            }
        }

        int fallbackIndex = Mathf.Clamp(unlockedLevel - 1, 0, mapManager.childCount - 1);
        return mapManager.GetChild(fallbackIndex);
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
}
