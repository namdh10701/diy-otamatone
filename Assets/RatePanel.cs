using Core.UI;
using DG.Tweening;
using Game.Shared;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class RatePanel : BasePopup
{
    int starCount = 0;
    [SerializeField] private List<GameObject> activeStars = new List<GameObject>();
    [SerializeField] private SkeletonGraphic instructionHand;
    public void OnStarClick(int index)
    {
        starCount = index + 1;
        for (int i = 0; i < 5; i++)
        {
            if (i < starCount)
            {
                activeStars[i].SetActive(true);
            }
            else
            {
                activeStars[i].SetActive(false);
            }
        }

        Debug.Log("Rated " + starCount);

        PlayerPrefs.SetInt(Constant.REVIEWED_BY_PLAYER_KEY, 1);
        if (starCount >= 4)
        {
            RateController.Instance.RateAndReview();
            Hide();
        }
        else
        {
            Hide();
        }
    }
}
