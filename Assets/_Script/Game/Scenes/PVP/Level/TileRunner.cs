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

public class TileRunner : Singleton<TileRunner>
{
    public UnityEvent StopGameEvent = new UnityEvent();
    public AudioSource AudioSource;
    public AudioSource ReverseAudioSource;
    private bool levelEditorMode = false;
    public Transform NoteRoot;
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
        base.Awake(); AudioSource.DOFade(.7f, 0);
        //LoadLevel(LevelSelectionScreen.current);
    }

    private void Start()
    {
        ReverseAudioSource.volume = 0;
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
        float timeFirstNoteReachLine = (Camera.main.orthographicSize + 2.4f - _noteHitLine.position.y) / LevelDefinition.NoteSpeed;
        float offsetTimeMinus = timeFirstNoteReachLine - LevelDefinition.TimeToFirstNote;
        AudioSource.clip = LevelDefinition.MusicClip;
        ReverseAudioSource.clip = LevelDefinition.MusicClip;
        ReverseAudioSource.Play();
        AudioSource.Play();
        StartCoroutine(Delay(Mathf.Abs(offsetTimeMinus)));

    }

    public void ResetLevel()
    {
        foreach (Tile tile in ActiveTiles)
        {
            tile.OnReset();
        }
        AudioSource.DOFade(.7f, 0);
        AudioSource.Stop();
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
        AudioSource.Stop();
        StopGameEvent.Invoke();
    }

    public float pausedTimeee = 0;
    public void PauseGame()
    {
        StopAllCoroutines();
        _currentState = State.Pause;
        pausedTimeee = AudioSource.time;
        pausedPos = NoteRoot.position;
        AudioSource.DOFade(0, 0.3f).OnComplete(
            () =>
            {

                ReverseAudioSource.Pause();
                AudioSource.Pause();
                AudioSource.DOFade(.7f, 0);
            }
            );
    }

    public Vector3 pausedPos;
    public float pausedTime;

    public void Reverse3Seconds()
    {
        StartCoroutine(ReverseAudio());
    }

    private IEnumerator ReverseAudio()
    {
        _currentState = State.Pause;
        ReverseAudioSource.volume = .7f;
        ReverseAudioSource.Play();
        ReverseAudioSource.pitch = -2;
        Vector3 currentPos = new Vector3(NoteRoot.transform.position.x, NoteRoot.transform.position.y, NoteRoot.transform.position.z);
        float elapsedTime = 0;
       while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime;
            NoteRoot.transform.Translate(Vector3.up * LevelDefinition.NoteSpeed * 2 * Time.deltaTime);
            yield return null;
        }
        ReverseAudioSource.Pause();
        ReverseAudioSource.pitch = 1;

        NoteRoot.transform.position = currentPos + Vector3.up * LevelDefinition.NoteSpeed * 2;
        AudioSource.time -= 2.3f;
    }

    public void Continue()
    {
        AudioSource.Play();
        _currentState = State.Playing;
    }
}
