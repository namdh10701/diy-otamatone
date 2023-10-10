using static Game.Craft.CraftStateSequence;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Part")]
public class Part : ScriptableObject
{
    public PartID PartID;
    public Sprite[] Sprites;
}