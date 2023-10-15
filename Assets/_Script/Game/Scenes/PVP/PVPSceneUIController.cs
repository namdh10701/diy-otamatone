using Core.Singleton;
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
    public Animator animator;
    public Sprite[] sprites;
    public Image monsterLoading;
    public Image avatarLoading;
    public Image monster;
    public Image avatar;
    public TextMeshProUGUI monsterName;
    public int SelectedMonsterIndex;
    public WinLosePanel winLosePanel;
    private void Start()
    {
        StartCoroutine(FindOpponent());
    }

    public void StartUpdateScore()
    {
        winLosePanel.StartUpdateScore();
        Debug.Log("sd");
    }

    public void ResetGameForNewMatch()
    {
        StartCoroutine(FindOpponent());
    }

    private IEnumerator FindOpponent()
    {
        avatar.gameObject.SetActive(false);
        monster.gameObject.SetActive(false);
        animator.SetTrigger("FindingUIAppear");
        float elapsedTime = 0;
        int imageIndex = 0;
        int previousImageIndex = -1;

        while (elapsedTime <= 3)
        {
            monsterLoading.sprite = sprites[imageIndex];
            avatarLoading.sprite = sprites[imageIndex];
            previousImageIndex = imageIndex;
            while (imageIndex == previousImageIndex)
            {
                imageIndex = UnityEngine.Random.Range(0, sprites.Length);
            }
            elapsedTime += 0.35f;
            yield return new WaitForSeconds(0.35f);
        }
        avatar.sprite = avatarLoading.sprite;
        monster.sprite = monsterLoading.sprite;
        avatar.gameObject.SetActive(true);
        monster.gameObject.SetActive(true);
        monsterName.text = "User@" + "666";
        PVPManager.Instance.OnOpponentSelected(imageIndex);
        yield return new WaitForSeconds(1f);
        OnFoundOpponent();
    }


    public void OnFoundOpponent()
    {
        animator.SetTrigger("SelectingSongAppear");
    }

    public void OnSelectingSongAppeared()
    {
        animator.SetTrigger("FindingUIDisappear");
    }

    public void OnDisappeared()
    {
        PVPManager.Instance.DisplayEndgameUI();
    }
    public void OnCountdownEvent()
    {
        AudioManager.Instance.PlaySound(SoundID.Beep);
    }

    public void HideHUD()
    {

        animator.SetTrigger("HUD Dissappear");
    }

    public void OnScoreCalculated()
    {
        animator.SetTrigger("WinLoseAppear");
    }

    public void StartCountdown()
    {
        PVPManager.Instance.ResetArrows();
        animator.SetTrigger("StartCountdown");
    }
    public void OnBeginEvent()
    {
        PVPManager.Instance.StartGame();
    }

    public void EnterHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
