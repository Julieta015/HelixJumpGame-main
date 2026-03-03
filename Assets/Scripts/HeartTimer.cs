using UnityEngine;
using UnityEngine.UI;

public class HeartTimer : MonoBehaviour
{
    [Header("UI circle around heart")]
    public Image timerImage;      // Դեղին շրջանաձև Image-ը
    public float duration = 5f;   // Քանի վայրկյանում պետք է լրիվ դադարկվի

    float timeLeft;

    void OnEnable()
    {
        timeLeft = duration;

        if (timerImage != null)
        {
            // Համոզվում ենք, որ ճիշտ կարգավորմամբ է
            timerImage.type = Image.Type.Filled;
            timerImage.fillMethod = Image.FillMethod.Radial360;
            timerImage.fillOrigin = 2;      // Սկսի վերևից
            timerImage.fillClockwise = false;
            timerImage.fillAmount = 1f;     // Սկզբում լրիվ լցված
        }
    }

    void Update()
    {
        if (duration <= 0f || timerImage == null)
            return;

        timeLeft -= Time.deltaTime;
        float t = Mathf.Clamp01(timeLeft / duration);

        timerImage.fillAmount = t; // 1 → 0, կամաց-կամաց դադարկվում է
    }
}

