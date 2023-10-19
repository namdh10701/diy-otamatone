using Core.UI;
using DG.Tweening;
using Monetization.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI songTitle;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI missed;
    [SerializeField] TextMeshProUGUI great;
    [SerializeField] TextMeshProUGUI bestCombo;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject[] diffs;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] GameObject newRecord;
    [SerializeField] PrizeWheel prizeWheel;

    public void Init(SongDefinition songDefinition, int bestCombo, int score, int missed, int great, int stars, bool isNewRecord)
    {
        for (int i = 0; i < diffs.Length; i++)
        {
            if (i == (int)PlayBotManager.SelectedDifficulty)
            {
                diffs[i].gameObject.SetActive(true);
            }
            else
            {
                diffs[i].gameObject.SetActive(false);
            }
        }
        newRecord.SetActive(isNewRecord);
        songTitle.text = songDefinition.SongName;
        this.great.text = great.ToString();
        this.score.text = "Score: " + score.ToString();
        this.missed.text = missed.ToString();
        this.bestCombo.text = bestCombo.ToString();
        for (int i = 0; i < stars; i++)
        {
            this.stars[i].SetActive(true);
        }
    }

    public void OnReplay()
    {
        AdsController.Instance.ShowInter(
            () =>
            {
                HideImmediately();
                OnReset();
                PlayBotManager.Instance.Replay();
            }
            );

    }

    public void OnHome()
    {
        AdsController.Instance.ShowInter(
            () =>
            {
                SceneManager.LoadScene("HomeScene");
            }
            );
    }
    public void Show()
    {
        prizeWheel.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
    void HideImmediately()
    {
        gameObject.SetActive(false);
    }

    void OnReset()
    {
        for (int i = 0; i < diffs.Length; i++)
        {
            diffs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < stars.Length; i++)
        {
            this.stars[i].SetActive(false);
        }
    }
}
