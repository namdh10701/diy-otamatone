using UnityEngine;
using UnityEngine.UI;

public class ScaleToHeight : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ApplyScale(float factor)
    {
        Debug.Log(1 + factor);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y * (1 + factor*.5f));
        if (scrollBar != null)
        {
            scrollBar.value = 1;
        }
    }
}
