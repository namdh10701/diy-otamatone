using Core.Singleton;
using Core.UI;
using DG.Tweening;
using Game.Audio;
using Game.Datas;
using Monetization.Ads.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static LevelDefinition;
using static PVPManager;

public class PlayBotManager : Singleton<PlayBotManager>
{
    public Transform P1;
    public Transform P2;
    DancingMonster P1Monster;
    DancingMonster P2Monster;

    public Transform Ready1;
    public Transform Ready2;
    public Transform Ready3;
    public Transform Begin;

    public CameraController cameraController;

    public enum State
    {
        Pause, Stop, Playing
    }
    public static SongDefinition SelectedSong;


    public static Difficulty SelectedDifficulty;
    public State CurrentState;
    public PausePanel pausePanel;
    public ResultPanel ResultPanel;
    public ArrowButton[] arrowButtons;
    public int missedCount = 0;
    public int hitCount = 0;
    private void Start()
    {
        P1Monster = P1.GetChild(LoadingMonsterManager.MonsterIndex).GetComponent<DancingMonster>();
        P1Monster.gameObject.SetActive(true);
        P2Monster = P2.GetChild(UnityEngine.Random.Range(0, 8)).GetComponent<DancingMonster>();
        P2Monster.gameObject.SetActive(true);
        CurrentState = State.Stop;
        switch (SelectedDifficulty)
        {
            case Difficulty.Easy:
                Instantiate(SelectedSong.EasyLevel);
                break;
            case Difficulty.Normal:
                Instantiate(SelectedSong.NormalLevel);
                break;
            case Difficulty.Hard:
                Instantiate(SelectedSong.HardLevel);
                break;
        }

        foreach (ArrowButton arrowButton in arrowButtons)
        {
            arrowButton.CooldownTime = TileRunner.Instance.cooldownTime;
        }
        TileRunner.Instance.StopGameEvent.AddListener(() => OnTileRunnerStop());
        TileRunner.Instance.OnNoteHit.AddListener((player) => OnNoteHit(player));
        TileRunner.Instance.OnNoteMissed.AddListener((player) => OnNoteMissed(player));
        P1Monster.Init(TileRunner.Instance.OnNoteMissed, TileRunner.Instance.OnNoteHit, TileRunner.Instance.StartGameEvent, TileRunner.Instance.StopGameEvent, TileRunner.Instance.LastNotePassedEvent, TileRunner.Player.P1);
        P2Monster.Init(TileRunner.Instance.OnNoteMissed, TileRunner.Instance.OnNoteHit, TileRunner.Instance.StartGameEvent, TileRunner.Instance.StopGameEvent, TileRunner.Instance.LastNotePassedEvent, TileRunner.Player.P2);
        cameraController.Init(TileRunner.Instance.OnNoteMissed, TileRunner.Instance.OnNoteHit, TileRunner.Instance.StartGameEvent, TileRunner.Instance.StopGameEvent, TileRunner.Instance.LastNotePassedEvent);
        StartCoroutine(CountdownToStartCoroutine());

    }

    private IEnumerator CountdownToStartCoroutine()
    {
        Sequence sequence = DOTween.Sequence();
        Ready1.gameObject.SetActive(false);
        Ready2.gameObject.SetActive(false);
        Ready3.gameObject.SetActive(false);
        Begin.gameObject.SetActive(false);
        sequence.AppendInterval(5);

        sequence.Append(Ready3.DOScale(.9f, .75f).OnStart(() =>
        {
            Ready3.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(SoundID.Beep);
        }).OnComplete(
            () => Ready3.gameObject.SetActive(false)
            ));
        sequence.Append(Ready2.DOScale(.9f, .75f).OnStart(() =>
        {
            Ready2.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(SoundID.Beep);
        }).OnComplete(
            () => Ready2.gameObject.SetActive(false)
            ));
        sequence.Append(Ready1.DOScale(.9f, .75f).OnStart(() =>
        {
            Ready1.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(SoundID.Beep);
        }).OnComplete(
            () => Ready1.gameObject.SetActive(false)
            ));
        sequence.Append(Begin.DOScale(1, .5f).OnStart(() => Begin.gameObject.SetActive(true)).OnComplete(() => Begin.gameObject.SetActive(false)));
        yield return new WaitForSeconds(.75f * 3 + 5);

        TileRunner.Instance.ResetLevel();
        TileRunner.Instance.StartTheGame();
        CurrentState = State.Playing;
    }

