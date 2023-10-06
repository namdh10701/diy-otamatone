
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
    private LevelDefinition loadedLevelDefinition;

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
        /*if ((state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode) && sourcelevelDefinition != null)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.name.Equals("LevelEditor"))
            {
                LoadLevel(sourcelevelDefinition);
            }
        }
        else*/
        if (state == PlayModeStateChange.ExitingEditMode && sourcelevelDefinition != null && loadedLevelDefinition != null && !levelLoaded)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.name.Equals("LevelEditor"))
            {

                SaveLevel(loadedLevelDefinition);
            }
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
        OnWindowOpen();
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
        loadedLevelDefinition.NotePrefab = (Tile)EditorGUILayout.ObjectField("Note Prefab", loadedLevelDefinition.NotePrefab, typeof(Tile), false);
        loadedLevelDefinition.Bpm = EditorGUILayout.FloatField("Bpm", loadedLevelDefinition.Bpm);
        loadedLevelDefinition.StartOffset = EditorGUILayout.FloatField("Start Offset", loadedLevelDefinition.StartOffset);
        loadedLevelDefinition.EndOffset = EditorGUILayout.FloatField("End Offset", loadedLevelDefinition.EndOffset);
        loadedLevelDefinition.GridHeight = EditorGUILayout.FloatField("Grid height", loadedLevelDefinition.GridHeight);
        loadedLevelDefinition.GridWidth = EditorGUILayout.FloatField("Grid width", loadedLevelDefinition.GridWidth);
        if (loadedLevelDefinition == null)
            return;

        if (GUILayout.Button("Re-spawn Level Notes"))
        {
            ClearNotes();
            SpawnLevelNotes();
        }

        if (GUILayout.Button("Spawn All Notes"))
        {
            msg = "";
            SpawnAllNotes();
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
        DrawGrid();
    }

    private void SaveLevel(LevelDefinition levelDefinition)
    {
        Tile[] spawnables = (Tile[])FindObjectsOfType(typeof(Tile));
        levelDefinition.Spawnables = new SpawnableObject[spawnables.Length];
        for (int i = 0; i < spawnables.Length; i++)
        {
            try
            {
                levelDefinition.Spawnables[i] = new SpawnableObject()
                {
                    SpawnablePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(spawnables[i].gameObject),
                    Row = spawnables[i].Row,
                    Col = spawnables[i].Col
                };
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
        loadedLevelDefinition.Spawnables = sourcelevelDefinition.Spawnables;
        foreach (SpawnableObject spawnableObject in loadedLevelDefinition.Spawnables)
        {
            SpawnNote(spawnableObject.Col, spawnableObject.Row);
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
        WindowInit();
        loadedLevelDefinition = Instantiate(levelDefinition);
        ClearNotes();
        loadedLevelDefinition.LevelName = levelDefinition.LevelName;
        loadedLevelDefinition.MusicClip = levelDefinition.MusicClip;
        loadedLevelDefinition.StartOffset = levelDefinition.StartOffset;
        loadedLevelDefinition.EndOffset = levelDefinition.EndOffset;
        loadedLevelDefinition.Bpm = levelDefinition.Bpm;
        loadedLevelDefinition.GridHeight = levelDefinition.GridHeight;
        loadedLevelDefinition.Spawnables = levelDefinition.Spawnables;
        loadedLevelDefinition.NotePrefab = levelDefinition.NotePrefab;
        loadedLevelDefinition.GridWidth = levelDefinition.GridWidth;
        levelLoaded = true;

        SpawnLevelNotes();

        //TODO Handle Spawnable here
    }
    private void SpawnAllNotes()
    {
        if (loadedLevelDefinition.NotePrefab == null)
        {
            Debug.LogError("Prefab not assigned.");
            return;
        }

        loadedLevelDefinition.Row = Mathf.CeilToInt(loadedLevelDefinition.Bpm * (loadedLevelDefinition.MusicClip.length - loadedLevelDefinition.StartOffset - loadedLevelDefinition.EndOffset) / 60);
        Debug.Log($"Number of rows: {loadedLevelDefinition.Row}");

        for (int y = 0; y < loadedLevelDefinition.Row; y++)
        {
            SpawnNote(UnityEngine.Random.Range(0, 4), y);
        }
    }

    private void SpawnNote(int col, int row)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 gridCenter = new Vector3(
            cameraPosition.x,
            cameraPosition.y,
            0
        );
        Vector3 spawnPosition = new Vector3(
        gridCenter.x + col * loadedLevelDefinition.GridWidth - loadedLevelDefinition.GridWidth * 1.5f,
        gridCenter.y + row * loadedLevelDefinition.GridHeight,
        0
    );
        Tile tile = (Tile)PrefabUtility.InstantiatePrefab(loadedLevelDefinition.NotePrefab, tileRunner.NoteRoot);
        tile.LevelDefinition = loadedLevelDefinition;
        tile.name = $"Tile C: {col} R:{row}";
        tile.Text.text = $"C: {col} R:{row}";
        tile.Row = row;
        tile.Col = col;
        tile.transform.position = spawnPosition;
        tile.SpawnPosition = tile.transform.position;
    }
    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        if (loadedLevelDefinition == null || camera == null)
            return;

        Handles.color = Color.gray;

        float halfWidth = loadedLevelDefinition.GridWidth * 0.5f;
        float halfHeight = loadedLevelDefinition.GridHeight * 0.5f;

        // Draw vertical lines
        for (float x = camera.transform.position.x - halfWidth;
             x <= camera.transform.position.x + halfWidth;
             x += loadedLevelDefinition.GridWidth)
        {
            Handles.DrawLine(new Vector3(x, camera.transform.position.y - halfHeight),
                             new Vector3(x, camera.transform.position.y + halfHeight));
        }

        // Draw horizontal lines
        for (float y = camera.transform.position.y - halfHeight;
             y <= camera.transform.position.y + halfHeight;
             y += loadedLevelDefinition.GridHeight)
        {
            Handles.DrawLine(new Vector3(camera.transform.position.x - halfWidth, y),
                             new Vector3(camera.transform.position.x + halfWidth, y));
        }
    }
}

