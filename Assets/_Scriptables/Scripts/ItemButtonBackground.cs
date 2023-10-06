using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemButtonBackground", fileName = "ItemButtonBackground")]
public class ItemButtonBackground : ScriptableObject
{
    public Sprite Selected;
    public Sprite Deselected;
}
