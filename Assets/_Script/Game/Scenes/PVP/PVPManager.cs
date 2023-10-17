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
    

    public enum State
    {
        Pause, Playing, Stop
    }

    public State CurrentState;
    public SongDefinition[] songDefinitions;
    public SongDefinition SelectedSongDefinition;

    public DancingMonster[] dancingMonsterPrefabs;
    public CameraFx cameraFx;


    [SerializeField] private Game.Timer timer;
    [SerializeField] private PVPSceneUIController _ui;

    private GameObject levelGO;

    private int reviveCount = 0;

    public RevivePanel reviePanel;

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
        //OnNoteMissed.RemoveAllListeners();
        HpManager.Instance.ResetHp();
        ScoreManager.Instance.ResetScore();
        Timer.Instance.ResetTimer();
        _ui.ResetUI();
        TileRunner.Instance.ResetLevel();
    }

    public void NewMatch()
    {
        if (levelGO != null)
        {
            Destroy(levelGO);

        }
        if (TileRunner.Instance != null)
        {
            TileRunner.Instance.StopGameEvent.RemoveListener(() => OnGameStop());
            TileRunner.Instance = null;
        }
        CurrentState = State.Stop;
        SelectedSongDefinition = SelectRandomSong();
        _ui.UpdateSongTitle(SelectedSongDefinition.SongName);
        InstantiateLevel();
        TileRunner.Instance.StopGameEvent.AddListener(() => OnGameStop());
        HpManager.Instance.OnZeroHp.RemoveListener(() => OnDeath());
        HpManager.Instance.OnZeroHp.AddListener(() => OnDeath());
    }



    public void OnGameStop()
    {
        if (CurrentState == State.Playing)
        {
            CurrentState = State.Stop;
            _ui.HideHUD();
        }
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
        HpManager.Instance.ResetHp();
        StartCoroutine(ReviveCoroutine());
    }

    private IEnumerator ReviveCoroutine()
    {
        cameraFx.ToGrayScale();
        TileRunner.Instance.Reverse3Seconds();
        yield return new WaitForSecondsRealtime(2);
        _ui.StartCountdownAfterRevive();
        cameraFx.ToColor();
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