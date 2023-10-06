using Core.UI;
using Services.FirebaseService.Analytics;
using Game.Audio;
using Game.Datas;
using Monetization.Ads;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Game.Shared;

namespace Game.Craft
{
    public class CraftCompleteView : BasePopup
    {
        public SkeletonGraphic Curtain;

        public void OnGetCoin()
        {

            FirebaseAnalytics.LogEvent("reward_coin_" + CraftSequenceManager.versionName + "_" + CraftSequenceManager.playedTimeCount);
            AdsController.Instance.ShowReward(
                watched =>
                {
                    if (watched)
                    {
                        AddGold();
                    }
                }
                );

        }

        private void AddGold()
        {
            Hide();
            GameDataManager.Instance.UpdateGold(25);
            GameDataManager.Instance.SaveDatas();
            AudioManager.Instance.PlaySound(SoundID.Coin);
            Invoke("CloseCurtain", 2f);

        }

        public void OnNext()
        {
            FirebaseAnalytics.LogEvent("Replay_" + CraftSequenceManager.versionName + "_" + CraftSequenceManager.playedTimeCount);

            if ((PlayerPrefs.GetInt(Constant.REVIEWED_BY_PLAYER_KEY, 0)) == 0
                && CraftSequenceManager.playedTimeCount >= 2)
            {
                Hide();
                ViewManager.Instance.GetView<RatePanel>().Show(
                    () =>
                    {
                        CloseCurtain();
                        return;
                    }
                    );
            }
            else
            {
                AdsController.Instance.ShowInter(
                    () =>
                    {
                        Hide();
                        CloseCurtain();
                    }
                    );
            }
        }
        private void CloseCurtain()
        {
            Curtain.AnimationState.SetAnimation(0, "Open", false);
            Curtain.AnimationState.AddAnimation(0, "Close", false, 1.1f);
            Curtain.AnimationState.AddAnimation(0, "Open_Idle", false, 0);
            Invoke("ResetCraft", 1);
        }
        private void ResetCraft()
        {
            CraftSequenceManager.Instance.ResetCraft();
            AudioManager.Instance.CrossfadeMusic(SoundID.Menu_BGM, .2f);
        }

        public void OnPianoTiles()
        {
            Hide();
            Curtain.AnimationState.SetAnimation(0, "Open", false);
            Curtain.AnimationState.AddAnimation(0, "Close", false, 1.1f);
            Curtain.AnimationState.AddAnimation(0, "Open_Idle", false, 0);
            Invoke("EnterPianoTilesMode", 1);

        }
        private void EnterPianoTilesMode()
        {
            PianoTilesManager.Instance.EnterPianoTilesMode();
        }
    }
}