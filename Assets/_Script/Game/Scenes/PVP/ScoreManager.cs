
using Core.Singleton;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public int CurrentP1Score;
    public int CurrentP2Score;

    private int CurrentP1Combo = 0;
    private int CurrentP2Combo = 0;
    public void OnEnable()
    {
        PVPManager.Instance.OnNoteHit.AddListener(player => OnNoteHit(player));
        PVPManager.Instance.OnNoteMissed.AddListener(player => OnNoteMissed(player));
    }

    private void OnNoteHit(PVPManager.Player player)
    {
        if (player == PVPManager.Player.P1)
        {
            CurrentP1Combo++;
            CurrentP1Score += 10*(int)(CurrentP1Combo * (CurrentP1Combo + 1) * .5f);

            UpdateScore(CurrentP1Score, true);
        }
        else
        {
            CurrentP2Combo++;
            CurrentP2Score += 10 * (int)(CurrentP2Combo * (CurrentP2Combo + 1) * .5f);
            UpdateScore(CurrentP2Score, false);
        }
    }
    private void OnNoteMissed(PVPManager.Player player)
    {
        if (player == PVPManager.Player.P1)
        {
            CurrentP1Combo = 0;
        }
        else
        {
            CurrentP2Combo = 0;
        }
    }


    public void UpdateScore(int amount, bool isP1)
    {
        if (isP1)
        {
            p1ScoreText.text = CurrentP1Score.ToString().PadLeft(6, '0');
        }
        else
        {
            p2ScoreText.text = CurrentP2Score.ToString().PadLeft(6, '0');
        }
    }

    public void ResetScore()
    {
        CurrentP1Combo = 0;
        CurrentP2Combo = 0;
        CurrentP1Score = 0;
        CurrentP2Score = 0;
        p1ScoreText.text = "000000";
        p2ScoreText.text = "000000";
    }
}