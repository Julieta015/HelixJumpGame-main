using UnityEngine;
using UnityEngine.UI;

public class HeartCircleTimer : MonoBehaviour
{
    [Header("Visual settings")]
    public Sprite circleSprite;          // Շրջանաձև sprite (filled circle)
    public Color circleColor = Color.yellow;
    public float radius = 1f;            // Շրջանագծի չափը world-space-ում

    [Header("Timer settings")]
    public float duration = 5f;          // Քանի վայրկյանում պետք է լիքը դեղինը դառնա դատարկ

    Image timerImage;
    float timeLeft;

    void Start()
    {
        // Ստեղծում ենք world-space Canvas հենց heart-ի վրա
        GameObject canvasGO = new GameObject("HeartCircleCanvas");
        canvasGO.transform.SetParent(transform, false);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        canvasGO.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvasGO.GetComponent<RectTransform>();
        canvasRect.localPosition = Vector3.zero;
        canvasRect.localRotation = Quaternion.identity;
        canvasRect.sizeDelta = new Vector2(radius, radius);
        canvasRect.localScale = Vector3.one;

        // Շրջանաձև Image
        GameObject imgGO = new GameObject("HeartTimerCircle");
        imgGO.transform.SetParent(canvasGO.transform, false);

        timerImage = imgGO.AddComponent<Image>();
        timerImage.sprite = circleSprite;
        timerImage.color = circleColor;
        timerImage.type = Image.Type.Filled;
        timerImage.fillMethod = Image.FillMethod.Radial360;
        timerImage.fillOrigin = 2;      // Վերևից
        timerImage.fillClockwise = false;
        timerImage.fillAmount = 1f;     // Սկզբում լրիվ լցված

        RectTransform imgRect = imgGO.GetComponent<RectTransform>();
        imgRect.anchorMin = new Vector2(0.5f, 0.5f);
        imgRect.anchorMax = new Vector2(0.5f, 0.5f);
        imgRect.pivot = new Vector2(0.5f, 0.5f);
        imgRect.anchoredPosition = Vector2.zero;
        imgRect.sizeDelta = new Vector2(radius, radius);

        timeLeft = duration;
    }

    void Update()
    {
        if (timerImage == null || duration <= 0f)
            return;

        timeLeft -= Time.deltaTime;
        float t = Mathf.Clamp01(timeLeft / duration);
        timerImage.fillAmount = t; // 1 → 0, դեղինը դանդաղ «հալվում» է
    }
}

