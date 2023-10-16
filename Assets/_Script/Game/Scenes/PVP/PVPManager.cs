using Core.Singleton;
using DG.Tweening;
using Game;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PVPManager : Singleton<PVPManager>
{
    public enum Player
    {
        P1, P2
    }

    public enum State
    {
        Pause, Playing, Stop
    }

    public State CurrentState;
    public SongDefinition[] songDefinitions;
    public SongDefinition SelectedSongDefinition;

    public DancingMonster[] dancingMonsterPrefabs;



    [SerializeField] private Game.Timer timer;
    [SerializeField] private PVPSceneUIController _ui;

    [HideInInspector] public UnityEvent<PVPManager.Player> OnNoteMissed = new UnityEvent<PVPManager.Player>();
    [HideInInspector] public UnityEvent<PVPManager.Player> OnNoteHit = new UnityEvent<Player>();
    private GameObject levelGO;

    private int reviveCount = 0;

    public RevivePanel reviePanel;

    private void Start()
    {
        CurrentState = State.Stop;
        SelectedSongDefinition = SelectRandomSong();
        _ui.UpdateSongTitle(SelectedSongDefinition.SongName);
        InstantiateLevel();
        TileRunner.Instance.StopGameEvent.RemoveListener(() => OnGameStop());
        TileRunner.Instance.StopGameEvent.AddListener(() => OnGameStop());
        HpManager.Instance.OnZeroHp.AddListener(() => OnDeath());
    }
    private SongDefinition SelectRandomSong()
    {
        SongDefinition songDef = Instantiate(songDefinitions[UnityEngine.Random.Range(0, songDefinitions.Length)]);
        return songDef;
    }
    public void InstantiateLevel()
    {
        levelGO = Instantiate(SelectedSongDefinition.HardLevel);
        TileRunner.Instance = levelGO.GetComponent<TileRunner>();
    }
    public void ResetLevel()
    {
        if (reviveCount == 1)
        {
            reviveCount = 0;
        }
        HpManager.Instance.ResetHp();
        ScoreManager.Instance.ResetScore();
        Timer.Instance.ResetTimer();
        _ui.ResetUI();
        TileRunner.Instance.ResetLevel();
    }

    public void ResetGameForNewMatch()
    {
        Destroy(levelGO);
        TileRunner.Instance = null;
        SelectRandomSong();
        InstantiateLevel();
    }



    public void OnGameStop()
    {
        CurrentState = State.Stop;
    }

    public void DisplayEndgameUI()
    {
        _ui.DisplayEndgameUI();
    }

    public void OnDeath()
    {
        CurrentState = State.Pause;
        TileRunner.Instance.PauseGame();
        if (reviveCount < 1)
        {
            reviveCount++;
            reviePanel.Show();
        }
        else
        {
            OnLevelLose();
        }
    }



    public void Revive()
    {
        StartCoroutine(ReviveCoroutine());
    }

    private IEnumerator ReviveCoroutine()
    {
        TileRunner.Instance.Reverse3Seconds();
        yield return new WaitForSecondsRealtime(2);
        _ui.StartCountdownAfterRevive();
    }

    public void OnLevelLose()
    {
        _ui.HideHUD();
        _ui.OnLevelLose();
    }

    public void StartLevel()
    {
        CurrentState = State.Playing;
        TileRunner.Instance.StartTheGame();
    }

    public void ContinueLevel()
    {
        CurrentState = State.Playing;
        TileRunner.Instance.Continue();
    }
}