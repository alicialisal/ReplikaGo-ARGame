using UnityEngine;
using System.Collections;

public class FeedbackManager : MonoBehaviour // atau class apa pun yang sesuai
{
    [SerializeField] private GameObject tryAgain; // Tarik di Inspector
    [SerializeField] private float displayDuration = 2.5f; // Durasi muncul (2-3 detik)
    [SerializeField] private float fadeDuration = 0.5f;   // Durasi fade out

    public void ShowTryAgain()
    {
        if (tryAgain == null)
        {
            Debug.LogError("tryAgain GameObject not assigned!");
            return;
        }

        // Pastikan CanvasGroup ada
        CanvasGroup canvasGroup = tryAgain.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = tryAgain.AddComponent<CanvasGroup>();

        // Mulai coroutine
        StartCoroutine(TryAgainSequence(canvasGroup));
    }

    IEnumerator TryAgainSequence(CanvasGroup canvasGroup)
    {
        // Aktifkan objek & set alpha ke 1
        tryAgain.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;

        // Tunggu selama displayDuration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = 1f - (elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pastikan alpha = 0 dan nonaktifkan
        canvasGroup.alpha = 0f;
        tryAgain.gameObject.SetActive(false);
    }
}