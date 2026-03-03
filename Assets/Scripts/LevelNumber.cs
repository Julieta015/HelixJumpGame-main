using UnityEngine;
using TMPro;

public class LevelNumber : MonoBehaviour
{

    public TextMeshProUGUI levelText;

    void Start()
    {
        if (Level.level == 0)
        {
            levelText.text = 1.ToString();
        }
        else
        {
            levelText.text = Level.level.ToString();
        }
        
    }
}