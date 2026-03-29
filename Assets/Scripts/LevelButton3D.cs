using UnityEngine;

public class LevelButton3D : MonoBehaviour
{
    public int levelToLoad;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ՍՏՈՒԳՈՒՄ. Արդյո՞ք այն օբյեկտը, որին կպել է ճառագայթը, հենց ՍԱ Է
                if (hit.transform == this.transform || hit.transform.IsChildOf(this.transform))
                {
                    HandleLevelClick(levelToLoad);
                }
            }
        }
    }

    void HandleLevelClick(int levelNum)
    {
        if (levelNum <= BallBounce.completLevel)
        {
            Debug.Log("Ընտրվեց Լեվել: " + levelNum);
            Level.SelectLevel(levelNum);
        }
        
    }
}
