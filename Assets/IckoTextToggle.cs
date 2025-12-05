using UnityEngine;

public class IckoTextToggle : MonoBehaviour
{
    [SerializeField] private GameObject textObject;

    private bool isVisible = false;

    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.touches[0];
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isVisible = !isVisible;
                    textObject.SetActive(isVisible);
                }
            }
        }
    }
}
