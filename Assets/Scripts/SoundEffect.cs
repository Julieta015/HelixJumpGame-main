using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource src;
    public AudioClip buttonclick;

    public void ButtonClick()
    {
        src.clip = buttonclick;
        src.Play();
    }

}
