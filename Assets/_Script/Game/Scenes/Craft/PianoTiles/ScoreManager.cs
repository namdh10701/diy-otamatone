using System;
using UnityEngine;
using Core.Singleton;
using TMPro;
using Facebook.Unity;

public class ScoreManager : Singleton<ScoreManager>
{
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public int CurrentP1Score;
    public int CurrentP2Score;

    public void UpdateScore(int amount, bool isP1)
    {
        if (isP1)
        {
            CurrentP1Score += amount;
            p1ScoreText.text = CurrentP1Score.ToString("######");
        }
        else
        {
            CurrentP2Score += amount;
            p2ScoreText.text = CurrentP2Score.ToString("######");
        }
    }

}