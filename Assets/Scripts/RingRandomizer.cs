
/*using System.Collections.Generic;
using UnityEngine;

public class RingRandomizer : MonoBehaviour
{
    
    public GameObject dangerPrefab;            // Օբյեկտ եռանկյուններով (HelixJump_Obstacle_Platform)
    public int maxDangerSegments = 2;          // Քանի հատ segment կարող է լինել վտանգավոր (այժմ INFORMATIVE է, կոդում չի օգտագործվում)
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

        //GAP-եր (բացվածքներ)
        int guaranteedGapIndex = Random.Range(1, segments.Count - 1);

        for (int i = 0; i < guaranteedGapIndex; i++)
        {
            segments[i].gameObject.SetActive(false);
        }

        // Ակտիվ սեգմենտների հավաքում
        List<Transform> activeSegments = new List<Transform>();
        foreach (Transform seg in segments)
        {
            if (seg.gameObject.activeSelf)
                activeSegments.Add(seg);
        }

        // Սկզբում բոլոր ակտիվները համարում ենք "անվտանգ"
        List<Transform> safeSegments = new List<Transform>(activeSegments);

        // 2️⃣ Danger segment-ներ (ՊԱՏԱՀԱԿԱՆԸՆՏՐՈՒԹՅԱՄԲ)
        // Այստեղ հենց կոդով ենք որոշում, թե որ սեգմենտները լինեն սև (վտանգավոր),
        // և ՀԵՆՑ ԴՐԱՆՑ ՎՐԱ ենք ավելացնում dangerPrefab-ը։
        //
        // ԲԱՅՑ՝ առաջին օղակի (SiblingIndex == 0) վրա
        // ընդհանրապես վտանգավոր սեգմենտ չենք դնում, որ գնդակի տակ
        // միշտ լինի սովորական (անվտանգ) պլատֆորմ։
        bool isFirstRing = (transform.parent != null && transform.GetSiblingIndex() == 0);

        if (!isFirstRing && Level.level % 2 == 0 && dangerPrefab != null && Level.level > 1)
        {
            int dangerCount = Random.Range(1, maxDangerSegments + 1);

            for (int i = 0; i < dangerCount && safeSegments.Count > 1; i++) // միշտ թողնում ենք գոնե 1 safe
            {
                int index = Random.Range(0, safeSegments.Count);
                Transform dangerSeg = safeSegments[index];

                // 1) դարձնում ենք սև
                MeshRenderer mr = dangerSeg.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material.color = Color.black;
                }

                // 2) ավելացնում ենք քո վտանգավոր prefab-ը ՀԵՆՑ ԷՍ սեգմենտի վրա
                MakeSegmentDangerous(dangerSeg);

                // 3) հանում ենք safe ցուցակից, որ գնդակը հետո սրա վրա չուղղվի
                safeSegments.RemoveAt(index);
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
    /// Հենց ՍԵՎ material ունեցող սեգմենտի վրա ավելացնում է dangerPrefab‑ը։
    /// </summary>
    void MakeSegmentDangerous(Transform segment)
    {
        // Tag-ով ասում ենք, որ սա վտանգավոր սեգմենտ է (BallBounce-ը օգտագործում է "Danger")
        segment.tag = "Danger";

        if (dangerPrefab == null) return;

        // Եթե արդեն ունի մեր prefab‑ից child, նորից չենք ավելացնում
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
        spikes.transform.localScale = Vector3.one; // prefab‑ի մեջ արդեն ճիշտ կառուցվածք/չափ ես տվել
    }
}*/

using System.Collections.Generic;
using UnityEngine;

public class RingRandomizer : MonoBehaviour
{
    public GameObject dangerPrefab;            // Օբյեկտ եռանկյուններով
    public int maxDangerSegments = 2;          // max danger
    [Range(0f, 1f)]
    public float gapChance = 0.25f;           // (քո կոդում հիմա չի օգտագործվում, թողել եմ)

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
}