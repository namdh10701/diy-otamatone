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
    }
}