    private void OnNoteMissed(TileRunner.Player player)
    {
        missedCount++;
        PlayBotScoreManager.Instance.OnNoteMissed(player);
    }

    private void OnNoteHit(TileRunner.Player player)
    {
        hitCount++;
        PlayBotScoreManager.Instance.OnNoteHit(player);
    }

    public void OnTileRunnerStop()
    {
        if (CurrentState == State.Playing)
        {
            SongProgress sp = SearchInDB();

            bool isNewRecord = false;
            int star = 0;
            int score = PlayBotScoreManager.Instance.CurrentP1Score;
            switch (SelectedDifficulty)
            {
                case Difficulty.Easy:
                    if (score > sp.Easy.BestScore)
                    {
                        sp.Easy.BestScore = score;
                        isNewRecord = true;
                    }

                    foreach (int threshold in SelectedSong.EasyThreshold)
                    {
                        if (score > threshold)
                        {
                            star++;
                        }
                    }
                    if (star > sp.Easy.BestStar)
                    {
                        sp.Easy.BestStar = star;
                    }
                    break;
                case Difficulty.Normal:
                    if (score > sp.Normal.BestScore)
                    {
                        sp.Normal.BestScore = score;
                        isNewRecord = true;
                    }

                    foreach (int threshold in SelectedSong.NormalThreshold)
                    {
                        if (score > threshold)
                        {
                            star++;
                        }
                    }
                    if (star > sp.Normal.BestStar)
                    {
                        sp.Normal.BestStar = star;
                    }
                    break;
                case Difficulty.Hard:
                    if (score > sp.Hard.BestScore)
                    {
                        Debug.Log("BestScored");
                        sp.Hard.BestScore = score;
                        isNewRecord = true;
                    }
                    else
                    {

                        Debug.Log("NotBestScored");
                    }

                    foreach (int threshold in SelectedSong.HardThreshold)
                    {
                        if (score > threshold)
                        {
                            star++;
                        }
                    }
                    if (star > sp.Hard.BestStar)
                    {
                        sp.Hard.BestStar = star;
                    }
                    break;
            }
            GameDataManager.Instance.SaveDatas2();

            CurrentState = State.Stop;



            ResultPanel.Init(SelectedSong, PlayBotScoreManager.Instance.BestCombo, PlayBotScoreManager.Instance.CurrentP1Score
                , missedCount, hitCount, star, isNewRecord);
            ResultPanel.Show();
        }
    }

    private SongProgress SearchInDB()
    {
        GameData2 gameData2 = GameDataManager.Instance.GameDatas2;
        foreach (SongProgress sp in gameData2.SongProgresses)
        {
            if (sp.Id == SelectedSong.Id)
            {
                return sp;
            }
        }
        return null;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        CurrentState = State.Pause;
        TileRunner.Instance.PauseGame();
        pausePanel.ShowWithCallback(() =>
        {
            OnContinue();
        });
    }
    public void OnContinue()
    {
        Time.timeScale = 1;
        TileRunner.Instance.Continue();
        CurrentState = State.Playing;
    }
    public void Replay()
    {
        Time.timeScale = 1;
        hitCount = 0;
        missedCount = 0;
        P1Monster.OnReset();
        P2Monster.OnReset();
        PlayBotScoreManager.Instance.ResetScore();
        TileRunner.Instance.ResetLevel();
        StartCoroutine(CountdownToStartCoroutine());

    }
}