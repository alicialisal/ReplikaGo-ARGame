using UnityEngine;

public class RotatePerPiece : MonoBehaviour
{
    public float rotateSpeed = 0.2f;
    private Vector2 lastPos;
    private bool rotating = false;

    void Update()
    {
        if (Input.touchCount == 0)
        {
            rotating = false;
            return;
        }

        Touch t = Input.GetTouch(0);

        // Raycast ke objek supaya hanya yang disentuh yang diputar
        if (t.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    rotating = true;
                    lastPos = t.position;
                }
            }
        }
        else if (t.phase == TouchPhase.Moved && rotating)
        {
            Vector2 delta = t.position - lastPos;

            float rotY = -delta.x * rotateSpeed;
            float rotX = delta.y * rotateSpeed;

            transform.Rotate(Vector3.up, rotY, Space.World);
            transform.Rotate(Vector3.right, rotX, Space.World);

            lastPos = t.position;
        }
        else if (t.phase == TouchPhase.Ended)
        {
            rotating = false;
        }
    }
}
