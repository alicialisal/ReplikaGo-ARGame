using UnityEngine;
using System.Collections.Generic;

public class PuzzleRotationManager : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationStep = 30f;

    [SerializeField] private List<PuzzlePieceSelector> puzzlePieces = new List<PuzzlePieceSelector>();
    private int currentIndex = -1;

    void Start()
    {
        if (puzzlePieces.Count == 0)
        {
            Debug.LogError("[PuzzleNavigationManager] No puzzle pieces assigned!");
            return;
        }
        SelectPiece(0); // Start with first piece
        Debug.Log("Selected piece index: 0");
    }

    public void SelectNext()
    {
        if (puzzlePieces.Count == 0) return;
        currentIndex = (currentIndex + 1) % puzzlePieces.Count;
        SelectPiece(currentIndex);
        Debug.Log("Selected piece index: " + currentIndex);
    }

    public void SelectPrevious()
    {
        if (puzzlePieces.Count == 0) return;
        currentIndex = (currentIndex - 1 + puzzlePieces.Count) % puzzlePieces.Count;
        SelectPiece(currentIndex);
        Debug.Log("Selected piece index: " + currentIndex);
    }

    void SelectPiece(int index)
    {
        if (index >= 0 && index < puzzlePieces.Count)
        {
            puzzlePieces[index].Select();
        }
    }

    // Optional: Get currently selected piece for rotation
    public PuzzlePieceSelector GetSelectedPiece()
    {
        return PuzzlePieceSelector.currentSelected;
    }
    public void RotateSelectedLeft()
    {
        if (PuzzlePieceSelector.currentSelected != null)
        {
            PuzzlePieceSelector.currentSelected.RotatePiece(-rotationStep);
            Debug.Log("Rotated piece left by " + rotationStep + " degrees.");
        }
        else
        {
            Debug.LogWarning("No puzzle piece selected!");
        }
    }

    public void RotateSelectedRight()
    {
        if (PuzzlePieceSelector.currentSelected != null)
        {
            PuzzlePieceSelector.currentSelected.RotatePiece(rotationStep);
            Debug.Log("Rotated piece right by " + rotationStep + " degrees.");
        }
        else
        {
            Debug.LogWarning("No puzzle piece selected!");
        }
    }
}