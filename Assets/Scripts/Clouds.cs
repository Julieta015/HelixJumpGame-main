using System.Collections;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    RectTransform rt;
    public int speed = 600;
    void Start()
    {
        rt = GetComponent<RectTransform>();

        Vector2 pos = rt.anchoredPosition;
        pos.x = 1662f;
        rt.anchoredPosition = pos;

        
    }

    public IEnumerator CloudMove()
    {
        while (Mathf.Abs(rt.anchoredPosition.x - (-264f)) > 1f)
        {
            Vector2 pos = rt.anchoredPosition;
            pos.x = Mathf.MoveTowards(pos.x, -264f, speed * Time.deltaTime);
            rt.anchoredPosition = pos;

            yield return null;
        }

        Vector2 finalPos = rt.anchoredPosition;
        finalPos.x = -264f;
        rt.anchoredPosition = finalPos;
    }
}
