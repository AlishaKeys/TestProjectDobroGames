using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip musicClip;

    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}
