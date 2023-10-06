using Game.Settings;
using UnityEngine;


public class FitRatioCamera : MonoBehaviour
{
    public Canvas canvas;
    public SpriteRenderer bg;
    public RectTransform banner;
    private void Awake()
    {

        float bgWidth = bg.bounds.size.x;
        float bgHeight = bg.bounds.size.y;
        float targetOrthoSize1 = Mathf.Max(bgWidth * 0.5f / Camera.main.aspect, bgHeight * 0.5f);




        Camera.main.orthographicSize = targetOrthoSize1;
        //Camera.main.transform.position = new Vector3(0, +(Camera.main.orthographicSize - cameraOriginalSize), -10);
    }
}

