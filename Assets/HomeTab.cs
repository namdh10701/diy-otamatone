using Game.Datas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeTab : MonoBehaviour
{
    public SongItem SongItemPrefab;
    public Transform NewAndHotView;
    public Transform TrendingView;
    public Transform Skibidy;

    public SongDefinition[] SongDefinitions;
    List<SongProgress> SongProgresses;
    private void Start()
    {
        SongProgresses = GameDataManager.Instance.GameDatas2.SongProgresses;

               
        foreach (SongDefinition songDefinition in SongDefinitions)
        {  SongProgress sp = SearchInSongProgresses(songDefinition.Id);
           
            SongItem songItem = null;
            if (songDefinition.IsNewAndHot)
            {
                songItem = Instantiate(SongItemPrefab, NewAndHotView);
                songItem.Init(songDefinition, sp);
            }
            if (songDefinition.IsTrending)
            {
                songItem = Instantiate(SongItemPrefab, TrendingView);
                songItem.Init(songDefinition, sp);
            }
            if (songDefinition.IsSkibidy)
            {
                songItem = Instantiate(SongItemPrefab, Skibidy);
                songItem.Init(songDefinition, sp);
            }
        }
    }

    private SongProgress SearchInSongProgresses(int songId)
    {
        foreach (SongProgress sp in SongProgresses)
        {
            if (sp.Id == songId)
            {
                return sp;
            }
        }
        return null;
    }
}
