 using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    public PuzzleManager puzzleManager;

    protected override void OnTrackingFound()
    {
        Debug.Log("Custom OnTrackingFound called");
        base.OnTrackingFound();
        if (puzzleManager != null)
            puzzleManager.ActivatePuzzle();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        // optional: hide pieces
    }
}
