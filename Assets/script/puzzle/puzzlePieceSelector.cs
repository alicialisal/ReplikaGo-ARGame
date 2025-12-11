using UnityEngine;

public class PuzzlePieceSelector : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }
    public static PuzzlePieceSelector currentSelected;

    [Header("Visual Feedback")]
    
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("Rotation Settings")]
    [SerializeField] private RotationAxis rotationAxis; // Bisa diubah di Inspector
    [SerializeField] private bool useGlobalY = true; // Gunakan sumbu Y global atau lokal

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

    // 🔁 Rotasi berdasarkan sumbu yang dipilih di Inspector
    public void RotatePiece(float degrees)
    {
        // if (useGlobalY && rotationAxis == RotationAxis.Y)
        // {
        //     // Putar di sumbu global Y (vertikal dunia)
        //     transform.Rotate(Vector3.up, degrees, Space.World);
        // }
        // else
        // {
        //     // Gunakan rotasi lokal untuk sumbu lain
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    transform.Rotate(Vector3.right, degrees, Space.Self);
                    break;
                case RotationAxis.Z:
                    transform.Rotate(Vector3.forward, degrees, Space.Self);
                    break;
                case RotationAxis.Y:
                    transform.Rotate(Vector3.up, degrees, Space.Self);
                    break;
            }
        // }
    }

    // Putar di sumbu Z (seperti memutar di layar 2D)
    public void RotatePieceZ(float degrees)
    {
        transform.Rotate(Vector3.forward, degrees, Space.Self);
    }   

    // Putar di sumbu X (miring ke depan/belakang)
    public void RotatePieceX(float degrees)
    {
        transform.Rotate(Vector3.right, degrees, Space.Self);
    }

    // Tambahkan di dalam class PuzzlePieceSelector
    public float GetCurrentYRotation()
    {
        return transform.eulerAngles.y;
    }

    public float GetCurrentZRotation()
    {
        return transform.eulerAngles.z;
    }

    public float GetCurrentXRotation()
    {
        return transform.eulerAngles.x;
    }
}