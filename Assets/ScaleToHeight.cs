using UnityEngine;
using UnityEngine.UI;

public class ScaleToHeight : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;

    private RectTransform rectTransform;
    public ScaleToHeight scaleToHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ApplyScale(float factor)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y * (1 + factor * 0.15f));
        scrollBar.value = 1;
    }
}
