using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Definition", fileName = "New Level Definition")]
public class LevelDefinition : ScriptableObject
{
    public string LevelName;
    public AudioClip MusicClip;
    public float TimeToFirstNote;
    public float TimeFromLastNote;
    public float Bpm;
    public float GridHeight;
    public float GridWidth = 1.8f;
    public float Row;
    public float NoteSpeed;
    public Tile DownNote;
    public TrailTile TrailNote;
    public SpawnableObject[] Spawnables;

    public void SaveValue(LevelDefinition levelDefinition)
    {
        name = levelDefinition.LevelName;
        LevelName = levelDefinition.LevelName;
        MusicClip = levelDefinition.MusicClip;
        TimeFromLastNote = levelDefinition.TimeFromLastNote;
        TimeToFirstNote = levelDefinition.TimeToFirstNote;
        Bpm = levelDefinition.Bpm;
        GridHeight = levelDefinition.GridHeight;
        GridHeight = levelDefinition.GridHeight;
        Spawnables = levelDefinition.Spawnables;
        DownNote = levelDefinition.DownNote;
        NoteSpeed = levelDefinition.NoteSpeed;
        TrailNote = levelDefinition.TrailNote;
    }

    [System.Serializable]
    public class SpawnableObject
    {
        public int Type;
        public float TrailHeight;
        public int Row;
        public int Col;
        public bool IsP2Turn;
    }
}
