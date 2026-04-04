using UnityEngine;
using UnityEngine.UI;


public class LevelProgressBar : MonoBehaviour
{
    public Slider progressBar;


    private int totalRings;
    private int brokenRings;

    public static LevelProgressBar Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 1. Ստանում ենք լեվելի համարները
        int currentLevel = BallBounce.completLevel;


        // 2. Հաշվում ենք, թե քանի հարթակ կա ընդհանուր (առանց վերջինի)
        totalRings = 15;//TowerGenerator.Instance.ringCount - 1;

        // 3. Զրոյացնում ենք գիծը
        progressBar.minValue = 0;
        progressBar.maxValue = totalRings;
        progressBar.value = 0;
    }

    public void IncrementProgress()
    {
        brokenRings++;
        // Թարմացնում ենք գծի արժեքը
        progressBar.value = brokenRings;
    }
}