using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    [SerializeField] private PuzzleUIManager puzzleUIManager;
    // [SerializeField] private ModelTarget modelTarget;
    // [SerializeField] private List<GameObject> modeldesc;


    protected override void OnTrackingFound()
    {
        Debug.Log("Custom OnTrackingFound called");
        base.OnTrackingFound();

        var managers = FindObjectsOfType<PuzzleUIManager>();
        foreach (var mgr in managers)
        {
            Debug.Log($"Found Puzzle UI Manager: {mgr.gameObject.name}");
            // if (mgr.gameObject.name == "PuzzleManager Keris")
            // {
            //     puzzleUIManager = mgr;
            //     break;
            // }
        }
        
        if (puzzleUIManager == null)
        {
            Debug.Log("[DEBUG] puzzleUIManager is NULL");
            return;
        }
        
        if (puzzleUIManager != null)
        {
            string targetName = GetComponent<ObserverBehaviour>().TargetName;
            Debug.Log($"[DEBUG] TargetName: '{targetName}'"); // Tanda kutip akan tunjukkan spasi
            int modelIndex = GetModelIndexByName(targetName);
            Debug.Log(targetName + "-" + modelIndex);
            HideAllModelDescExcludingPuzzle();

            if (modelIndex >= 0)
            {
                // 🔥 Cek apakah model ini sudah solved
                if (puzzleUIManager.IsModelSolved(modelIndex))
                {
                    // Langsung tampilkan deskripsi (non-puzzle)
                    ShowAllModelDescExcludingPuzzle();
                    Debug.Log("[Vuforia] Model sudah solved. Menampilkan deskripsi langsung.");
                }
                else
                {
                    // Tampilkan puzzle seperti biasa
                    puzzleUIManager.OnItemFound(modelIndex);
                    Debug.Log($"[Vuforia] Model '{targetName}' terdeteksi. Index: {modelIndex}");
                }
            }
            else
            {
                Debug.LogError($"[Vuforia] Model '{targetName}' tidak dikenali!");
            }
        }
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        // Optional: reset puzzle jika hilang
    }

    public void ShowAllModelDescExcludingPuzzle()
    {
        Transform modelTarget = transform; // Karena script ini attach ke ModelTarget

        for (int i = 0; i < modelTarget.childCount; i++)
        {
            GameObject child = modelTarget.GetChild(i).gameObject;

            // Lewati PuzzleManager (atau child yang mengandung "PuzzleManager" di nama)
            if (child.name.Contains("PuzzleManager"))
            {
                continue;
            }

            // Aktifkan semua child lainnya
            child.SetActive(true);
            Debug.Log($"[ShowAllModelDesc] Mengaktifkan: {child.name}");
        }
    }

    public void HideAllModelDescExcludingPuzzle()
    {
        Transform modelTarget = transform; // Karena script ini attach ke ModelTarget
    
        for (int i = 0; i < modelTarget.childCount; i++)
        {
            GameObject child = modelTarget.GetChild(i).gameObject;
    
            // Lewati PuzzleManager (atau child yang mengandung "PuzzleManager" di nama)
            if (child.name.Contains("PuzzleManager"))
            {
                continue;
            }
    
            // Aktifkan semua child lainnya
            child.SetActive(false);
            Debug.Log($"[HideAllModelDesc] Menonaktifkan: {child.name}");
        }
    }


    int GetModelIndexByName(string name)
    {
        switch (name)
        {
            case "data1":
                return 0;
            case "dataset2":
                return 1;
            case "database3":
                return 2;
            default:
                return -1;
        }
    }
}