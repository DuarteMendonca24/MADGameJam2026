using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        soundObject.clip = audioClip;

        soundObject.volume = volume;

        soundObject.Play();
    }

    public void StopSoundClip() 
    {
        soundObject.Stop();
    }
}
