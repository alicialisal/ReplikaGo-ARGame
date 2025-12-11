using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    // [SerializeField] private PuzzleUIManager puzzleUIManager;
    // [SerializeField] private ModelTarget modelTarget;
    // [SerializeField] private List<GameObject> modeldesc;


    protected override void OnTrackingFound()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("PuzzleManager"))
            {
                child.gameObject.SetActive(true);
                Debug.Log($"[ACTIVATE] Mengaktifkan: {child.name}");
            }
        }

        Debug.Log("Custom OnTrackingFound called");
        base.OnTrackingFound();

        // 🔥 CARI PuzzleUIManager DI CHILD
        PuzzleUIManager localPuzzleUIManager = GetComponentInChildren<PuzzleUIManager>();

        if (localPuzzleUIManager == null)
        {
            Debug.LogError($"[ERROR] Tidak menemukan PuzzleUIManager di child {name}!");
            return;
        }

        string targetName = GetComponent<ObserverBehaviour>().TargetName;
        int modelIndex = GetModelIndexByName(targetName);
        Debug.Log($"Target: {targetName} | Index: {modelIndex}");

        HideAllModelDescExcludingPuzzle();

        if (modelIndex >= 0)
        {
            if (localPuzzleUIManager.IsModelSolved(modelIndex))
            {
                ShowAllModelDescExcludingPuzzle();
                Debug.Log("[Vuforia] Model sudah solved. Menampilkan deskripsi langsung.");
            }
            else
            {
                localPuzzleUIManager.OnItemFound(modelIndex);
                Debug.Log($"[Vuforia] Model '{targetName}' terdeteksi.");
            }
        }
        else
        {
            Debug.LogError($"[Vuforia] Model '{targetName}' tidak dikenali!");
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