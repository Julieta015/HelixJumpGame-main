using UnityEngine;
using TMPro;

public class NextLevel : MonoBehaviour
{

    public TextMeshProUGUI levelText;

    void Start()
    {
        if (Level.level == 0)
        {
            levelText.text = 2.ToString();
        }
        else
        {
            levelText.text = (Level.level + 1).ToString();
        }
        
    }
}