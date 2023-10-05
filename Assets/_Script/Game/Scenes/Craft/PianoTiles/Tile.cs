
using UnityEngine;
using static LevelDefinition;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Col;
    public Transform Transform;
    private void Awake()
    {
        Transform = transform;
    }
}