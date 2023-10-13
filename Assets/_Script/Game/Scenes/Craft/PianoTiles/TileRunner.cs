using Core.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TileRunner : Singleton<TileRunner>
{
    public UnityEvent StopGameEvent = new UnityEvent();
    public AudioSource AudioSource;
    private bool levelEditorMode = false;
    public Transform NoteRoot;

    public enum State
    {
        DelayPlay, Playing, Stop
    }

    public LevelDefinition LevelDefinition;

    [SerializeField] private Transform _noteHitLine;

    [SerializeField] private State _currentState = State.Stop;
    public List<Tile> ActiveTiles;
    protected override void Awake()
    {
        base.Awake();
        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        ActiveTiles = FindObjectsOfType<Tile>().ToList();
        List<TrailTile> TrailTiles = FindObjectsOfType<TrailTile>().ToList();
        foreach (TrailTile tile in TrailTiles)
        {
            tile.TrailTopMat = tile.TrailTop.material;
            tile.TrailMat = tile.Trail.material;
            tile.LevelDefinition = this.LevelDefinition;
            tile.TrailMat.SetTexture("_MainTex", tile.Trail.sprite.texture);
            tile.TrailTopMat.SetFloat("_IsActive", 0);
            tile.TrailTopMat.SetFloat("_IsFade", 0);
            tile.TrailMat.SetFloat("_IsFade", 1);
            tile.TrailMat.SetFloat("_IsActive", 0);
        }
    }

    public void StartTheGame()
    {
        _currentState = State.DelayPlay;
        float timeFirstNoteReachLine = (Camera.main.orthographicSize + 2.4f - _noteHitLine.position.y) / LevelDefinition.NoteSpeed;
        float offsetTimeMinus = timeFirstNoteReachLine - LevelDefinition.TimeToFirstNote;
        AudioSource.clip = LevelDefinition.MusicClip;
        AudioSource.Play();
        StartCoroutine(Delay(Mathf.Abs(offsetTimeMinus)));

    }

    public void ResetLevel()
    {
        foreach (Tile tile in ActiveTiles)
        {
            tile.OnReset();
        }
        NoteRoot.position = new Vector2(0, Camera.main.orthographicSize + 2.4f);
        StartTheGame();
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
        StopGameEvent.Invoke();
    }
}
