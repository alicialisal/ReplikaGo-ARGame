using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Tooltip("Daftar potongan puzzle yang sudah ditempatkan di scene (dalam keadaan disabled)")]
    [SerializeField] private GameObject[] puzzlePieces;

    /// <summary>
    /// Aktifkan semua potongan puzzle dan beri rotasi acak di sumbu Z.
    /// </summary>
    public void ActivatePuzzle()
    {
        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            Debug.LogWarning("[PuzzleManager] Tidak ada potongan puzzle yang ditetapkan.");
            return;
        }

        foreach (GameObject piece in puzzlePieces)
        {
            if (piece == null) continue;

            // Beri rotasi acak hanya di sumbu Z (dalam ruang lokal)
            Vector3 currentEuler = piece.transform.localEulerAngles;
            piece.transform.localEulerAngles = new Vector3(
                currentEuler.x,
                Random.Range(0f, 360f),
                currentEuler.z
            );

            // Aktifkan objek
            piece.SetActive(true);
        }
        
        // PuzzleChecker.Instance.OnPuzzleSpawned();
        Debug.Log("[PuzzleManager] Puzzle diaktifkan dengan rotasi acak.");
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