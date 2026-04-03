using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public GameObject islandPrefab;
    public float islandDistance = 0.2f; // Հեռավորությունը ձողից դեպի կողք
    public GameObject levelPrefab;
    public int totalLevels = 20;

    [Header("Starting Position")]
    public float startX = 0.4f;
    public float startY = -3.1f;
    public float startZ = 0f;

    [Header("Spacing Settings")]
    public float verticalStep = 2f;
    public float forwardStep = 1f;
    public float sideCurve = 2.5f;
    public float sineFrequency = 1.2f;

    void Start()
    {
        ClearGeneratedLevels();
        GenerateMap();
    }

    

    void CreateIsland(int levelIndex, Vector3 levelPos)
    {
        if (levelIndex % 4 == 0 && levelIndex != 0 || levelIndex == 7)
        {
            // 1. Հաշվում ենք կողմը (1 կամ -1)
            float sideMultiplier = ((levelIndex / 4) % 2 != 0) ? 1f : -1f;
            if (levelIndex == 7)
            {
                sideMultiplier = 4f;
            }
            // 2. Որոշում ենք ԹԱՐՄ World Position-ը
            // islandDistance-ը Inspector-ում դիր օրինակ 15
            float xOffset = 1 * sideMultiplier;
            Vector3 finalWorldPos = new Vector3(levelPos.x + xOffset, levelPos.y - 0f, levelPos.z);

            // 3. Ստեղծում ենք առանց Parent-ի (null)
            GameObject island = Instantiate(islandPrefab, finalWorldPos, Quaternion.identity);

            // 4. Ուժով ստիպում ենք ընդունել World Position-ը (կրկնակի ապահովագրություն)
            island.transform.position = finalWorldPos;

            island.transform.LookAt(levelPos);

            // 2. Հիմա ստանում ենք այդ պտույտի աստիճանները
            Vector3 currentEuler = island.transform.eulerAngles;

            // 3. Ուժով սահմանում ենք X-ը -90 (կամ 90), բայց պահում ենք Y-ը, որ նայի ձողին
            // Փորձիր -90, եթե չեղավ՝ 90, կամ 270
            island.transform.rotation = Quaternion.Euler(-90f, currentEuler.y, 0f);

            // 6. Scale-ը
            island.transform.localScale = new Vector3(10, 10, 10);

            // Ավելացնենք սա, որ տեսնես Console-ում թե ինչ թիվ է գրում իրականում
            Debug.Log($"Կղզի {levelIndex}-ը դրվեց այս դիրքում: {island.transform.position}");
        }
    }

    void GenerateMap()
    {
        for (int i = 0; i < totalLevels; i++)
        {
            float xPos = startX + Mathf.Sin((i % 2) * sineFrequency) * sideCurve;
            float yPos = startY + (i * verticalStep);
            float zPos = startZ + (i * forwardStep);

            Vector3 spawnPos = new Vector3(xPos, yPos, zPos);

            GameObject newLevel = Instantiate(levelPrefab, spawnPos, levelPrefab.transform.rotation, transform);

            // Տեքստի կարգավորում
            TextMeshPro textComponent = newLevel.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null)
            {
                textComponent.text = (i + 1).ToString();
            }
            else
            {
                TextMeshProUGUI textUI = newLevel.GetComponentInChildren<TextMeshProUGUI>();
                if (textUI != null) textUI.text = (i + 1).ToString();
            }

            // Button-ի կարգավորում
            LevelButton3D btn = newLevel.GetComponent<LevelButton3D>();
            if (btn == null) btn = newLevel.AddComponent<LevelButton3D>();
            btn.levelToLoad = i + 1;

            // Կանչում ենք կղզու ստեղծումը
            CreateIsland(i, spawnPos);
        }
    }

    void ClearGeneratedLevels()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
