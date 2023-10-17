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
    [HideInInspector] public UnityEvent<Player> OnNoteMissed = new UnityEvent<Player>();
    [HideInInspector] public UnityEvent<Player> OnNoteHit = new UnityEvent<Player>();
    public UnityEvent StopGameEvent = new UnityEvent();
    public AudioSource PrimaryAudioSource;
    public AudioSource SecondaryAudioSource;
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
    public List<Tile> ActiveTiles;
    public float CameraYBound;
    public UnityEvent GameStarted = new UnityEvent();
    protected override void Awake()
    {
        base.Awake();
        PrimaryAudioSource.DOFade(.7f, 0);
        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        SecondaryAudioSource.volume = 0;
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
        float timeFirstNoteReachLine = (Camera.main.orthographicSize + 2.4f - (-Camera.main.orthographicSize + 3.25f)) / LevelDefinition.NoteSpeed;
        float offsetTimeMinus = timeFirstNoteReachLine - LevelDefinition.TimeToFirstNote;
        PrimaryAudioSource.clip = LevelDefinition.MusicClip;
        SecondaryAudioSource.clip = LevelDefinition.MusicClip;
        SecondaryAudioSource.Play();
        PrimaryAudioSource.Play();
        PrimaryAudioSource.pitch = 1;
        StartCoroutine(Delay(Mathf.Abs(offsetTimeMinus)));

    }

    public void ResetLevel()
    {
        foreach (Tile tile in ActiveTiles)
        {
            tile.OnReset();
        }
        SecondaryAudioSource.DOFade(0, 0);
        SecondaryAudioSource.Stop();
        PrimaryAudioSource.DOFade(.7f, 0);
        PrimaryAudioSource.Stop();
        NoteRoot.position = new Vector2(0, Camera.main.orthographicSize + 2.4f);

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
        PrimaryAudioSource.Stop();
        StopGameEvent.Invoke();
    }

    public void PauseGame()
    {
        StopAllCoroutines();
        _currentState = State.Pause;
        float pausedTime = PrimaryAudioSource.time;
        SecondaryAudioSource.time = pausedTime;
        pausedPos = NoteRoot.position;
        SecondaryAudioSource.Pause();
        PrimaryAudioSource.DOFade(0, 0.3f).OnComplete(
            () =>
            {

                PrimaryAudioSource.Pause();
                PrimaryAudioSource.DOFade(.7f, 0);
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
        PrimaryAudioSource.Play();
        PrimaryAudioSource.pitch = -2;
        float elapsedTime = 0;
        while (elapsedTime <= 2)
        {
            elapsedTime += Time.deltaTime * 2;
            NoteRoot.transform.Translate(Vector3.up * LevelDefinition.NoteSpeed * 2 * Time.deltaTime);
            yield return null;
        }
        PrimaryAudioSource.Pause();
        PrimaryAudioSource.mute = true;
        SecondaryAudioSource.time -= elapsedTime;
        NoteRoot.transform.position = pausedPos + Vector3.up * LevelDefinition.NoteSpeed * elapsedTime;
    }

    public void Continue()
    {
        SecondaryAudioSource.DOFade(.7f, .2f);
        SecondaryAudioSource.Play();
        _currentState = State.Playing;
    }
}
