using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Tooltip("Daftar potongan puzzle yang sudah ditempatkan di scene (dalam keadaan disabled)")]
    [SerializeField] private GameObject[] puzzlePieces;
    [Header("Base Rotations (Harus sejajar dengan puzzlePieces)")]
    [SerializeField] private Vector3[] baseRotations; // Rotasi dasar per potongan

    public void ActivatePuzzle()
    {
        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            Debug.LogWarning("[PuzzleManager] Tidak ada potongan puzzle yang ditetapkan.");
            return;
        }

        // Pastikan array sejajar
        if (baseRotations.Length != puzzlePieces.Length)
        {
            Debug.LogError($"[PuzzleManager] Jumlah baseRotations ({baseRotations.Length}) tidak sama dengan puzzlePieces ({puzzlePieces.Length})!");
            return;
        }

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            GameObject piece = puzzlePieces[i];
            if (piece == null) continue;

            // 🔥 Set rotasi dasar dari array
            piece.transform.localRotation = Quaternion.Euler(baseRotations[i]);

            // Tambahkan rotasi acak hanya di sumbu Y
            float randomY = Random.Range(0f, 360f);
            piece.transform.Rotate(Vector3.up, randomY, Space.Self);

            piece.SetActive(true);
        }

        Debug.Log("[PuzzleManager] Puzzle diaktifkan dengan rotasi acak (hanya sumbu Y).");
    }

    /// <summary>
    /// Nonaktifkan semua potongan puzzle (opsional, untuk reset).
    /// </summary>
    public void DeactivatePuzzle()
    {
        if (puzzlePieces == null) return;

        foreach (GameObject piece in puzzlePieces)
        {
            if (piece != null)
                piece.SetActive(false);
        }

        Debug.Log("[PuzzleManager] Puzzle dinonaktifkan.");
    }
}