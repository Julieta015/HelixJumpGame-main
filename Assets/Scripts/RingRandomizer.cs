

//using System;
using System.Collections.Generic;
using UnityEngine;

public class RingRandomizer : MonoBehaviour
{
    [Header("Danger Settings")]
    public int maxDangerSegments = 3;

    [Header("Level Design Rules")]
    public bool firstRingAlwaysSafe = true;
    public bool oddLevelsHaveNoDanger = true;

    public List<Material> materials;
    public List<Material> dangerMaterials;
    private List<Transform> dangerSegments = new List<Transform>();
    private void Start()
    {
        if (transform.parent != null)
        {
            int myIndex = transform.GetSiblingIndex();
            int lastIndex = transform.parent.childCount - 1;

            // Վերջին ռինգը չենք randomize անում
            if (myIndex == lastIndex)
                return;
        }

        RandomizeRing();
        ChangeMaterial();
    }

    void RandomizeRing()
    {

        List<Transform> segments = GetAllSegments();
        if (segments.Count < 3) return;

        bool isFirstRing = transform.parent != null && transform.GetSiblingIndex() == 0;

        RingDifficulty difficulty = GetDifficulty(Level.level, isFirstRing);
        RingPattern pattern = GeneratePattern(difficulty, segments.Count);

        ApplyGaps(segments, pattern.gapIndices, isFirstRing);

        List<Transform> activeSegments = GetActiveSegments(segments);
        if (activeSegments.Count <= 1) return;

        List<Transform> safeSegments = new List<Transform>(activeSegments);

        ApplyDangerSegments(safeSegments, pattern.dangerCount, isFirstRing);

        //if (safeSegments.Count > 0)
        //{
        //    Transform landingPad = safeSegments[Random.Range(0, safeSegments.Count)];
        //    float targetAngle = landingPad.localEulerAngles.y;
        //    transform.eulerAngles = new Vector3(0f, targetAngle, 0f);
        //}
    }

    List<Transform> GetAllSegments()
    {
        List<Transform> segments = new List<Transform>();

        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            if (t == transform) continue;
            if (t.name.Contains("Cylinder")) continue;


            segments.Add(t);
        }

        return segments;
    }

    List<Transform> GetActiveSegments(List<Transform> segments)
    {
        List<Transform> activeSegments = new List<Transform>();

        foreach (Transform seg in segments)
        {
            if (seg.gameObject.activeSelf)
                activeSegments.Add(seg);
        }

        return activeSegments;
    }

    void ApplyGaps(List<Transform> segments, List<int> gapIndices, bool isFirstRing)
    {
        if (isFirstRing)
        {
            segments[4].gameObject.SetActive(false);
            segments[3].gameObject.SetActive(false);
        }
        else
        {
            foreach (int index in gapIndices)
            {
                if (index >= 0 && index < segments.Count)
                {
                    segments[index].gameObject.SetActive(false);
                }
            }
        }

    }

    void ApplyDangerSegments(List<Transform> safeSegments, int dangerCount, bool isFirstRing)
    {
        if (isFirstRing && firstRingAlwaysSafe) return;
        if (Level.level <= 1) return;

        // Կենտ լեվլերում danger չկա
        if (oddLevelsHaveNoDanger && Level.level % 2 != 0 || Level.level == 1) return;

        List<Transform> dangerCandidates = new List<Transform>(safeSegments);

        // Առնվազն 1 safe հատված պիտի մնա
        int allowedDangerCount = Mathf.Min(dangerCount, dangerCandidates.Count - 1);
        if (allowedDangerCount <= 0) return;

        for (int i = 0; i < allowedDangerCount; i++)
        {
            int index = Random.Range(0, dangerCandidates.Count);
            Transform dangerSeg = dangerCandidates[index];
            dangerSegments.Add(dangerSeg);



            MakeSegmentDangerous(dangerSeg);

            dangerCandidates.RemoveAt(index);
            safeSegments.Remove(dangerSeg);
        }
    }

    RingDifficulty GetDifficulty(int level, bool isFirstRing)
    {
        if (isFirstRing && firstRingAlwaysSafe)
            return RingDifficulty.Easy;

        if (level <= 3)
            return RingDifficulty.Easy;
        else if (level <= 7)
            return RingDifficulty.Medium;
        else
            return RingDifficulty.Hard;
    }

    RingPattern GeneratePattern(RingDifficulty difficulty, int segmentCount)
    {
        RingPattern pattern = new RingPattern();
        pattern.gapIndices = new List<int>();

        int gapCount = 0;

        switch (difficulty)
        {
            case RingDifficulty.Easy:
                gapCount = Random.Range(2, 4);
                pattern.dangerCount = Random.Range(0, 2);
                break;

            case RingDifficulty.Medium:
                gapCount = Random.Range(2, 4);
                pattern.dangerCount = Random.Range(1, 3);
                break;

            case RingDifficulty.Hard:
                gapCount = Random.Range(1, 3);
                pattern.dangerCount = Random.Range(2, maxDangerSegments + 1);
                break;
        }


        gapCount = Mathf.Min(gapCount, segmentCount - 1);

        while (pattern.gapIndices.Count < gapCount)
        {
            int randomIndex = Random.Range(0, segmentCount);

            if (!pattern.gapIndices.Contains(randomIndex))
            {
                pattern.gapIndices.Add(randomIndex);
            }
        }

        return pattern;
    }

    void MakeSegmentDangerous(Transform segment)
    {
        MeshRenderer mr = segment.GetComponent<MeshRenderer>();
        Material dangerMaterial = GetMaterialByIndex(dangerMaterials, (Level.level / 2) - 1);
        if (mr != null && dangerMaterial != null)
        {
            mr.material = dangerMaterial;
        }
        segment.tag = "Danger";
    }

    enum RingDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    struct RingPattern
    {
        public List<int> gapIndices;
        public int dangerCount;
    }

    void ChangeMaterial()
    {

        List<Transform> segments = GetAllSegments();

        foreach (Transform seg in segments)
        {
            MeshRenderer mr = seg.GetComponent<MeshRenderer>();

            if (dangerSegments.Contains(seg))
                continue;

            Material levelMaterial = GetMaterialByIndex(materials, Level.level - 1);
            if (mr != null && levelMaterial != null)
            {
                mr.material = levelMaterial;
            }
        }

    }

    Material GetMaterialByIndex(List<Material> source, int rawIndex)
    {
        if (source == null || source.Count == 0)
            return null;

        int safeIndex = ((rawIndex % source.Count) + source.Count) % source.Count;
        return source[safeIndex];
    }
}
