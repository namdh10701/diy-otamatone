
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelDefinition;

public class LevelEditor : EditorWindow
{
    //ToDo Snap To Grid
    //Display note number on note visual
    //
    //Demo a gameplay
    //Auto Play
    //Trail Note

    private LevelDefinition sourcelevelDefinition;
    private LevelDefinition loadedLevelDefinition;

    private Canvas canvas;
    private float canvasUnitHeight;
    private bool levelLoaded;

    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor), false, "Level Editor");
        window.Show();
        window.WindowInit();
    }
    public void WindowInit()
    {
        canvas = FindAnyObjectByType<Canvas>();
        Rect rect = canvas.GetComponent<RectTransform>().rect;
        canvasUnitHeight = rect.height / 10;

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
        if ((state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode) && sourcelevelDefinition != null)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.name.Equals("LevelEditor"))
            {
                LoadLevel(sourcelevelDefinition);
            }
        }
        else if (state == PlayModeStateChange.ExitingEditMode && sourcelevelDefinition != null && loadedLevelDefinition != null && !levelLoaded)
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
        loadedLevelDefinition.NotePrefab = (Tile)EditorGUILayout.ObjectField("Note Prefab", loadedLevelDefinition.NotePrefab, typeof(SpawnableObject), false);
        loadedLevelDefinition.Bpm = EditorGUILayout.FloatField("Bpm", loadedLevelDefinition.Bpm);
        loadedLevelDefinition.StartOffset = EditorGUILayout.FloatField("Start Offset", loadedLevelDefinition.StartOffset);
        loadedLevelDefinition.EndOffset = EditorGUILayout.FloatField("End Offset", loadedLevelDefinition.EndOffset);
        loadedLevelDefinition.GridHeight = EditorGUILayout.FloatField("Grid height", loadedLevelDefinition.GridHeight);
        loadedLevelDefinition.GridWidth = canvas.GetComponent<RectTransform>().rect.width / 4;
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
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < loadedLevelDefinition.Row; y++)
            {
                SpawnNote(x, y);
            }
        }
    }

    private void SpawnNote(int col, int row)
    {
        Vector3 spawnPosition = new Vector3(col * loadedLevelDefinition.GridWidth - (loadedLevelDefinition.GridWidth * 1.5f),
                    row * loadedLevelDefinition.GridHeight * canvasUnitHeight, 0);
        Tile tile = (Tile)PrefabUtility.InstantiatePrefab(loadedLevelDefinition.NotePrefab, canvas.transform);
        tile.name = $"Tile C: {col} R:{row}";
        tile.Row = row;
        tile.Col = col;
        RectTransform rectTransform = tile.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = spawnPosition;
        }
    }

}

