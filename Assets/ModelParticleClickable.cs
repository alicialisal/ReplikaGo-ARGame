using UnityEngine;
using Vuforia;

/// <summary>
/// Script untuk membuat bagian model Vuforia bisa diklik
/// dan menampilkan/menyembunyikan text penjelasan
/// 
/// CARA PAKAI:
/// 1. Attach script ini ke GameObject Icko_2
/// 2. Drag Text bwh ke field "Text Object" di Inspector
/// 3. Adjust settings di Inspector sesuai kebutuhan
/// 4. Build dan test di HP
/// 
/// FILE: Assets/Scripts/ModelPartClickableAR.cs
/// </summary>
public class ModelPartClickableSimple : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("GameObject Text bwh yang akan muncul/hilang")]
    public GameObject textObject;
    
    [Header("Visual Settings")]
    [Tooltip("Aktifkan highlight saat model diklik")]
    public bool useHighlight = true;
    
    [Tooltip("Warna normal model")]
    public Color normalColor = Color.white;
    
    [Tooltip("Warna saat text ditampilkan")]
    public Color selectedColor = Color.yellow;
    
    [Header("AR Settings")]
    [Tooltip("Jarak maksimal raycast (dalam meter)")]
    public float maxRayDistance = 100f;
    
    [Tooltip("Layer yang bisa di-raycast")]
    public LayerMask raycastLayers = -1;
    
    [Header("Collider Settings")]
    [Tooltip("Ukuran Box Collider (jika auto-generate)")]
    public Vector3 colliderSize = new Vector3(0.5f, 0.5f, 0.5f);
    
    [Header("Debug")]
    [Tooltip("Tampilkan debug ray di Scene view")]
    public bool showDebugRay = true;
    
    [Tooltip("Tampilkan debug log di Console")]
    public bool showDebugLog = true;
    
    // Private variables
    private bool isTextVisible = false;
    private Renderer modelRenderer;
    private Material modelMaterial;
    private Color originalColor;
    private Camera arCamera;
    private ModelTargetBehaviour modelTarget;

    void Start()
    {
        LogDebug("=== AR SETUP START ===");
        
        // Find AR Camera
        FindARCamera();
        
        // Setup renderer untuk highlight
        SetupRenderer();
        
        // Setup text object
        SetupTextObject();
        
        // Check/setup collider
        SetupCollider();
        
        // Find Model Target untuk tracking check
        modelTarget = GetComponentInParent<ModelTargetBehaviour>();
        if (modelTarget != null)
        {
            LogDebug("Model Target found: " + modelTarget.name);
        }
        
        LogDebug("=== SETUP COMPLETE ===");
    }

    void Update()
    {
        // Check if Model Target is being tracked (Vuforia specific)
        if (!IsModelTracked())
        {
            return;
        }
        
        // Keyboard test untuk testing di Unity Editor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LogDebug(">>> SPACE pressed - Manual toggle");
            ToggleText();
            return;
        }
        
        // Touch detection untuk mobile AR
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            // Only detect on touch began (bukan drag)
            if (touch.phase == TouchPhase.Began)
            {
                LogDebug(">>> TOUCH at: " + touch.position);
                CheckRaycastHit(touch.position);
            }
        }
        // Mouse detection untuk testing di Unity Editor
        else if (Input.GetMouseButtonDown(0))
        {
            LogDebug(">>> CLICK at: " + Input.mousePosition);
            CheckRaycastHit(Input.mousePosition);
        }

        // Debug ekstrem: cek apakah collider & rigidbody ada
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("❌ NO COLLIDER on " + name);
        }
        else
        {
            Debug.Log("✅ Collider found: " + col.GetType().Name + " | isTrigger: " + col.isTrigger);
        }
    
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("❌ NO RIGIDBODY on " + name);
        }
        else
        {
            Debug.Log("✅ Rigidbody found: IsKinematic=" + rb.isKinematic + ", UseGravity=" + rb.useGravity);
        }
    }

    #region Setup Methods

    void FindARCamera()
    {
        arCamera = Camera.main;
        
        if (arCamera == null)
        {
            // Cari camera dengan nama ARCamera
            GameObject arCameraObj = GameObject.Find("ARCamera");
            if (arCameraObj != null)
            {
                arCamera = arCameraObj.GetComponent<Camera>();
            }
        }
        
        if (arCamera == null)
        {
            // Fallback: cari camera pertama
            arCamera = FindObjectOfType<Camera>();
        }
        
        if (arCamera != null)
        {
            LogDebug("AR Camera found: " + arCamera.name);
        }
        else
        {
            Debug.LogError("AR CAMERA NOT FOUND!");
        }
    }

    void SetupRenderer()
    {
        modelRenderer = GetComponent<Renderer>();
        
        if (modelRenderer != null)
        {
            modelMaterial = modelRenderer.material;
            
            // Try different material property names
            if (modelMaterial.HasProperty("_Color"))
            {
                originalColor = modelMaterial.color;
                LogDebug("Material color saved (_Color)");
            }
            else if (modelMaterial.HasProperty("_BaseColor"))
            {
                originalColor = modelMaterial.GetColor("_BaseColor");
                LogDebug("Material color saved (_BaseColor)");
            }
            else
            {
                originalColor = Color.white;
                LogDebug("Material has no color property, using white");
            }
        }
        else
        {
            Debug.LogWarning("No Renderer found on " + gameObject.name);
        }
    }

    void SetupTextObject()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
            LogDebug("Text Object hidden: " + textObject.name);
        }
        else
        {
            Debug.LogError("TEXT OBJECT IS NULL! Drag Text bwh to Inspector!");
        }
    }

    void SetupCollider()
    {
        Collider col = GetComponent<Collider>();
        
        if (col == null)
        {
            Debug.LogWarning("No Collider found! Adding Box Collider...");
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            box.size = colliderSize;
            LogDebug("Box Collider added with size: " + colliderSize);
        }
        else
        {
            LogDebug("Collider found: " + col.GetType().Name);
            
            // Jika Mesh Collider, warn user
            if (col is MeshCollider)
            {
                MeshCollider meshCol = col as MeshCollider;
                if (!meshCol.convex)
                {
                    Debug.LogWarning("Mesh Collider should be Convex for better click detection!");
                }
            }
        }
    }

    #endregion

    #region Tracking Check

    bool IsModelTracked()
    {
        if (modelTarget != null)
        {
            var status = modelTarget.TargetStatus.Status;
            
            if (status != Status.TRACKED && status != Status.EXTENDED_TRACKED)
            {
                // Model belum ter-track
                return false;
            }
        }
        
        return true;
    }

    #endregion

    #region Raycast Detection

    void CheckRaycastHit(Vector2 screenPosition)
    {
        if (arCamera == null)
        {
            Debug.LogError("AR Camera is NULL! Cannot raycast.");
            return;
        }
        
        // Buat ray dari screen point
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        // Visual debug ray (terlihat di Scene view)
        if (showDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.red, 2f);
        }
        
        LogDebug("Ray Origin: " + ray.origin + " | Direction: " + ray.direction);
        
        // Lakukan raycast
        if (Physics.Raycast(ray, out hit, maxRayDistance, raycastLayers))
        {
            LogDebug(">>> HIT: " + hit.collider.gameObject.name + 
                     " | Distance: " + hit.distance.ToString("F2") + "m" +
                     " | Point: " + hit.point);
            
            // Cek apakah yang di-hit adalah object ini
            if (hit.collider.gameObject == gameObject)
            {
                LogDebug(">>> ✓✓✓ CLICKED ON " + gameObject.name + "! ✓✓✓");
                ToggleText();
            }
            else
            {
                LogDebug(">>> Hit different object: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            LogDebug(">>> Raycast hit NOTHING");
        }
    }

    #endregion

    #region Toggle Text

    public void ToggleText()
    {
        LogDebug(">>> TOGGLE TEXT called");
        isTextVisible = !isTextVisible;
        
        if (textObject != null)
        {
            textObject.SetActive(isTextVisible);
            LogDebug(">>> Text is now: " + (isTextVisible ? "✓ VISIBLE ✓" : "✗ HIDDEN ✗"));
        }
        else
        {
            Debug.LogError(">>> Cannot toggle: Text Object is NULL!");
            return;
        }
        
        // Update highlight
        UpdateHighlight();
    }

    public void ShowText()
    {
        LogDebug(">>> SHOW TEXT called");
        isTextVisible = true;
        
        if (textObject != null)
        {
            textObject.SetActive(true);
        }
        
        UpdateHighlight();
    }

    public void HideText()
    {
        LogDebug(">>> HIDE TEXT called");
        isTextVisible = false;
        
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
        
        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        if (!useHighlight || modelMaterial == null)
        {
            return;
        }
        
        Color targetColor = isTextVisible ? selectedColor : originalColor;
        
        if (modelMaterial.HasProperty("_Color"))
        {
            modelMaterial.color = targetColor;
        }
        else if (modelMaterial.HasProperty("_BaseColor"))
        {
            modelMaterial.SetColor("_BaseColor", targetColor);
        }
        
        LogDebug(">>> Highlight color: " + (isTextVisible ? "Selected" : "Normal"));
    }

    #endregion

    #region Debug Helpers

    void LogDebug(string message)
    {
        if (showDebugLog)
        {
            Debug.Log(message);
        }
    }

    // Draw collider bounds di Scene view
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            
            // Draw label
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(col.bounds.center, "Clickable Area");
            #endif
        }
    }

    // Draw selected collider bounds
    void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }

    #endregion
}