using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongItem : MonoBehaviour
{
    public SongDefinition SongDefinition;
    public TextMeshProUGUI SongTitle;
    public LevelDefinition.Difficulty Difficulty = LevelDefinition.Difficulty.Normal;
    public GameObject[] Difficulties;
    public GameObject[] Stars;
    public void Init(SongDefinition songDefinition)
    {
        SongDefinition = songDefinition;
        SongTitle.text = songDefinition.SongName;
        for (int i = 0; i < Stars.Length; i++)
        {
            if (i < songDefinition.Stars)
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
    }
}
