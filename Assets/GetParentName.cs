using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class GetParentName : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private GameObject parent;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnGUI()
    {
        _text.text = parent.name;
    }
}
