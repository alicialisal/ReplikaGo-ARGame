using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private ExperienceManager experienceManager;
    // --- UI References (TMP) ---
    public Image bgImage;
    public TMP_Text judulText;         // "Kuis!" title
    public TMP_Text petunjuk;
    public TMP_Text pertanyaanText;    // Question text
    public Button optionAButton;       // Tetap Button biasa
    public Button optionBButton;
    public Button optionCButton;
    public Button optionDButton;
    public Button submitButton;

    // --- TMP Text inside Buttons ---
    public TMP_Text optionAText;
    public TMP_Text optionBText;
    public TMP_Text optionCText;
    public TMP_Text optionDText;

    // --- Background Sprites ---
    public Sprite bgNormal;
    public Sprite bgSelected;
    public Sprite bgCorrect;
    public Sprite bgWrong;

    // --- Data Structure ---
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;
        public int correctAnswerIndex; // 0=A, 1=B, 2=C, 3=D
    }

    [System.Serializable]
    public class ModelTargetData
    {
        public string modelName;
        public List<QuizQuestion> questions;
    }

    public List<ModelTargetData> modelTargets = new List<ModelTargetData>();

    // --- State ---
    private int currentModelIndex = -1;
    private int currentQuestionIndex = 0;
    private bool isQuizActive = false;
    private int selectedOption = -1;

    public delegate void OnQuizCompleted(string modelName);
    public static event OnQuizCompleted QuizCompleted;

    void Start()
    {
        Debug.Log("QuizManager initialized");
        HideQuizUI();

        // Attach button listeners
        if (optionAButton != null) optionAButton.onClick.AddListener(() => SelectOption(0));
        if (optionBButton != null) optionBButton.onClick.AddListener(() => SelectOption(1));
        if (optionCButton != null) optionCButton.onClick.AddListener(() => SelectOption(2));
        if (optionDButton != null) optionDButton.onClick.AddListener(() => SelectOption(3));
        if (submitButton != null) submitButton.onClick.AddListener(OnSubmitClicked);
    }

    public void StartQuizForModel(int modelIndex)
    {
        Debug.Log("Quiz manager UI called for model index: " + modelIndex);
        if (modelIndex < 0 || modelIndex >= modelTargets.Count)
        {
            Debug.LogError("Invalid model index: " + modelIndex);
            return;
        }

        currentModelIndex = modelIndex;
        currentQuestionIndex = 0;
        selectedOption = -1;
        isQuizActive = true;

        ShowQuizUI();
        LoadCurrentQuestion();
        ResetButtonStates();
    }

    private void LoadCurrentQuestion()
    {
        if (currentModelIndex == -1 || !isQuizActive) return;

        var model = modelTargets[currentModelIndex];
        if (currentQuestionIndex >= model.questions.Count)
        {
            EndQuizForModel(model.modelName);
            return;
        }

        var q = model.questions[currentQuestionIndex];

        judulText.text = "Kuis!";
        pertanyaanText.text = q.question;
        optionAText.text = q.optionA;
        optionBText.text = q.optionB;
        optionCText.text = q.optionC;
        optionDText.text = q.optionD;

        ResetButtonStates();
        selectedOption = -1;
        Debug.Log("Loaded question: " + q.question);
    }

    private void SelectOption(int index)
    {
        if (!isQuizActive) return;
        selectedOption = index;

        ResetButtonStates();

        Button btn = GetButtonByIndex(index);
        if (btn != null && btn.image != null)
            btn.image.sprite = bgSelected;
    }

    private void OnSubmitClicked()
    {
        if (!isQuizActive || selectedOption == -1 || currentModelIndex == -1) return;

        var q = modelTargets[currentModelIndex].questions[currentQuestionIndex];

        // Update visuals: correct = green, wrong = red, others = normal
        for (int i = 0; i < 4; i++)
        {
            Button btn = GetButtonByIndex(i);
            if (btn == null || btn.image == null) continue;

            if (i == q.correctAnswerIndex)
                btn.image.sprite = bgCorrect;
            else if (i == selectedOption)
                btn.image.sprite = bgWrong;
            else
                btn.image.sprite = bgNormal;
        }

        if (selectedOption == q.correctAnswerIndex)
        {
            Invoke(nameof(LoadNextQuestion), 1.5f);
            experienceManager.AddExperience(100); // Tambahkan EXP di sini
            Debug.Log("✅ Benar! EXP 100 diberikan.");
        }
        else
        {
            Debug.Log("❌ Salah!");
            // Optional: allow retry or auto-next
        }
    }

    private void LoadNextQuestion()
    {
        currentQuestionIndex++;
        LoadCurrentQuestion();
    }

    private void EndQuizForModel(string modelName)
    {
        isQuizActive = false;
        HideQuizUI();
        QuizCompleted?.Invoke(modelName);
        Debug.Log($"Quiz selesai untuk: {modelName}");
    }

    private Button GetButtonByIndex(int index)
    {
        switch (index)
        {
            case 0: return optionAButton;
            case 1: return optionBButton;
            case 2: return optionCButton;
            case 3: return optionDButton;
            default: return null;
        }
    }

    private void SetButtonBackground(Button btn, Sprite sprite)
    {
        if (btn != null && btn.image != null)
            btn.image.sprite = sprite;
    }

    private void ResetButtonStates()
    {
        SetButtonBackground(optionAButton, bgNormal);
        SetButtonBackground(optionBButton, bgNormal);
        SetButtonBackground(optionCButton, bgNormal);
        SetButtonBackground(optionDButton, bgNormal);
    }

    private void ShowQuizUI()
    {
        SetActiveSafe(bgImage, true);
        SetActiveSafe(judulText, true);
        SetActiveSafe(petunjuk, true);
        SetActiveSafe(pertanyaanText, true);
        SetActiveSafe(optionAButton, true);
        SetActiveSafe(optionBButton, true);
        SetActiveSafe(optionCButton, true);
        SetActiveSafe(optionDButton, true);
        SetActiveSafe(submitButton, true);
        Debug.Log("Quiz UI shown");
    }

    private void HideQuizUI()
    {
        // Debug.Log("Quiz UI hidden - called from: " + Environment.StackTrace.Substring(0, 200));
        SetActiveSafe(bgImage, false);
        SetActiveSafe(judulText, false);
        SetActiveSafe(petunjuk, false);
        SetActiveSafe(pertanyaanText, false);
        SetActiveSafe(optionAButton, false);
        SetActiveSafe(optionBButton, false);
        SetActiveSafe(optionCButton, false);
        SetActiveSafe(optionDButton, false);
        SetActiveSafe(submitButton, false);
        Debug.Log("Quiz UI hidden");
    }

    private void SetActiveSafe(Component comp, bool active)
    {
        if (comp != null && comp.gameObject != null)
            comp.gameObject.SetActive(active);
    }

    public void ResetQuiz()
    {
        currentModelIndex = -1;
        currentQuestionIndex = 0;
        selectedOption = -1;
        isQuizActive = false;
        HideQuizUI();
        Debug.Log("Quiz reset");
    }
}