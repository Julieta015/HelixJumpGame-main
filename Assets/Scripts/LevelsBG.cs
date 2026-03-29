using UnityEngine;

public class LevelsBG : MonoBehaviour
{
    Transform bg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //if (Level.level == 0)
        //{
        //    bg = transform.GetChild(0);
        //}
        //else
        //{
            bg = transform.GetChild(Level.level-1);
        //}
        
        bg.gameObject.SetActive(true);
    }

    
}
