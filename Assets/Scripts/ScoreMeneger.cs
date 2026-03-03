using UnityEngine;
using TMPro; // Եթե օգտագործում ես TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddRandomScore()
    {
        // Ավելացնում ենք 20-ից 30 պատահական թիվ
        int randomAdd = Random.Range(20, 31); // 31-ը ներառված չէ, դրա համար 31
        currentScore += randomAdd;

        // Թարմացնում ենք տեքստը
        scoreText.text = currentScore.ToString();

        Debug.Log("Ավելացավ: " + randomAdd + " | Ընդհանուր: " + currentScore);
    }
}