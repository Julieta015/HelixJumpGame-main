using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSetting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Sprite> sprites;
    bool sp = true;
    
    public void ChangeSprite()
    {
        
        Image renderer = GetComponent<Image>();
        RectTransform transform = GetComponent<RectTransform>();
        if (sp)
        {
            transform.sizeDelta = new Vector2(41, 175);
            transform.anchoredPosition = new Vector2(-444, 680);
            renderer.sprite = sprites[1];
            sp = false;
        }
        else
        {
            transform.anchoredPosition = new Vector2(-444, 949);
            transform.sizeDelta = new Vector2(41, 41);
            renderer.sprite = sprites[0];
            sp = true;
        }
    }
    
}
