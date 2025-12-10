using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public static PuzzleChecker Instance;

    [Header("Puzzle Setup")]
    [SerializeField] private PuzzleUIManager uiManager; // Tarik di Inspector
    [SerializeField] private PuzzlePieceSelector[] puzzlePieces;
    [SerializeField] private ExperienceManager experienceManager;
    [SerializeField] private float toleranceDegrees = 30f; // ±10°

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool IsPuzzleSolved()
    {
        if (puzzlePieces.Length <= 1) return true;

        // Ambil rotasi potongan pertama sebagai referensi
        float baseRotation = puzzlePieces[0].GetCurrentYRotation();

        for (int i = 1; i < puzzlePieces.Length; i++)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(
                baseRotation,
                puzzlePieces[i].GetCurrentYRotation()
            ));

            string status = diff <= toleranceDegrees ? "✅ PASS" : "❌ FAIL";
            Debug.Log($"[PuzzleChecker] {puzzlePieces[i].name} vs base: Δ={diff:F1}° | {status}");

            if (diff > toleranceDegrees)
            {
                Debug.Log($"[PuzzleChecker] 🛑 {puzzlePieces[i].name} exceeds tolerance ({toleranceDegrees}°)");
                return false;
            }
        }

        Debug.Log("[PuzzleChecker] 🎉 All pieces aligned!");
        PlayerPrefs.SetInt("puzzleCaseSolved", 1);
        PlayerPrefs.Save();
        return true;
    }

    public void OnCheckButtonPressed()
    {
        if (IsPuzzleSolved())
        {
            int puzzleExp = 200; // Sesuaikan
            experienceManager.AddExperience(puzzleExp);
            Debug.Log($"Puzzle solved! +{puzzleExp} EXP");

            // Beri tahu UI Manager
            if (uiManager != null)
                uiManager.OnPuzzleSolved();
            }
        else
        {
            Debug.Log("Puzzle not solved yet.");
        }
    }
}