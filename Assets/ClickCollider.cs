using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCollider : MonoBehaviour
{
    [SerializeField] private ArrowButton ArrowButton;
    private void OnMouseUp()
    {
        ArrowButton.OnRealease();
    }

    private void OnMouseDown()
    {
        ArrowButton.OnButton();
    }
}
