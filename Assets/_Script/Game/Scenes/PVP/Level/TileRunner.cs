using Core.Singleton;
using DG.Tweening;
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
using static PVPManager;

public class TileRunner : Singleton<TileRunner>
{
    public enum Player
    {
        P1, P2
    }
    [HideInInspector] public UnityEvent StartGameEvent = new UnityEvent();
    [HideInInspector] public UnityEvent<Player> OnNoteMissed = new UnityEvent<Player>();
    [HideInInspector] public UnityEvent<Player> OnNoteHit = new UnityEvent<Player>();
    [HideInInspector] public UnityEvent StopGameEvent = new UnityEvent();
    [HideInInspector] public UnityEvent LastNotePassedEvent = new UnityEvent();

    [SerializeField] AudioSource[] AudioSources;



    private bool levelEditorMode = false;
    public Transform NoteRoot;
    public float cooldownTime = .1f;
    public enum State
    {
        DelayPlay, Playing, Stop, Pause
    }

    public LevelDefinition LevelDefinition;

    [SerializeField] private Transform _noteHitLine;

    [SerializeField] private State _currentState = State.Stop;
    [SerializeField] private State previousState;
    public List<Tile> ActiveTiles;
    public float CameraYBound;
    public UnityEvent GameStarted = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        ActiveTiles = gameObject.GetComponentsInChildren<Tile>().ToList();
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
        CameraYBound = Camera.main.orthographicSize;
        _currentState = State.DelayPlay;
        previousState = _currentState;
        float timeFirstNoteReachLine = (NoteRoot.position.y - (-Camera.main.orthographicSize + 3.3f)) / LevelDefinition.NoteSpeed;
        float offsetTimeMinus = timeFirstNoteReachLine - LevelDefinition.TimeToFirstNote;

        // 2 audiosource being played
        // one produce music in the game, the other tracking the time of the music
        // switch 2 audiosource role when paused -> unpaused
        foreach (AudioSource audioSource in AudioSources)
        {
            audioSource.clip = LevelDefinition.MusicClip;
            audioSource.pitch = 1;
            audioSource.Play();
        }
        AudioSources[1].mute = true;
        StartGameEvent.Invoke();
        StartCoroutine(Delay(Mathf.Abs(offsetTimeMinus)));

    }

    public void ResetLevel()
    {
        StopAllCoroutines();
        foreach (Tile tile in ActiveTiles)
        {
            tile.OnReset();
        }
        foreach (AudioSource audioSource in AudioSources)
        {
            audioSource.Stop();
            audioSource.clip = LevelDefinition.MusicClip;
            audioSource.pitch = 1;
            audioSource.time = 0;

        }
        NoteRoot.position = new Vector2(0, Camera.main.orthographicSize + 2.4f);

    }


    IEnumerator Delay(float time)
    {
        Debug.Log($"Delay time {time}");
        yield return new WaitForSeconds(time);
        StartGame();
    }
    private void StartGame()
    {
        previousState = _currentState;
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
        previousState = _currentState;
        _currentState = State.Stop;
        AudioSources[0].Stop();
        StopGameEvent.Invoke();
    }

    public void PauseGame()
    {
        previousState = _currentState;
        _currentState = State.Pause;
        pausedPos = NoteRoot.position;
        AudioSources[1].Pause();
        AudioSources[0].DOFade(0, 0.3f).SetUpdate(true).OnComplete(
            () =>
            {
                AudioSources[0].Pause();
                AudioSources[0].DOFade(.7f, 0).SetUpdate(true);
            }
            );
    }

    public Vector3 pausedPos;

    public void Reverse3Seconds()
    {
        StartCoroutine(ReverseAudio());
    }

    private IEnumerator ReverseAudio()
    {
        // TODO: clear destroyed notes when reverse

        _currentState = State.Pause;
        AudioSources[0].Play();
        AudioSources[0].pitch = -2;
        float elapsedTime = 0;
        while (elapsedTime <= 2)
        {
            elapsedTime += Time.deltaTime * 2;
            NoteRoot.transform.Translate(Vector3.up * LevelDefinition.NoteSpeed * 2 * Time.deltaTime);
            yield return null;
        }
        AudioSources[0].Pause();
        AudioSources[0].mute = true;
        AudioSources[1].time -= elapsedTime;
        NoteRoot.transform.position = pausedPos + Vector3.up * LevelDefinition.NoteSpeed * elapsedTime;
    }

    public void Continue()
    {
        if (_currentState != State.Pause)
        {
            return;
        }
        _currentState = previousState;
        AudioSources[0].mute = true;
        AudioSources[0].DOFade(.7f, 0);
        AudioSources[0].time = AudioSources[1].time;
        AudioSources[0].Play();
        AudioSources[1].DOFade(.7f, .2f);
        AudioSources[1].Play();
        AudioSources[1].mute = false;



        AudioSource temp = AudioSources[0];
        AudioSources[0] = AudioSources[1];
        AudioSources[1] = temp;
    }
}
