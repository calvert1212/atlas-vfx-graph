using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownText : MonoBehaviour
{
    // Event that will be triggered when countdown completes
    public static event Action OnCountdownComplete;
    
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float delayBetweenNumbers = 1.0f;
    [SerializeField] private string[] countdownSequence = new string[] { "3", "2", "1", "GO!" };
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip numberBeepSound;
    [SerializeField] private AudioClip goSound;
    [SerializeField] private float volumeScale = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(PlayCountdownSequence());
    }

    private IEnumerator PlayCountdownSequence()
    {
        // Make sure we have a reference to the text component
        if (countdownText == null)
        {
            countdownText = GetComponent<TextMeshProUGUI>();
            
            if (countdownText == null)
            {
                Debug.LogError("No TextMeshProUGUI component found!");
                yield break;
            }
        }

        // Make sure we have an audio source
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Display each number in the countdown sequence
        for (int i = 0; i < countdownSequence.Length; i++)
        {
            string number = countdownSequence[i];
            countdownText.text = number;
            
            // Play the appropriate sound
            if (number == "GO!" && goSound != null)
            {
                audioSource.PlayOneShot(goSound, volumeScale);
            }
            else if (numberBeepSound != null)
            {
                audioSource.PlayOneShot(numberBeepSound, volumeScale);
            }
            
            // Add a slight scale animation for each number
            countdownText.transform.localScale = Vector3.one * 1.2f;
            float elapsedTime = 0f;
            float duration = 0.2f;
            
            // Scale down animation
            while (elapsedTime < duration)
            {
                countdownText.transform.localScale = Vector3.Lerp(
                    Vector3.one * 1.2f, 
                    Vector3.one, 
                    elapsedTime / duration
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            countdownText.transform.localScale = Vector3.one;
            
            // Wait for the specified delay
            yield return new WaitForSeconds(delayBetweenNumbers);
        }

        // Trigger the event to notify other components
        OnCountdownComplete?.Invoke();

        // Deactivate the GameObject after the countdown is complete
        gameObject.SetActive(false);
    }
}
