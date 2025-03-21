using System.Collections;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float crossFadeDuration = 2.0f;
    
    private void OnEnable()
    {
        // Subscribe to game events
        GameManager.OnLoadingComplete += OnLoadingComplete;
        GameManager.OnGameplayStart += OnGameplayStart;
    }
    
    private void OnDisable()
    {
        // Always unsubscribe when disabled
        GameManager.OnLoadingComplete -= OnLoadingComplete;
        GameManager.OnGameplayStart -= OnGameplayStart;
    }
    
    private void Start()
    {
        // Make sure we have an audio source
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        // Initially, don't play any music until loading is complete
        audioSource.Stop();
    }
    
    private void OnLoadingComplete()
    {
        // When loading is complete, play menu music if available
        if (menuMusic != null)
        {
            PlayMusic(menuMusic);
        }
    }
    
    private void OnGameplayStart()
    {
        // When gameplay starts, switch to gameplay music
        if (gameplayMusic != null)
        {
            if (audioSource.isPlaying && crossFadeDuration > 0)
            {
                StartCoroutine(CrossFadeMusic(gameplayMusic));
            }
            else
            {
                PlayMusic(gameplayMusic);
            }
        }
    }
    
    private void PlayMusic(AudioClip music)
    {
        if (audioSource != null && music != null)
        {
            audioSource.clip = music;
            
            if (fadeInDuration > 0)
            {
                // Start with volume at 0 and fade in
                audioSource.volume = 0;
                audioSource.Play();
                StartCoroutine(FadeInMusic());
            }
            else
            {
                // Just play at full volume
                audioSource.Play();
            }
        }
    }
    
    private IEnumerator FadeInMusic()
    {
        float startVolume = 0;
        float targetVolume = 1;
        float currentTime = 0;
        
        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeInDuration);
            yield return null;
        }
        
        // Ensure we end at the target volume
        audioSource.volume = targetVolume;
    }
    
    private IEnumerator CrossFadeMusic(AudioClip newMusic)
    {
        // Create a temporary audio source for the new music
        GameObject tempGO = new GameObject("TempAudioSource");
        tempGO.transform.parent = transform;
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        
        // Configure the temporary source
        tempSource.clip = newMusic;
        tempSource.volume = 0;
        tempSource.loop = true;
        tempSource.Play();
        
        // Fade out the current music while fading in the new music
        float currentTime = 0;
        
        while (currentTime < crossFadeDuration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / crossFadeDuration;
            
            audioSource.volume = Mathf.Lerp(1, 0, t);
            tempSource.volume = Mathf.Lerp(0, 1, t);
            
            yield return null;
        }
        
        // Switch to the new music
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.volume = 1;
        audioSource.Play();
        
        // Clean up the temporary audio source
        Destroy(tempGO);
    }
}
