using Core.Singleton;
using DG.Tweening;
using Game.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPSceneUIController : Singleton<PVPSceneUIController>
{
    [SerializeField] private Transform arrows;
    private Vector3 _originalArrowPos;

    [SerializeField] Animator _animator;
    [SerializeField] WinLosePanel _winLosePanel;
    [SerializeField] CountdownText _text;
    [SerializeField] PVPHud _pvpHUD;
    [SerializeField] FindingOpponentScreen _findingOpponentScreen;


    [SerializeField] private TextMeshProUGUI[] _songTitiles;
    private void Start()
    {
        TileRunner.Instance.StopGameEvent.RemoveListener(() => HideHUD());
        TileRunner.Instance.StopGameEvent.AddListener(() => HideHUD());

        _originalArrowPos = arrows.transform.position;
        _findingOpponentScreen.StartFindOpponent();
    }

    public void StartUpdateScore()
    {
        _winLosePanel.StartUpdateScore();
    }
    public void ResetArrows()
    {
        arrows.DOMove(_originalArrowPos, 0f);
    }



    public void HideHUD()
    {
        _pvpHUD.HideHud();
        arrows.DOMoveY(arrows.transform.position.y - 4.8f, 1f);
    }

    public void EnterHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void UpdateSongTitle(string songName)
    {
        foreach (TextMeshProUGUI title in _songTitiles)
        {
            title.text = "- " + songName + " -";
        }

    }

    public void ResetUI()
    {
        ResetArrows();
    }

    public void SetGameState(WinLosePanel.State state)
    {
        _winLosePanel.SetState(state);
    }

    public void OnLevelLose()
    {
        _winLosePanel.SetState(WinLosePanel.State.Lose);
    }

    public void DisplayEndgameUI()
    {
        _winLosePanel.gameObject.SetActive(true);
    }

    public void StartCountdownAfterRevive()
    {
        _animator.SetTrigger("CountdownAfterRevive");
    }
    public void OnCountdownEvent1()
    {
        AudioManager.Instance.PlaySound(SoundID.Beep);
    }
    public void OnContinueEvent()
    {
        PVPManager.Instance.ContinueLevel();
    }
}
