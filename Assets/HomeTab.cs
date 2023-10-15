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
    private void Start()
    {
        foreach (SongDefinition songDefinition in SongDefinitions)
        {
            SongItem songItem = null;
            if (songDefinition.IsNewAndHot)
            {
                songItem = Instantiate(SongItemPrefab, NewAndHotView);
            }
            if (songDefinition.IsTrending)
            {
                songItem = Instantiate(SongItemPrefab, TrendingView);
            }
            if (songDefinition.IsSkibidy)
            {
                songItem = Instantiate(SongItemPrefab, Skibidy);
            }
            if (songItem != null)
            {
                Debug.Log("Song def has not been tagged");
                songItem.Init(songDefinition);
            }
        }
    }
}
