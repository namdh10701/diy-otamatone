
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelDefinition;
//ToDo Snap To Grid
//Demo a gameplay
//Auto Play
//Trail Note
[ExecuteAlways]
public class LevelEditor : EditorWindow
{
    private LevelDefinition sourcelevelDefinition;
    [SerializeField] private LevelDefinition loadedLevelDefinition;

    private TileRunner tileRunner;
    private bool levelLoaded;
    private Camera camera;

    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor), false, "Level Editor");
        window.Show();
        window.WindowInit();
    }
    public void WindowInit()
    {
        camera = Camera.main;
        tileRunner = FindFirstObjectByType<TileRunner>();
    }
    private void OnWindowOpen()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        if (Application.isPlaying)
        {
            GUILayout.Label("Exit play mode to continue editing level.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (!scene.name.Equals("LevelEditor"))
        {
            if (GUILayout.Button("Open Level Editor Scene"))
            {
                EditorSceneManager.OpenScene("Assets/_Scenes/LevelEditor.unity");
                WindowInit();
                if (sourcelevelDefinition != null)
                {
                    LoadLevel(sourcelevelDefinition);

                }
            }
            return;
        }
    }
    void OnDestroy()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorSceneManager.sceneSaved -= OnSceneSaved;
    }
    void OnFocus()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;

        EditorSceneManager.sceneSaved -= OnSceneSaved;
        EditorSceneManager.sceneSaved += OnSceneSaved;
    }
    void OnPlayModeChanged(PlayModeStateChange state)
    {
        if ((state == PlayModeStateChange.EnteredEditMode) && sourcelevelDefinition != null)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.name.Equals("LevelEditor"))
            {
                Debug.Log("Loaded here");
                LoadLevel(sourcelevelDefinition);
            }
        }
        if (state == PlayModeStateChange.ExitingEditMode && sourcelevelDefinition != null && loadedLevelDefinition != null && levelLoaded)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
        }
    }

    void OnSceneSaved(Scene scene)
    {
        if (sourcelevelDefinition != null && loadedLevelDefinition != null && levelLoaded)
        {
            if (scene.name.Equals("LevelEditor"))
            {
                SaveLevel(loadedLevelDefinition);
            }
        }
    }
    string msg = "";
    private void OnGUI()
    {
        GUILayout.Label("Level Definition", EditorStyles.boldLabel);

        sourcelevelDefinition = (LevelDefinition)EditorGUILayout.ObjectField("Level Definition", sourcelevelDefinition, typeof(LevelDefinition), false, null);
        if (GUILayout.Button("Load Level"))
        {
            LoadLevel(sourcelevelDefinition);
            msg = $"Loaded level {sourcelevelDefinition.name} \n{sourcelevelDefinition.Spawnables.Length} Notes";
        }
        GUILayout.Label(msg);
        if (sourcelevelDefinition == null || loadedLevelDefinition == null)
        {
            GUILayout.Label("Select a LevelDefinition ScriptableObject to begin.");
            return;
        }

        loadedLevelDefinition.LevelName = EditorGUILayout.TextField("Level Name", loadedLevelDefinition.LevelName);
        loadedLevelDefinition.MusicClip = (AudioClip)EditorGUILayout.ObjectField("Music", loadedLevelDefinition.MusicClip, typeof(AudioClip), false);
        loadedLevelDefinition.TrailNote = (TrailTile)EditorGUILayout.ObjectField("Trail Note", loadedLevelDefinition.TrailNote, typeof(TrailTile), false);
        loadedLevelDefinition.DownNote = (Tile)EditorGUILayout.ObjectField("Down Note", loadedLevelDefinition.DownNote, typeof(Tile), false);
        loadedLevelDefinition.Bpm = EditorGUILayout.FloatField("Bpm", loadedLevelDefinition.Bpm);
        loadedLevelDefinition.TimeToFirstNote = EditorGUILayout.FloatField("Time To First Note", loadedLevelDefinition.TimeToFirstNote);
        loadedLevelDefinition.TimeFromLastNote = EditorGUILayout.FloatField("Time From Last Note", loadedLevelDefinition.TimeFromLastNote);
        loadedLevelDefinition.GridHeight = EditorGUILayout.FloatField("Grid Height", loadedLevelDefinition.GridHeight);
        loadedLevelDefinition.NoteSpeed = EditorGUILayout.FloatField("Note Speed", loadedLevelDefinition.NoteSpeed);
        if (loadedLevelDefinition == null)
            return;

        if (GUILayout.Button("Re-spawn Level Notes"))
        {
            ClearNotes();
            SpawnLevelNotes();
        }
        Tile a = FindFirstObjectByType<Tile>();
        if (a == null)
        {
            if (GUILayout.Button("Spawn All Notes"))
            {
                msg = "";
                SpawnAllNotes();
            }
        }
        if (GUILayout.Button("Clear Notes"))
        {
            msg = "";
            ClearNotes();
        }
        if (GUILayout.Button("Save Level"))
        {
            SaveLevel(loadedLevelDefinition);
        }
        if (GUILayout.Button("Prepare To Prefab"))
        {
            PrepareToPrefab();
        }
    }

    private void PrepareToPrefab()
    {
        Tile[] spawnables = (Tile[])FindObjectsOfType(typeof(Tile));
        foreach (Tile tile in spawnables)
        {
            tile.IsEditMode = false;
        }
    }

    private void SaveLevel(LevelDefinition levelDefinition)
    {
        Tile[] spawnables = (Tile[])FindObjectsOfType(typeof(Tile));
        levelDefinition.Spawnables = new SpawnableObject[spawnables.Length];
        for (int i = 0; i < spawnables.Length; i++)
        {
            try
            {
                if (spawnables[i].Type == Tile.NoteType.Trail)
                {
                    float TrailHeight = ((TrailTile)spawnables[i]).TrailHeight;
                    Debug.Log(TrailHeight);
                    levelDefinition.Spawnables[i] = new SpawnableObject()
                    {
                        Type = (int)spawnables[i].Type,
                        TrailHeight = TrailHeight,
                        Row = spawnables[i].Row,
                        Col = spawnables[i].Col
                    };
                }
                else
                {
                    levelDefinition.Spawnables[i] = new SpawnableObject()
                    {
                        Type = (int)spawnables[i].Type,
                        TrailHeight = 0,
                        Row = spawnables[i].Row,
                        Col = spawnables[i].Col,
                        IsP2Turn = spawnables[i].IsP2Turn
                    };
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        sourcelevelDefinition.SaveValue(levelDefinition);
        EditorUtility.SetDirty(sourcelevelDefinition);
        AssetDatabase.SaveAssets();
        Debug.Log("Level saved");
    }

    private void SpawnLevelNotes()
    {
        float x = loadedLevelDefinition.Bpm * (loadedLevelDefinition.MusicClip.length / 60);
        loadedLevelDefinition.Row = Mathf.CeilToInt(x);
        float result = (x - 1) * loadedLevelDefinition.GridHeight / loadedLevelDefinition.MusicClip.length;
        Debug.LogWarning($"Suggest Note Speed: " + result);
        loadedLevelDefinition.Spawnables = sourcelevelDefinition.Spawnables;
        foreach (SpawnableObject spawnableObject in loadedLevelDefinition.Spawnables)
        {
            SpawnNote2(spawnableObject.Col, spawnableObject.Row, spawnableObject.Type, spawnableObject.TrailHeight,
                spawnableObject.IsP2Turn);
        }
    }

    private void ClearNotes()
    {
        Tile[] spawnableObjects = FindObjectsOfType<Tile>().ToArray();
        foreach (Tile o in spawnableObjects)
        {

            DestroyImmediate(o.gameObject);
        }
        loadedLevelDefinition.Spawnables = null;
    }


    private void LoadLevel(LevelDefinition levelDefinition)
    {
        Debug.Log("Level loaded");
        WindowInit();
        loadedLevelDefinition = Instantiate(levelDefinition);
        ClearNotes();
        loadedLevelDefinition.LevelName = levelDefinition.LevelName;
        loadedLevelDefinition.MusicClip = levelDefinition.MusicClip;
        loadedLevelDefinition.TimeToFirstNote = levelDefinition.TimeToFirstNote;
        loadedLevelDefinition.TimeFromLastNote = levelDefinition.TimeFromLastNote;
        loadedLevelDefinition.Bpm = levelDefinition.Bpm;
        loadedLevelDefinition.GridHeight = levelDefinition.GridHeight;
        loadedLevelDefinition.Spawnables = levelDefinition.Spawnables;
        loadedLevelDefinition.DownNote = levelDefinition.DownNote;
        loadedLevelDefinition.GridWidth = 1.8f;
        levelLoaded = true;
        tileRunner.LevelDefinition = loadedLevelDefinition;
        SpawnLevelNotes();

        //TODO Handle Spawnable here
    }
    private void SpawnAllNotes()
    {
        float audioTimeHasBeat = (loadedLevelDefinition.MusicClip.length - loadedLevelDefinition.TimeToFirstNote - loadedLevelDefinition.TimeFromLastNote);
        float x = loadedLevelDefinition.Bpm * (loadedLevelDefinition.MusicClip.length / 60);
        loadedLevelDefinition.Row = Mathf.CeilToInt(x);
        float result = (x - 1) * loadedLevelDefinition.GridHeight / loadedLevelDefinition.MusicClip.length;
        Debug.LogWarning($"Suggest Note Speed: " + result);
        for (int y = 0; y < loadedLevelDefinition.Row; y++)
        {
            SpawnNote(0, y);
            //SpawnNote(0, y);
        }

    }

    private void SpawnNote(int col, int row)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 NoteSpawnPos = new Vector2(0, Camera.main.orthographicSize + 2.4f);
        Vector3 gridCenter = new Vector3(0,
            0,
            0);
        tileRunner.NoteRoot.transform.position = NoteSpawnPos;
        Vector3 spawnPosition = new Vector3(
        tileRunner.NoteRoot.transform.position.x + col * loadedLevelDefinition.GridWidth - .5f - 1.2f - 1,
        tileRunner.NoteRoot.transform.position.y + row * loadedLevelDefinition.GridHeight,
        0
    );

        /*        row = Mathf.FloorToInt((spawnPosition.y) / (loadedLevelDefinition.GridWidth / 4));*/
        Tile notePrefab = loadedLevelDefinition.DownNote;

        Tile tile = (Tile)PrefabUtility.InstantiatePrefab(notePrefab, tileRunner.NoteRoot);
        tile.IsSnapToGrid = false;
        tile.LevelDefinition = loadedLevelDefinition;

        tile.Col = col;
        tile.transform.position = spawnPosition;

        tile.Row = (int)(Mathf.Round(tile.transform.localPosition.y) / (loadedLevelDefinition.GridHeight / 4));
        tile.name = $"Tile C:{col} R:{tile.Row}";

        //tile.Transform.hasChanged = false;
        tile.IsSnapToGrid = true;
    }

    private void SpawnNote2(int col, int row, int type, float trailHeight, bool isP2Turn)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 NoteSpawnPos = new Vector2(0, Camera.main.orthographicSize + 2.4f);
        Vector3 gridCenter = new Vector3(0,
            0,
            0);
        tileRunner.NoteRoot.transform.position = NoteSpawnPos;
        Vector3 spawnPosition = new Vector3(
        tileRunner.NoteRoot.transform.position.x + col * loadedLevelDefinition.GridWidth - .5f - 1.2f - 1,
        tileRunner.NoteRoot.transform.position.y + row * (loadedLevelDefinition.GridHeight / 4),
        0
    );
        Tile notePrefab;
        if (type == 0)
        {
            notePrefab = loadedLevelDefinition.DownNote;
        }
        else
        {
            notePrefab = loadedLevelDefinition.TrailNote;
            notePrefab.GetComponent<TrailTile>().SetTrailHeight(trailHeight * loadedLevelDefinition.GridHeight / 4);
        }
        /*        row = Mathf.FloorToInt((spawnPosition.y) / (loadedLevelDefinition.GridWidth / 4));*/


        Tile tile = (Tile)PrefabUtility.InstantiatePrefab(notePrefab, tileRunner.NoteRoot);
        tile.IsSnapToGrid = false;
        tile.LevelDefinition = loadedLevelDefinition;
        tile.IsP2Turn = isP2Turn;
        tile.Col = col;
        tile.transform.position = spawnPosition;

        tile.Row = (int)(Mathf.Round(tile.transform.localPosition.y) / (loadedLevelDefinition.GridHeight / 4));
        tile.name = $"Tile C:{col} R:{tile.Row}";

        tile.Transform.hasChanged = true;
        tile.IsSnapToGrid = true;
    }
}

