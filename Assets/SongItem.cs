using Game.Datas;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SongItem : MonoBehaviour
{
    public SongDefinition SongDefinition;
    public TextMeshProUGUI SongTitle;
    public LevelDefinition.Difficulty Difficulty = LevelDefinition.Difficulty.Normal;
    public GameObject[] Difficulties;
    public GameObject[] Stars;
    public int EasyStar;
    public int NormalStar;
    public int HardStar;
    public void Init(SongDefinition songDefinition, SongProgress sp)
    {
        EasyStar = sp.Easy.BestStar;
        NormalStar = sp.Normal.BestStar;
        HardStar = sp.Hard.BestStar;
        SongDefinition = songDefinition;
        SongTitle.text = songDefinition.SongName;
        UpdateStar();

    }

    private void UpdateStar()
    {
        int srcStar = 0;
        switch (Difficulty)
        {
            case LevelDefinition.Difficulty.Easy:
                srcStar = EasyStar;
                break;
            case LevelDefinition.Difficulty.Normal:
                srcStar = NormalStar;
                break;
            case LevelDefinition.Difficulty.Hard:
                srcStar = HardStar;
                break;
        }

        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < srcStar)
            {
                Stars[i].gameObject.SetActive(true);
            }
            else
            {
                Stars[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnPlayButtonClick()
    {
        PlayBotManager.SelectedSong = SongDefinition;
        PlayBotManager.SelectedDifficulty = Difficulty;
        HomeUIController.Instance.EnterPlayBot(this);
        //TODO : Load new to PlayBotScene
    }
    public void IncreaseDifficulty()
    {
        Difficulties[(int)Difficulty].SetActive(false);
        if (Difficulty == LevelDefinition.Difficulty.Hard)
        {
            Difficulty = LevelDefinition.Difficulty.Easy;
        }
        else
        {
            Difficulty++;
        }
        Difficulties[(int)Difficulty].SetActive(true);
        UpdateStar();
    }

    public void DecreaseDifficulty()
    {
        Difficulties[(int)Difficulty].SetActive(false);
        if (Difficulty == LevelDefinition.Difficulty.Easy)
        {
            Difficulty = LevelDefinition.Difficulty.Hard;
        }
        else
        {
            Difficulty--;
        }
        Difficulties[(int)Difficulty].SetActive(true);
        UpdateStar();
    }
}
