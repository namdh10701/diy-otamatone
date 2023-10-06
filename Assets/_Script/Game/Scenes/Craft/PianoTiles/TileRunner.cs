using Core.Singleton;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TileRunner : Singleton<TileRunner>
{
    public enum State
    {
        Playing, Stop
    }
    private State _currentState;
    [SerializeField] private float _tileSpeed;
    private LevelDefinition currentLevel;
    private Tile[] _activeTiles;
    [SerializeField] private Transform _noteHitLine;
    private bool levelEditorMode = false;
    public Transform NoteRoot;

    protected override void Awake()
    {
        base.Awake();

        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            Debug.Log("Awake");
            levelEditorMode = true;
            _activeTiles = FindObjectsOfType<Tile>().ToArray();
            StartGame();
        }
    }
    public void LoadLevel(LevelDefinition levelDefinition)
    {
        currentLevel = levelDefinition;
        //tiles =
        StartGame();
    }

    private void StartGame()
    {
        ResetLevel();
    }

    private void ResetLevel()
    {
        _currentState = State.Playing;
    }
    private void Update()
    {
        switch (_currentState)
        {
            case State.Playing:
                for (int i = 0; i < _activeTiles.Length; i++)
                {
                    _activeTiles[i].transform.Translate(Vector3.down * _tileSpeed * Time.deltaTime);
                }
                break;
        }
    }


    public static void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
    {
        if (levelDefinition == null)
        {
            Debug.LogError("Invalid Level!");
            return;
        }

        if (levelGameObject != null)
        {
            if (Application.isPlaying)
            {
                Destroy(levelGameObject);
            }
            else
            {
                DestroyImmediate(levelGameObject);
            }
        }

        /* levelGameObject = new GameObject("LevelManager");
         LevelManager levelManager = levelGameObject.AddComponent<LevelManager>();
         levelManager.LevelDefinition = levelDefinition;
         Transform levelParent = levelGameObject.transform;

         for (int i = 0; i < levelDefinition.Spawnables.Length; i++)
         {
             LevelDefinition.SpawnableObject spawnableObject = levelDefinition.Spawnables[i];

             if (spawnableObject.SpawnablePrefab == null)
             {
                 continue;
             }

             Vector3 position = spawnableObject.Position;
             Vector3 eulerAngles = spawnableObject.EulerAngles;
             Vector3 scale = spawnableObject.Scale;

             GameObject go = null;

             if (Application.isPlaying)
             {
                 go = GameObject.Instantiate(spawnableObject.SpawnablePrefab, position, Quaternion.Euler(eulerAngles));
             }
             else
             {
 #if UNITY_EDITOR
                 go = (GameObject)PrefabUtility.InstantiatePrefab(spawnableObject.SpawnablePrefab);
                 go.transform.position = position;
                 go.transform.eulerAngles = eulerAngles;
 #endif
             }

             if (go == null)
             {
                 return;
             }

             // Set Base Color
             Spawnable spawnable = go.GetComponent<Spawnable>();
             if (spawnable != null)
             {
                 spawnable.SetBaseColor(spawnableObject.BaseColor);
                 spawnable.SetScale(scale);
                 levelManager.AddSpawnable(spawnable);
             }

             if (go != null)
             {
                 go.transform.SetParent(levelParent);
             }
         }*/
    }

}
