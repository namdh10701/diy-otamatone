using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCollider : MonoBehaviour
{
    [SerializeField] private ArrowButton ArrowButton;
    private void OnMouseUp()
    {
        switch (PVPManager.Instance.CurrentState)
        {
            case PVPManager.State.Playing:
                {
                    ArrowButton.OnRealease();
                }
                break;
        }

    }

    private void OnMouseDown()
    {
        switch (PVPManager.Instance.CurrentState)
        {
            case PVPManager.State.Playing:
                {
                    ArrowButton.OnButton();
                }
                break;
        }
    }
}
