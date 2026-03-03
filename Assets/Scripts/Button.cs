using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [Header("Click sound")]
    public AudioClip clickSound;      // Քաշիր այստեղ քո ձայնի ֆայլը
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
    }

    public void Play() 
    {
        PlayClickSound();
        SceneManager.LoadScene(0);
    }

    public void Map()
    {
        PlayClickSound();
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {       
        PlayClickSound();
        SceneManager.LoadScene(0);       
    }

    public void Skin()
    {
        PlayClickSound();
        // Skin-ի տրամաբանությունը ավելացրու այստեղ
    }
}
