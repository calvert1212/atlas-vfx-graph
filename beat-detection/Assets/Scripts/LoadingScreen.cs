using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private string[] loadingTips;
    [SerializeField] private float tipChangeInterval = 5f;
    
    private void OnEnable()
    {
        StartCoroutine(AnimateLoadingBar());
        StartCoroutine(CycleThroughTips());
    }
    
    private IEnumerator AnimateLoadingBar()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = 0;
            
            while (true)
            {
                // Get actual loading progress if available
                float targetProgress = 1.0f;
                
                // Smoothly animate towards the target progress
                while (progressBar.fillAmount < targetProgress)
                {
                    progressBar.fillAmount = Mathf.MoveTowards(
                        progressBar.fillAmount, 
                        targetProgress, 
                        Time.deltaTime * 0.5f
                    );
                    
                    if (progressText != null)
                    {
                        progressText.text = $"{Mathf.Round(progressBar.fillAmount * 100)}%";
                    }
                    
                    yield return null;
                }
                
                yield return null;
            }
        }
    }
    
    private IEnumerator CycleThroughTips()
    {
        if (tipText != null && loadingTips != null && loadingTips.Length > 0)
        {
            int currentTipIndex = 0;
            
            while (true)
            {
                tipText.text = loadingTips[currentTipIndex];
                
                yield return new WaitForSeconds(tipChangeInterval);
                
                currentTipIndex = (currentTipIndex + 1) % loadingTips.Length;
            }
        }
    }
}
