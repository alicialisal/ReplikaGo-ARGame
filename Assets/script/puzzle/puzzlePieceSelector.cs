using UnityEngine;

public class PuzzlePieceSelector : MonoBehaviour
{
    public static PuzzlePieceSelector currentSelected;

    [Header("Visual Feedback")]
    [SerializeField] private Color selectedColor = Color.yellow;

    private Renderer renderer;
    private Material originalMaterial;
    private Material selectedMaterial;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        originalMaterial = renderer.material;
        selectedMaterial = new Material(originalMaterial);
        selectedMaterial.color = selectedColor;

        Deselect(); // Start unselected
    }

    // Call this from PuzzleManager
    public void Select()
    {
        currentSelected?.Deselect(); // Deselect previous
        currentSelected = this;
        renderer.material = selectedMaterial;
        Debug.Log("Selected: " + name);
    }

    public void Deselect()
    {
        renderer.material = originalMaterial;
    }

    public void RotatePiece(float degrees)
    {
        transform.Rotate(Vector3.up, degrees, Space.World);
    }

    // Tambahkan di dalam class PuzzlePieceSelector
    public float GetCurrentYRotation()
    {
        return transform.eulerAngles.y;
    }
}