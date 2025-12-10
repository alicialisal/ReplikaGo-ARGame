using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TapToContinueTour : MonoBehaviour
{
    [Header("Tutorial Images (In Order)")]
    [SerializeField] Image[] tutorialImages;

    // private QuizManager quizManager;

    [Header("Fade")]
    [SerializeField] float fadeDuration = 0.4f;
    // [SerializeField] GameObject quizUI; // Assign QuizUI GameObject
    private int currentIndex = 0;
    private bool isFading = false;

    void Start()
    {
        // PlayerPrefs.DeleteAll();
        // PlayerPrefs.Save();

        // Matikan semua image dulu
        foreach (var img in tutorialImages)
            img.gameObject.SetActive(false);
        
        if (PlayerPrefs.GetInt("tourDone", 0) == 0) {
            // Mulai di image pertama
            ShowImage(currentIndex);
        }
        else
        {
            // quizUI.SetActive(true);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("tourDone", 0) == 1) return;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            NextImage();
        }
    }

    void NextImage()
    {
        if (currentIndex < tutorialImages.Length - 1)
        {
            StartCoroutine(SwitchImage(currentIndex, currentIndex + 1));
            currentIndex++;
        }
        else
        {
            // Terakhir → hilangkan
            StartCoroutine(FadeOutImage(tutorialImages[currentIndex]));
            Debug.Log("Tutorial selesai!");

            PlayerPrefs.SetInt("tourDone", 1);
            PlayerPrefs.Save();

            this.enabled = false;
            // Aktifkan QuizUI
            // quizUI.SetActive(true);

            // quizUI.GetComponent<QuizManager>().StartQuizForModel(0);
        }
        
        Debug.Log(PlayerPrefs.GetInt("tourDone", 0));
    }

    void ShowImage(int index)
    {
        var img = tutorialImages[index];
        img.gameObject.SetActive(true);

        CanvasGroup cg = img.GetComponent<CanvasGroup>();
        if (cg == null) cg = img.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        StartCoroutine(FadeCanvasGroup(cg, 0, 1, fadeDuration));
    }

    IEnumerator SwitchImage(int from, int to)
    {
        isFading = true;

        Image imgOut = tutorialImages[from];
        Image imgIn = tutorialImages[to];

        CanvasGroup cgOut = imgOut.GetComponent<CanvasGroup>();
        if (cgOut == null) cgOut = imgOut.gameObject.AddComponent<CanvasGroup>();

        CanvasGroup cgIn = imgIn.GetComponent<CanvasGroup>();
        if (cgIn == null) cgIn = imgIn.gameObject.AddComponent<CanvasGroup>();

        imgIn.gameObject.SetActive(true);
        cgIn.alpha = 0;

        yield return StartCoroutine(FadeCanvasGroup(cgOut, 1, 0, fadeDuration));
        imgOut.gameObject.SetActive(false);

        yield return StartCoroutine(FadeCanvasGroup(cgIn, 0, 1, fadeDuration));

        isFading = false;
    }

    IEnumerator FadeOutImage(Image img)
    {
        isFading = true;

        CanvasGroup cg = img.GetComponent<CanvasGroup>();
        if (cg == null) cg = img.gameObject.AddComponent<CanvasGroup>();

        yield return StartCoroutine(FadeCanvasGroup(cg, 1, 0, fadeDuration));
        img.gameObject.SetActive(false);

        isFading = false;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
