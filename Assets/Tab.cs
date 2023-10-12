using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tab : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    [Header("Game Objects")]
    [SerializeField] private GameObject[] _showOnSelected;
    [SerializeField] private GameObject[] _hideOnDeselected;

    private Button _button;
    private Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    public void Deselect()
    {
        _image.sprite = _inactiveSprite;
    }

    public void Select()
    {
        _image.sprite = _activeSprite;
    }

    public void AddSelectEvent(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
