using UnityEngine;
using UnityEngine.UI;

public class FireworkControlUI : MonoBehaviour
{
    public PatrioticFireworkLauncher fireworkLauncher;
    public Toggle autoLaunchToggle;
    public Slider intervalSlider;
    public Text intervalText;
    public Button[] specificFireworkButtons;
    
    void Start()
    {
        if (fireworkLauncher == null)
        {
            Debug.LogError("Firework Launcher not assigned!");
            return;
        }
        
        // Set up auto launch toggle
        if (autoLaunchToggle != null)
        {
            autoLaunchToggle.isOn = fireworkLauncher.enableAutoLaunch;
            autoLaunchToggle.onValueChanged.AddListener(OnAutoLaunchToggled);
        }
        
        // Set up interval slider
        if (intervalSlider != null)
        {
            intervalSlider.value = fireworkLauncher.autoLaunchInterval;
            intervalSlider.onValueChanged.AddListener(OnIntervalChanged);
            UpdateIntervalText();
        }
        
        // Set up specific firework buttons
        for (int i = 0; i < specificFireworkButtons.Length; i++)
        {
            if (specificFireworkButtons[i] != null)
            {
                int index = i; // Capture for lambda
                specificFireworkButtons[i].onClick.AddListener(() => OnFireworkButtonClicked(index));
            }
        }
    }
    
    void OnAutoLaunchToggled(bool isOn)
    {
        fireworkLauncher.enableAutoLaunch = isOn;
    }
    
    void OnIntervalChanged(float value)
    {
        fireworkLauncher.autoLaunchInterval = value;
        UpdateIntervalText();
    }
    
    void UpdateIntervalText()
    {
        if (intervalText != null)
        {
            intervalText.text = $"Interval: {fireworkLauncher.autoLaunchInterval:F1}s";
        }
    }
    
    void OnFireworkButtonClicked(int index)
    {
        fireworkLauncher.LaunchSpecificFirework(index);
    }
}
