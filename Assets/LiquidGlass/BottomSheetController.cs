using UnityEngine;
using UnityEngine.EventSystems;

public class BottomSheetController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform sheet;

    public float collapsedY = -1348f;
    public float expandedY = 0f;
    public float smoothSpeed = 20f;
    public float dragThreshold = 15f; // geser sedikit saja langsung naik
    private float startDragY;

    // private float collapsedY;
    private float targetY;

    void Start()
    {
        // Hitung posisi collapse berdasar tinggi sheet
        targetY = collapsedY;
        sheet.anchoredPosition = new Vector2(0, collapsedY);
    }

    void Update()
    {
        Vector2 pos = sheet.anchoredPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * smoothSpeed);
        sheet.anchoredPosition = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // float drag = eventData.delta.y * 1.8f; // buat sensitif
        // float newY = sheet.anchoredPosition.y + drag;

        // sheet.anchoredPosition = new Vector2(
        //     0,
        //     Mathf.Clamp(newY, collapsedY, expandedY)
        // );

        if (eventData.delta.y > 0)
        {
            // drag ke atas → langsung expand
            if (Mathf.Abs(eventData.delta.y) > dragThreshold)
                targetY = expandedY;
        }
        else
        {
            // drag ke bawah → collapse
            if (Mathf.Abs(eventData.delta.y) > dragThreshold)
                targetY = collapsedY;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // float mid = (collapsedY + expandedY) / 2f;

        // if (sheet.anchoredPosition.y > mid)
        //     targetY = expandedY;
        // else
        //     targetY = collapsedY;

         // Fallback jika tidak kena threshold (optional)
        float mid = (collapsedY + expandedY) / 2f;
        targetY = (sheet.anchoredPosition.y > mid) ? expandedY : collapsedY;
    }

    public void ShowCollapsed() => targetY = collapsedY;
    public void ShowExpanded()  => targetY = expandedY;
}
