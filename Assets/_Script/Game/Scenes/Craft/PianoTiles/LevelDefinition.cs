using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Definition", fileName = "New Level Definition")]
public class LevelDefinition : ScriptableObject
{
    public string LevelName;
    public AudioClip MusicClip;
    public float StartOffset;
    public float EndOffset;
    public float Bpm;
    public float GridHeight;
    public float GridWidth;
    public float Row;
    public Tile NotePrefab;
    public SpawnableObject[] Spawnables;

    public void SaveValue(LevelDefinition levelDefinition)
    {
        LevelName = levelDefinition.LevelName;
        MusicClip = levelDefinition.MusicClip;
        StartOffset = levelDefinition.StartOffset;
        EndOffset = levelDefinition.EndOffset;
        Bpm = levelDefinition.Bpm;
        GridHeight = levelDefinition.GridHeight;
        GridHeight = levelDefinition.GridHeight;
        Spawnables = levelDefinition.Spawnables;
        NotePrefab = levelDefinition.NotePrefab;
    }

    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject SpawnablePrefab;
        public int Row;
        public int Col;
    }
}
