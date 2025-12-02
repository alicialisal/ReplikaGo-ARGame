using UnityEngine;
using UnityEngine.UI;

public class AudioToggleButton : MonoBehaviour
{
    [Header("Assign Images")]
    [SerializeField] Image audioButton;
    [SerializeField] Image slashImage;     // The "/" overlay image
    [SerializeField] float fadeDuration = 0.25f;

    [Header("Optional Audio Source")]
    [SerializeField] AudioSource audioSource;

    private bool isMuted = false;
    private float targetAlpha = 0f;

    void Start()
    {
        // Ensure slash starts hidden
        SetSlashAlpha(0f);
    }

    public void ToggleAudio()
    {
        isMuted = AudioManager.Instance.ToggleMute();

        float targetAlpha = isMuted ? 1f : 0f;
        StopAllCoroutines();
        StartCoroutine(FadeSlash(targetAlpha));
    }

    private System.Collections.IEnumerator FadeSlash(float target)
    {
        float start = slashImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float a = Mathf.Lerp(start, target, time / fadeDuration);
            SetSlashAlpha(a);
            time += Time.deltaTime;
            yield return null;
        }

        SetSlashAlpha(target);
    }

    private void SetSlashAlpha(float a)
    {
        Color c = slashImage.color;
        c.a = a;
        slashImage.color = c;
    }
}
