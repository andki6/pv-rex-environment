using UnityEngine;

public class SoundEnvironmentAnimation : EnvironmentAnimation
{
    public GameObject AudioSourceObject; // Assign this in the Inspector
    public float MaxVolume = 1.0f; // The maximum volume of the AudioSource

    private AudioSource audioSource;
    private bool isPlaying = false;

    public override void Init()
    {
        // Initialize the AudioSource component
        if (AudioSourceObject != null)
        {
            audioSource = AudioSourceObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("SoundEnvironmentAnimation: AudioSource component not found on the GameObject.");
                return;
            }

            // Start with the AudioSource volume at 0
            audioSource.volume = 0f;
            audioSource.Stop();
        }
        else
        {
            Debug.LogError("SoundEnvironmentAnimation: AudioSourceObject is not assigned.");
        }
    }

    public override void UpdateAnimation(float progress)
    {
        if (progress == 0f) return;
        if (!isPlaying && audioSource != null)
        {
            audioSource.Play();
            isPlaying = true;
        }

        // Adjust the volume based on the progress, from 0 to MaxVolume
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Lerp(0, MaxVolume, progress);
        }
    }

    void Update()
    {
        // Ensure the sound continues to play at max volume after the animation is complete
        if (isPlaying && audioSource.volume < MaxVolume)
        {
            audioSource.volume = MaxVolume;
        }
    }
}
