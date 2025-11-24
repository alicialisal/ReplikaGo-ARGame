using UnityEngine;
using UnityEngine.EventSystems;

public class ImageBottomSheet : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform sheet; 
    public float collapsedY = -350f;  
    public float expandedY = 0f;      
    public float smoothSpeed = 10f;

    private float targetY;

    void Start()
    {
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
        float newY = sheet.anchoredPosition.y + eventData.delta.y;
        sheet.anchoredPosition = new Vector2(0, Mathf.Clamp(newY, collapsedY, expandedY));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Snap otomatis ke buka atau tutup
        if (sheet.anchoredPosition.y > (collapsedY + expandedY) / 2f)
            targetY = expandedY;
        else
            targetY = collapsedY;
    }

    public void ShowCollapsed() => targetY = collapsedY;
    public void ShowExpanded()  => targetY = expandedY;
}
