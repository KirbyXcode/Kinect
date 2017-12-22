using UnityEngine;
using DevelopEngine;

public class AudioManager : MonoSingleton<AudioManager> 
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        AudioClip clip = Factory.Instance.LoadSound(name);
        audioSource.clip = clip;
        audioSource.Play();
    }
}
