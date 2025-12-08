using UnityEngine;

public class PuzzleUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rotateUI;           // Canvas Rotate UI
    [SerializeField] private PuzzleManager puzzleManager; // Parent semua potongan
    [SerializeField] private PuzzleChecker puzzleChecker;
    [SerializeField] private QuizManager quizManager; // Tarik di Inspector
    private int currentModelIndex = -1; // Simpan index model saat ini

    private bool isPuzzleActive = false;

    void Start()
    {
        // Pastikan awalnya sembunyikan UI & puzzle
        HidePuzzleAndUI();
    }

    /// <summary>
    /// Dipanggil saat Vuforia mendeteksi item baru
    /// </summary>
    public void OnItemFound(int modelIndex)
    {
        if (isPuzzleActive) return; // Hindari duplikat

        currentModelIndex = modelIndex; // Simpan index model saat ini
        ShowPuzzleAndUI();
        isPuzzleActive = true;
        Debug.Log("Puzzle & UI muncul untuk item baru.");
    }

    /// <summary>
    /// Dipanggil oleh PuzzleChecker saat puzzle solved
    /// </summary>
    public void OnPuzzleSolved()
    {
        if (!isPuzzleActive) return;

        HidePuzzleAndUI();
        isPuzzleActive = false;
        Debug.Log("Puzzle solved! UI & puzzle disembunyikan.");

        if (quizManager != null && currentModelIndex >= 0)
        {
            quizManager.StartQuizForModel(currentModelIndex);
        }
    }

    void ShowPuzzleAndUI()
    {
        if (rotateUI != null) rotateUI.SetActive(true);
        puzzleManager.ActivatePuzzle();
        // if (puzzlePiecesContainer != null) puzzlePiecesContainer.SetActive(true);
    }

    void HidePuzzleAndUI()
    {
        if (rotateUI != null) rotateUI.SetActive(false);
        // if (puzzlePiecesContainer != null) puzzlePiecesContainer.SetActive(false);
        puzzleManager.DeactivatePuzzle();
        
    }
}