 using UnityEngine;
using Vuforia;

public class CustomObserverEventHandler : DefaultObserverEventHandler
{
    public PuzzleSpawner puzzleSpawner;

    protected override void OnTrackingFound()
    {
        Debug.Log("Custom OnTrackingFound called");
        base.OnTrackingFound();
        if (puzzleSpawner != null)
            puzzleSpawner.SpawnPuzzle();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        // optional: hide pieces
    }
}
