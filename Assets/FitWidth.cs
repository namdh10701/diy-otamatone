using UnityEngine;

public class FitWidth : MonoBehaviour
{
    // Reference to the RectTransform of the Canvas
    private RectTransform canvasRectTransform;

    // The original width of your animation
    public float originalWidth; // Adjust this to match your animation's original width
    public float originalHeight;
    public bool fitHeight = true;
    private void Start()
    {
        // Get the RectTransform of the Canvas
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Call the ResizeAnimation method
        ResizeAnimation();
    }

    private void ResizeAnimation()
    {
        if (canvasRectTransform == null)
            return;

        // Calculate the scaling factor based on the screen width
        float scaleFactor = canvasRectTransform.rect.width / originalWidth;

        float scaleFactorY;
        if (fitHeight)
            scaleFactorY = canvasRectTransform.rect.height / originalHeight;
        else
            scaleFactorY = transform.localScale.y;
        transform.localScale = new Vector3(scaleFactor, scaleFactorY, 1f);
    }
}
