using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzlePieces;
    [SerializeField] private Transform spawnParent; // Biasanya ModelTarget atau child-nya
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.2f, 0.2f); // Naik + maju sedikit
    [SerializeField] private float scaleFactor = 5f; // 🔑 Atur di sini via Inspector! (default: 5x)

    public void SpawnPuzzle()
    {
        // 🔍 Validasi awal
        if (spawnParent == null)
        {
            Debug.LogError("[PuzzleSpawner] spawnParent is NULL! Assign it in Inspector.");
            return;
        }

        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            Debug.LogWarning("[PuzzleSpawner] No puzzle pieces assigned.");
            return;
        }

        Debug.Log($"[PuzzleSpawner] Spawning {puzzlePieces.Length} pieces at {spawnParent.name} (scale: {scaleFactor}x)");

        foreach (GameObject piece in puzzlePieces)
        {
            if (piece == null)
            {
                Debug.LogWarning("[PuzzleSpawner] Skipping null puzzle piece in array.");
                continue;
            }

            // 📦 Instantiate
            GameObject obj = Instantiate(piece, spawnParent);
            obj.name = "PuzzlePiece_" + piece.name; // opsional: rename untuk debugging

            // 📍 Posisi: offset dari parent
            obj.transform.localPosition += spawnOffset;

            // 🌀 Rotasi acak di sumbu Z (seperti permintaan Anda)
            obj.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            // 🔍 Perbesar ukuran
            obj.transform.localScale *= scaleFactor; // kalikan skala asli prefab

            // ✅ Aktifkan objek (jaga-jaga jika prefab inactive)
            obj.SetActive(true);
        }

        Debug.Log("[PuzzleSpawner] Spawn completed.");
    }
}