using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    [SerializeField] private PuzzleUIManager puzzleUIManager;

    protected override void OnTrackingFound()
    {
        Debug.Log("Custom OnTrackingFound called");
        base.OnTrackingFound();

        if (puzzleUIManager != null)
        {
            string targetName = GetComponent<ObserverBehaviour>().TargetName;
            int modelIndex = GetModelIndexByName(targetName);
            
            if (modelIndex >= 0)
            {
                puzzleUIManager.OnItemFound(modelIndex);
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