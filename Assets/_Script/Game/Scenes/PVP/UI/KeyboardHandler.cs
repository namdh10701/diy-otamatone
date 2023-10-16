using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{

    [SerializeField] private ArrowButton left;
    [SerializeField] private ArrowButton right;
    [SerializeField] private ArrowButton up;
    [SerializeField] private ArrowButton down;

    // Update is called once per frame
    void Update()
    {
        if (PVPManager.Instance.CurrentState == PVPManager.State.Playing)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                up.OnButton();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                left.OnButton();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                down.OnButton();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                right.OnButton();
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                up.OnRealease();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                left.OnRealease();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                down.OnRealease();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                right.OnRealease();
            }
        }
    }
}
