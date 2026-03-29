using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private const string SavedLevelKey = "SavedLevel";
    public static int level;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BootstrapProgress()
    {
        EnsureProgressLoaded();
    }

    public static void EnsureProgressLoaded()
    {
        int savedLevel = Mathf.Max(1, PlayerPrefs.GetInt(SavedLevelKey, 1));

        if (BallBounce.completLevel < savedLevel)
        {
            BallBounce.completLevel = savedLevel;
        }

        if (level <= 0 || level > BallBounce.completLevel)
        {
            level = BallBounce.completLevel;
        }
    }

    public static void SelectLevel(int selectedLevel)
    {
        EnsureProgressLoaded();
        level = Mathf.Clamp(selectedLevel, 1, BallBounce.completLevel);
    }

    public static void UnlockNextLevel()
    {
        EnsureProgressLoaded();
        BallBounce.completLevel++;
        level = BallBounce.completLevel;

        PlayerPrefs.SetInt(SavedLevelKey, BallBounce.completLevel);
        PlayerPrefs.Save();
    }

}
