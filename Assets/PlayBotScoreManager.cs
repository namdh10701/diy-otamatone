
using Core.Singleton;
using TMPro;

public class PlayBotScoreManager : Singleton<PlayBotScoreManager>
{
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p1ComboText;

    public int CurrentP1Score;

    private int CurrentP1Combo = 0;
    public int BestCombo = 0;
    public void OnNoteHit(TileRunner.Player player)
    {
        if (player == TileRunner.Player.P1)
        {
            CurrentP1Combo++;
            if (BestCombo < CurrentP1Combo)
            {
                BestCombo = CurrentP1Combo;
            }
            CurrentP1Score += 10 * (int)(CurrentP1Combo * (CurrentP1Combo + 1) * .5f);
            p1ComboText.text = CurrentP1Combo.ToString();
            UpdateScore(CurrentP1Score, true);
        }
    }

    public void OnNoteMissed(TileRunner.Player player)
    {
        if (player == TileRunner.Player.P1)
        {
            CurrentP1Combo = 0;
            p1ComboText.text = CurrentP1Combo.ToString();
        }
    }


    public void UpdateScore(int amount, bool isP1)
    {
        if (isP1)
        {
            p1ScoreText.text = CurrentP1Score.ToString().PadLeft(7, '0');
        }
    }

    public void ResetScore()
    {
        CurrentP1Combo = 0;
        CurrentP1Score = 0;
        p1ScoreText.text = "0000000";
        p1ComboText.text = "0";
    }
}