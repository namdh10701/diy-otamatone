using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tab : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Graphic[] _graphics;
    [SerializeField] private Color[] _activeColors;
    [SerializeField] private Color[] _inactiveColors;
    [Header("Game Objects")]
    [SerializeField] private GameObject[] _showOnSelected;
    [SerializeField] private GameObject[] _hideOnDeselected;

    public Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Deselect()
    {
        for (int i = 0; i < _graphics.Length; i++)
        {
            _graphics[i].color = _inactiveColors[i];
        }
    }

    public void Select()
    {
        for (int i = 0; i < _graphics.Length; i++)
        {
            _graphics[i].color = _activeColors[i];
        }
    }

    public void AddSelectEvent(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
