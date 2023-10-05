using UnityEngine;

public class ScaleWithScreen : MonoBehaviour
{
    void Start()
    {
        float ratio = Screen.height / Screen.width;
        if (ratio > 9f / 16f)
        {
            GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
            GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(900, 410);
        }
    }

    void Update()
    {

    }
}
