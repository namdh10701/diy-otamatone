using UnityEngine;
using static Game.Craft.CraftStateSequence;

[CreateAssetMenu(menuName = "ScriptableObjects/OtamatoneParts", fileName = "Otamatone Parts")]
public class OtamatoneParts : ScriptableObject
{
    public Part[] Parts;
}

