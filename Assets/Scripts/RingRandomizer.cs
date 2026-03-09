

/*using System.Collections.Generic;
using UnityEngine;

public class RingRandomizer : MonoBehaviour
{
    public GameObject dangerPrefab;          
    public int maxDangerSegments = 2; 
    [Range(0f, 1f)]
    public float gapChance = 0.25f;           

    private void Start()
    {
        if (transform.parent != null)
        {
            int myIndex = transform.GetSiblingIndex();
            int lastIndex = transform.parent.childCount - 1;
            if (myIndex == lastIndex) return;
        }
        RandomizeRing();
    }

    void RandomizeRing()
    {
        var segments = new List<Transform>();

        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            if (t == transform || t.name.Contains("Cylinder")) continue;
            segments.Add(t);
        }

        if (segments.Count < 3) return;

        // ✅ GAP-եր (բացվածքներ)
        int guaranteedGapIndex = Random.Range(1, segments.Count - 1);

        // Քո լոգիկան՝ առաջին guaranteedGapIndex հատը անջատվում են
        for (int i = 0; i < guaranteedGapIndex; i++)
        {
            segments[i].gameObject.SetActive(false);
        }

        // ✅ Ակտիվ սեգմենտների հավաքում
        List<Transform> activeSegments = new List<Transform>();
        foreach (Transform seg in segments)
        {
            if (seg.gameObject.activeSelf)
                activeSegments.Add(seg);
        }

        // Սկզբում բոլոր ակտիվները համարում ենք "անվտանգ"
        List<Transform> safeSegments = new List<Transform>(activeSegments);

        // ✅ Danger candidates = safeSegments, բայց հանում ենք gap-ի տակ գտնվողները
        // Քո դեպքում gap zone-ը = indices [0..guaranteedGapIndex-1]
        List<Transform> dangerCandidates = new List<Transform>();
        for (int si = 0; si < safeSegments.Count; si++)
        {
            Transform s = safeSegments[si];
            int segIndex = segments.IndexOf(s); // s-ը segments-ից է եկել

            // Եթե սեգմենտը gap zone-ի տակ է -> չի կարելի danger
            if (segIndex >= 0 && segIndex < guaranteedGapIndex)
                continue;

            dangerCandidates.Add(s);
        }

        // 2️⃣ Danger segment-ներ (ՊԱՏԱՀԱԿԱՆԸՆՏՐՈՒԹՅԱՄԲ)
        bool isFirstRing = (transform.parent != null && transform.GetSiblingIndex() == 0);

        if (!isFirstRing && Level.level % 2 == 0 && dangerPrefab != null && Level.level > 1)
        {
            int dangerCount = Random.Range(1, maxDangerSegments + 1);

            // ✅ dangerCandidates-ից ենք ընտրում, ոչ թե safeSegments-ից
            for (int i = 0; i < dangerCount && dangerCandidates.Count > 0 && safeSegments.Count > 1; i++)
            {
                int index = Random.Range(0, dangerCandidates.Count);
                Transform dangerSeg = dangerCandidates[index];

                // 1) դարձնում ենք սև
                MeshRenderer mr = dangerSeg.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material.color = Color.black;
                }

                // 2) ավելացնում ենք dangerPrefab-ը
                MakeSegmentDangerous(dangerSeg);

                // 3) հանում ենք ցուցակներից, որ կրկին չընտրվի ու safe մնա
                dangerCandidates.RemoveAt(index);
                safeSegments.Remove(dangerSeg);
            }
        }

        // 3️⃣ Գնդակի դիրքի ապահովում
        if (safeSegments.Count > 0)
        {
            Transform landingPad = safeSegments[Random.Range(0, safeSegments.Count)];
            float targetAngle = landingPad.localEulerAngles.y;
            transform.eulerAngles = new Vector3(0, targetAngle, 0);
        }
    }

    /// <summary>
    /// Սև material ունեցող սեգմենտի վրա ավելացնում է dangerPrefab-ը։
    /// </summary>
    void MakeSegmentDangerous(Transform segment)
    {
        segment.tag = "Danger";

        if (dangerPrefab == null) return;

        foreach (Transform child in segment)
        {
            if (child.name.Contains(dangerPrefab.name))
            {
                return;
            }
        }

        GameObject spikes = Instantiate(dangerPrefab, segment);
        spikes.transform.localPosition = Vector3.zero;
        spikes.transform.localRotation = Quaternion.identity;
        spikes.transform.localScale = Vector3.one;
    }
}*/

using System.Collections.Generic;
using UnityEngine;

public class RingRandomizer : MonoBehaviour
{
    [Header("Danger Settings")]
    public int maxDangerSegments = 3;

    [Header("Level Design Rules")]
    public bool firstRingAlwaysSafe = true;
    public bool oddLevelsHaveNoDanger = true;

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
    }

    void RandomizeRing()
    {
        List<Transform> segments = GetAllSegments();
        if (segments.Count < 3) return;

        bool isFirstRing = transform.parent != null && transform.GetSiblingIndex() == 0;

        RingDifficulty difficulty = GetDifficulty(Level.level, isFirstRing);
        RingPattern pattern = GeneratePattern(difficulty, segments.Count);

        ApplyGaps(segments, pattern.gapIndices);

        List<Transform> activeSegments = GetActiveSegments(segments);
        if (activeSegments.Count <= 1) return;

        List<Transform> safeSegments = new List<Transform>(activeSegments);

        ApplyDangerSegments(safeSegments, pattern.dangerCount, isFirstRing);

        if (safeSegments.Count > 0)
        {
            Transform landingPad = safeSegments[Random.Range(0, safeSegments.Count)];
            float targetAngle = landingPad.localEulerAngles.y;
            transform.eulerAngles = new Vector3(0f, targetAngle, 0f);
        }
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

    void ApplyGaps(List<Transform> segments, List<int> gapIndices)
    {
        foreach (int index in gapIndices)
        {
            if (index >= 0 && index < segments.Count)
            {
                segments[index].gameObject.SetActive(false);
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

            MeshRenderer mr = dangerSeg.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.material.color = Color.black;
            }

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
                gapCount = Random.Range(2, 4);      // 2 կամ 3 gap
                pattern.dangerCount = 1;            // հեշտ ռինգում թող ավելի հանգիստ լինի
                break;

            case RingDifficulty.Medium:
                gapCount = Random.Range(2, 4);      // 2 կամ 3 gap
                pattern.dangerCount = Random.Range(1, 3); // 1 կամ 2 danger
                break;

            case RingDifficulty.Hard:
                gapCount = Random.Range(1, 3);      // 1 կամ 2 gap
                pattern.dangerCount = Random.Range(2, maxDangerSegments + 1);
                break;
        }

        // Որպեսզի ամբողջ ռինգը չփակվի
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
}