using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // Events for different game phases
    public static event Action OnLoadingComplete;
    public static event Action OnGameplayStart;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject countdownObject;
    [SerializeField] private GameObject gameplayElements;
    
    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadingTime = 2f; // Ensures loading screen shows for at least this duration
    [SerializeField] private bool simulateLoadingDelay = false;
    [SerializeField] private float simulatedLoadingTime = 3f;
    
    private bool _isLoaded = false;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to the countdown complete event
        CountdownText.OnCountdownComplete += StartGameplay;
    }
    
    private void OnDisable()
    {
        // Always unsubscribe when disabled
        CountdownText.OnCountdownComplete -= StartGameplay;
    }
    
    private void Start()
    {
        // Initialize game state
        if (loadingScreen) loadingScreen.SetActive(true);
        if (countdownObject) countdownObject.SetActive(false);
        if (gameplayElements) gameplayElements.SetActive(false);
        
        // Start the loading process
        StartCoroutine(LoadGameRoutine());
    }
    
    private IEnumerator LoadGameRoutine()
    {
        Debug.Log("Starting game loading process...");
        
        float startTime = Time.time;
        
        // Load all necessary assets and initialize systems
        yield return StartCoroutine(LoadAllGameSystems());
        
        // Ensure minimum loading time is respected
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadingTime)
        {
            yield return new WaitForSeconds(minimumLoadingTime - elapsedTime);
        }
        
        // Mark loading as complete
        _isLoaded = true;
        
        // Hide loading screen and show countdown
        if (loadingScreen) loadingScreen.SetActive(false);
        
        // Notify any listeners that loading is complete
        OnLoadingComplete?.Invoke();
        
        // Start the countdown
        if (countdownObject) countdownObject.SetActive(true);
    }
    
    private IEnumerator LoadAllGameSystems()
    {
        // Load all your game systems here
        // For example:
        yield return StartCoroutine(LoadAudioSystem());
        yield return StartCoroutine(LoadPlayerData());
        yield return StartCoroutine(LoadLevelData());
        
        // Simulate loading time if needed (for testing)
        if (simulateLoadingDelay)
        {
            yield return new WaitForSeconds(simulatedLoadingTime);
        }
    }
    
    private IEnumerator LoadAudioSystem()
    {
        Debug.Log("Loading audio system...");
        // Initialize your audio system here
        // For example, preload common sound effects
        yield return null;
    }
    
    private IEnumerator LoadPlayerData()
    {
        Debug.Log("Loading player data...");
        // Load player progress, settings, etc.
        yield return null;
    }
    
    private IEnumerator LoadLevelData()
    {
        Debug.Log("Loading level data...");
        // Load level-specific data
        yield return null;
    }
    
    private void StartGameplay()
    {
        Debug.Log("Countdown complete! Starting gameplay...");
        
        // Enable all gameplay elements
        if (gameplayElements) gameplayElements.SetActive(true);
        
        // Notify any listeners that gameplay has started
        OnGameplayStart?.Invoke();
    }
    
    // Public method to check if game is fully loaded
    public bool IsGameLoaded()
    {
        return _isLoaded;
    }
}
