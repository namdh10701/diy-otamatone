using JetBrains.Annotations;
using UnityEngine;

public class RetainHeight : MonoBehaviour
{
    public RectTransform bottom;
    public Canvas canvas;
    public void Apply(float factor)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, transform.parent.GetComponent<RectTransform>().sizeDelta.y * (1 + factor) - (250 * (1 - factor)));
    }
}
