using Spine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel : MonoBehaviour
{
    public enum State
    {
        Win, Lose, Compare
    }
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI songTitle;
    [SerializeField] private TextMeshProUGUI p1Scoretext;
    [SerializeField] private TextMeshProUGUI p2Scoretext;
    public Sprite winSprite;
    public Sprite loseSprite;
    [SerializeField] private Image loseWinIcon;
    private State state;

    public void StartUpdateScore()
    {
        StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        int p1Score = 0;
        int p2Score = 0;
        p1Scoretext.text = "000000";
        p2Scoretext.text = "000000";

        float elapsedTime = 0;
        float progress = 0;
        float duration = 4;
        while (progress < 1)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            p1Score = (int)Mathf.Lerp(0, ScoreManager.Instance.CurrentP1Score, progress);
            p2Score = (int)Mathf.Lerp(0, ScoreManager.Instance.CurrentP2Score, progress);
            if (ScoreManager.Instance.CurrentP1Score == 0)
            {
                p1Scoretext.text = "000000";
            }
            else
            {
                p1Scoretext.text = p1Score.ToString("######");
            }
            if (ScoreManager.Instance.CurrentP2Score == 0)
            {
                p2Scoretext.text = "000000";
            }
            else
            {
                p2Scoretext.text = p2Score.ToString("######");
            }
            yield return null;
        }
        p1Score = ScoreManager.Instance.CurrentP1Score;
        p2Score = ScoreManager.Instance.CurrentP2Score;
        if (ScoreManager.Instance.CurrentP1Score == 0)
        {
            p1Scoretext.text = "000000";
        }
        else
        {
            p1Scoretext.text = p1Score.ToString("######");
        }
        if (ScoreManager.Instance.CurrentP2Score == 0)
        {
            p2Scoretext.text = "000000";
        }
        else
        {
            p2Scoretext.text = p2Score.ToString("######");
        }
        CompareScore();
    }

    private void CompareScore()
    {
        if (state == State.Lose)
        {
            loseWinIcon.sprite = loseSprite;
        }
        else
        {
            if (ScoreManager.Instance.CurrentP1Score >= ScoreManager.Instance.CurrentP2Score)
            {
                loseWinIcon.sprite = winSprite;
            }
            else
            {
                loseWinIcon.sprite = loseSprite;
            }

        }
        OnScoreCalculated();
    }

    public void SetState(State state)
    {
        this.state = state;
    }

    public void OnScoreCalculated()
    {
        _animator.SetTrigger("WinLoseAppear");
    }
}
