using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorToCamera : MonoBehaviour
{
    public enum Side
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Center,
    }

    public Camera uiCamera = null;
    public Side side = Side.Center;
    public Vector3 relativeOffset = Vector3.zero;

    public void Apply()
    {
        transform.position = new Vector3(0, (-Camera.main.orthographicSize + 2.4f), transform.position.z);
    }
}

