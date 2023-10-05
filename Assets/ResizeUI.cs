using UnityEngine;

public class ResizeUI : MonoBehaviour
{
    [SerializeField] private float narrowAspectRatio = 9f / 16f;

    private RectTransform rectTransform;
    private float originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale.x;
    }
    private void Start()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        // Check if the screen aspect ratio is narrower than 6:19
        if (screenAspect < narrowAspectRatio)
        {
            // Resize the RectTransform here
            // You can set new size, scale, or any other relevant property as per your requirement
            // For example, to scale it down uniformly, you can do something like this:
            rectTransform.localScale = new Vector3(originalScale, originalScale, originalScale) * (screenAspect / narrowAspectRatio);
        }
        else
        {
            // Screen aspect ratio is wider, retain the original size or reset it to a default size
            rectTransform.localScale = new Vector3(originalScale, originalScale, originalScale);
        }
    }
}
