using Core.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TileRunner : Singleton<TileRunner>
{

    private bool levelEditorMode = false;
    public Transform NoteRoot;
    public Vector2 NoteSpawnPos = new Vector2(0, 8);

    public enum State
    {
        DelayPlay, Playing, Stop
    }

    public LevelDefinition LevelDefinition;
    public AudioSource AudioSource;
    public float offsetTime;
    [SerializeField] private Transform _noteHitLine;

    [SerializeField] private State _currentState = State.Stop;
    public List<Tile> ActiveTiles;
    protected override void Awake()
    {
        base.Awake();
        NoteSpawnPos = new Vector2(0, Camera.main.orthographicSize + 2.4f);
        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelEditor")
        {
            ActiveTiles = FindObjectsOfType<Tile>().ToList();
        }

    }

    public void StartTheGame()
    {
        _currentState = State.DelayPlay;
        float timeFirstNoteReachLine = (NoteSpawnPos.y - _noteHitLine.position.y) / LevelDefinition.NoteSpeed;
        float offsetTimeMinus = timeFirstNoteReachLine - LevelDefinition.TimeToFirstNote;
        AudioSource.clip = LevelDefinition.MusicClip;
        AudioSource.Play();
        StartCoroutine(Delay(Mathf.Abs(offsetTimeMinus)));

    }
    IEnumerator Delay(float time)
    {
        Debug.Log($"Delay time {time}");
        yield return new WaitForSecondsRealtime(time);
        StartGame();
    }
    private void StartGame()
    {
        _currentState = State.Playing;
    }
    float elapsedTime = 0;
    public TextMeshProUGUI text;
    void Update()
    {
        switch (_currentState)
        {
            case State.Playing:
                NoteRoot.transform.Translate(Vector3.down * LevelDefinition.NoteSpeed * Time.deltaTime);
                break;
        }
    }
    public void StopGame()
    {
        _currentState = State.Stop;
        AudioSource.Stop();
        ActiveTiles = null;
    }

}
