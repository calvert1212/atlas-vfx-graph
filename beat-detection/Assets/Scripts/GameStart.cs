using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    public FadeToBlack fadeToBlack;
    public GameObject jukeBox;

    void Start()
    {
        if (fadeToBlack != null)
        {
            fadeToBlack.StartFade();
        }

        if (jukeBox != null)
        {
            StartCoroutine(StartJukeBox());
        }
    }

    private IEnumerator StartJukeBox()
    {
        yield return new WaitForSeconds(1f);
        AudioSource audioSource = jukeBox.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
