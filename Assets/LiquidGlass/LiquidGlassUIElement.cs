using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class LiquidGlassUIElement : MonoBehaviour
{
    [SerializeField] private float radius = 24f;
    [SerializeField] private Color tint = Color.white;
    [SerializeField] private float blur = 2f;

    private RectTransform rectTransform;
    private int elementIndex = -1;
    private LiquidGlassElement lastSubmitted;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        SubmitElement();
    }

    void LateUpdate()
    {
        SubmitElement();
    }

    void SubmitElement()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        Vector2 radiusOffset = new Vector2(radius, radius);
        min += radiusOffset;
        max -= radiusOffset;

        var element = new LiquidGlassElement
        {
            rect = new float4(min.x, min.y, max.x, max.y),
            radius = radius,
            tint = new float4(tint.r, tint.g, tint.b, tint.a),
            blur = blur,
        };

        if (!element.Equals(lastSubmitted))
        {
            lastSubmitted = element;
            var manager = LiquidGlassElementsManager.GetInstance();
            manager.SubmitElement(ref elementIndex, element);
        }
    }

    void OnDisable()
    {
        if (elementIndex >= 0)
        {
            var mgr = LiquidGlassElementsManager.GetInstance();
            mgr.RemoveElement(elementIndex);   // you may implement this later
            elementIndex = -1;
        }
    }
}
