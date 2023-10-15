using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI songTitle;
    [SerializeField] private TextMeshProUGUI p1Scoretext;
    [SerializeField] private TextMeshProUGUI p2Scoretext;
    public Sprite winSprite;
    public Sprite loseSprite;
    [SerializeField] private Image loseWinIcon;
    private void Start()
    {
        songTitle.text = "-" + PVPManager.Instance.SelectedSongDefinition.SongName + "-";
        
    }

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
            p1Scoretext.text = p1Score.ToString("######");
            p2Scoretext.text = p2Score.ToString("######");
            yield return null;
        }
        p1Score = ScoreManager.Instance.CurrentP1Score;
        p2Score = ScoreManager.Instance.CurrentP2Score;
        p1Scoretext.text = p1Score.ToString("######");
        p2Scoretext.text = p2Score.ToString("######");
        CompareScore();
    }

    private void CompareScore()
    {
        if (ScoreManager.Instance.CurrentP1Score >= ScoreManager.Instance.CurrentP2Score)
        {
            loseWinIcon.sprite = winSprite;
        }
        else
        {
            loseWinIcon.sprite = loseSprite;
        }
        PVPSceneUIController.Instance.OnScoreCalculated();
    }
}
