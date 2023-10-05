using UnityEngine;
using Core.Singleton;
using static Game.Craft.CraftStateSequence;
using System;
using Game.Datas;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;
using Game.Audio;
using Monetization.Ads.UI;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using TMPro;

namespace Game.Craft
{
    public class CraftSequenceManager : Singleton<CraftSequenceManager>
    {
        [SerializeField] public NativeAdPanel _nativeAdPanel;
        public static string versionName;
        public static int playedTimeCount;

        public static UnityEvent OnActionTaken = new UnityEvent();
        public static UnityEvent OnHeadPicked = new UnityEvent();
        public static UnityEvent OnOtamatoneCompeleted = new UnityEvent();
        public static UnityEvent OnDoneClicked = new UnityEvent();

        public static bool HeadTapShowInter = true;
        public static bool MonsterTapShowInter = true;
        public static bool EyeTapShowInter = true;
        public static bool MouthTapShowInter = true;
        public static bool BgTapShowInter = true;
        public static bool BodyTapShowInter = true;
        public static bool SequenceBtnTapShowInter = true;
        public static bool BeforeWinPanelShowInter = true;
        public static bool NextBtnTapShowInter = true;

        private CraftStateSequence _craftStateSequence;
        [SerializeField] private Sequence[] _sequences;
        [SerializeField] private Booth _booth;
        public NextButton NextButton;
        public GameObject _confirmButton;
        public CraftCompleteView _craftCompleteView;
        public bool IsCompleted;
        Sequence first; Sequence second; Sequence third; Sequence fourth; Sequence fifth; Sequence sixth;
        public Otamatone otamatone;
        [SerializeField] private SequenceButton[] _sequenceButtons;


        private Dictionary<int, string> _skinIdNameMap = new Dictionary<int, string>()
    {
        {0,"default"},
        {1,"Joyville"},
        {2,"Banana Cat"},
        {3,"Rainbow Blue"},
        {4,"Banban"},
        {5,"Amanda"},
        {6,"Wednesday"},
        {7,"Jumbo Josh"}
    };




        public Sequence CurrentSeqeuence
        {
            get
            {
                return _craftStateSequence.CurrentState;
            }
        }

        public Booth Booth => _booth;
        public PartID[] Order =
        {
            PartID.Monster,
            PartID.Head,
            PartID.Body,
            PartID.Eye,
            PartID.Mouth,
            PartID.Background
        };
        private void Start()
        {
            Debug.Log(Screen.safeArea);
            AdsHandler adsHandler = FindFirstObjectByType<AdsHandler>();
            adsHandler.LoadBannerAndNativeAd();

            AdsController.Instance.ShowNativeAd(_nativeAdPanel);

            versionName = Application.version;
            versionName = versionName.Replace(".", "");
            versionName = "v" + versionName;
            MicCheck();
            AudioManager.Instance.PlayMusic(SoundID.Menu_BGM, true);
            playedTimeCount = PlayerPrefs.GetInt("PLAYED_TIME_COUNT");
            foreach (Sequence sequence in _sequences)
            {
                if (sequence.PartID == PartID.Monster)
                {
                    first = sequence;
                }
                if (sequence.PartID == PartID.Head)
                {
                    second = sequence;
                }
                if (sequence.PartID == PartID.Body)
                {
                    third = sequence;
                }
                if (sequence.PartID == PartID.Eye)
                {
                    fourth = sequence;
                }
                if (sequence.PartID == PartID.Mouth)
                {
                    fifth = sequence;
                }
                if (sequence.PartID == PartID.Background)
                {
                    sixth = sequence;
                }
            }
            _craftStateSequence = new CraftStateSequence(first, second, third,
                fourth, fifth, sixth);

            _sequenceButtons[0].SetData(first);
            _sequenceButtons[1].SetData(second);
            _sequenceButtons[2].SetData(third);
            _sequenceButtons[3].SetData(fourth);
            _sequenceButtons[4].SetData(fifth);
            _sequenceButtons[5].SetData(sixth);
            _craftStateSequence.CurrentState.Enter();
            MicCheck();

        }
        private void MicCheck()
        {
            playedTimeCount = PlayerPrefs.GetInt("PLAYED_TIME_COUNT", 0);
            playedTimeCount++;
            PlayerPrefs.SetInt("PLAYED_TIME_COUNT", playedTimeCount);
            FirebaseAnalytics.LogEvent("Start_" + versionName + "_" + playedTimeCount);
        }
        public void OnNextState()
        {
            if (NextBtnTapShowInter)
            {
                AdsController.Instance.ShowInter(
                    () =>
                    {
                        ProcessNextState();
                    }
                    );
            }
            else
            {
                ProcessNextState();
            }

        }

        private void ProcessNextState()
        {
            OnActionTaken.Invoke();
            int selectedTimeCount = 0;
            int index = CurrentSeqeuence.SelectedItemIndex;
            if (CurrentSeqeuence.PartID == PartID.Monster)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Monster_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Monster_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_monster_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            if (CurrentSeqeuence.PartID == PartID.Head)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Head_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Head_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_head_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            if (CurrentSeqeuence.PartID == PartID.Mouth)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Mouth_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Mouth_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_mouth_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            if (CurrentSeqeuence.PartID == PartID.Eye)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Eye_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Eye_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_eye_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            if (CurrentSeqeuence.PartID == PartID.Body)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Body_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Body_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_body_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            if (CurrentSeqeuence.PartID == PartID.Background)
            {
                selectedTimeCount = PlayerPrefs.GetInt("Background_" + (index + 1), 0);
                selectedTimeCount++;
                PlayerPrefs.SetInt("Background_" + (index + 1), selectedTimeCount);
                FirebaseAnalytics.LogEvent("Next_BG_" + versionName + "_" + (index + 1) + "_" + selectedTimeCount);
            }
            _craftStateSequence.Next();
            if (_craftStateSequence.IsLastState)
            {
                NextButton.gameObject.SetActive(false);
            }
        }

        public void OnStateSelected(PartID partID)
        {
            if (SequenceBtnTapShowInter)
            {
                AdsController.Instance.ShowInter(
                    () =>
                    {
                        ProcessStateSelect(partID);
                    }
                    );
            }
            else
            {
                ProcessStateSelect(partID);
            }
        }

        private void ProcessStateSelect(PartID partID)
        {
            OnActionTaken.Invoke();
            if (partID == _craftStateSequence.CurrentState.PartID)
            {
                return;
            }
            if (_sequences[(int)partID].IsCompleted)
            {
                _craftStateSequence.SetCurrentSequence(_sequences[(int)partID]);
                OnActionTaken.Invoke();
            }

            if (_craftStateSequence.IsLastState || IsCompleted)
            {
                NextButton.gameObject.SetActive(false);
                _confirmButton.gameObject.SetActive(true);
            }
            else if (_craftStateSequence.CurrentState.IsCompleted)
            {
                NextButton.gameObject.SetActive(true);
            }
        }

        bool OtamatoneCompleted = false;
        public void OnItemSelected(int index)
        {
            OnActionTaken.Invoke();
            if (CurrentSeqeuence.PartID == PartID.Head)
            {
                OnHeadPicked.Invoke();
            }
            if (CurrentSeqeuence.PartID == PartID.Mouth)
            {
                if (!OtamatoneCompleted)
                {
                    OtamatoneCompleted = true;
                    OnOtamatoneCompeleted.Invoke();
                }
            }

            _craftStateSequence.CurrentState.OnSelect(index);
            if (_craftStateSequence.IsLastState)
            {
                IsCompleted = true;
                NextButton.gameObject.SetActive(false);
                _confirmButton.gameObject.SetActive(true);
            }
            else
            {
                NextButton.Activate();
            }
        }
        public void OnSequenceEnter(Sequence sequence)
        {
            foreach (SequenceButton b in _sequenceButtons)
            {
                if (b.Sequence == _craftStateSequence.CurrentState)
                {
                    b.Select();
                }
                else
                {
                    b.DeSelect();
                }
            }
            _booth.OnSequenceEnter(sequence);


            if (sequence.IsCompleted && sequence.SelectedItemIndex != -1)
            {
                NextButton.Activate();
            }
            else
            {
                NextButton.DeActivate();
            }
        }

        public void OnFinishCraft()
        {
            StopAllCoroutines();
            AudioManager.Instance.PlaySound(SoundID.Win);
            AudioManager.Instance.StopMusic();
            FirebaseAnalytics.LogEvent("Done_" + versionName + "_" + playedTimeCount);
            OnDoneClicked.Invoke();
            _booth.HideContents();
            otamatone.FinishCraft();

        }

        public void ResetCraft()
        {
            OtamatoneCompleted = false;
            IsCompleted = false;
            NextButton.gameObject.SetActive(true);
            _confirmButton.SetActive(false);
            otamatone.ResetOtamatone();
            foreach (Sequence sequence in _sequences)
            {
                sequence.ResetSequence();
            }
            _craftStateSequence = new CraftStateSequence(first, second, third,
                fourth, fifth, sixth);

            _sequenceButtons[0].SetData(first);
            _sequenceButtons[1].SetData(second);
            _sequenceButtons[2].SetData(third);
            _sequenceButtons[3].SetData(fourth);
            _sequenceButtons[4].SetData(fifth);
            _sequenceButtons[5].SetData(sixth);
            _craftStateSequence.CurrentState.Enter();

            _booth.ShowContent();
            otamatone.ResetOtamatone();
            MicCheck();

        }

        public void ShowFinishPanel()
        {
            _craftCompleteView.Show();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            if (!IsCompleted)
            {
                playedTimeCount = PlayerPrefs.GetInt("PLAYED_TIME_COUNT", 1);
                playedTimeCount--;
                PlayerPrefs.SetInt("PLAYED_TIME_COUNT", playedTimeCount);
            }
        }

        public void IntroduceCat()
        {
            StartCoroutine(IntroduceCatCoroutine());
        }

        private IEnumerator IntroduceCatCoroutine()
        {
            AudioManager.Instance.CrossfadeMusic(SoundID.Cat_BGM, .3f);
            yield return new WaitForSeconds(7);
            AudioManager.Instance.CrossfadeMusic(SoundID.Menu_BGM, .3f);
        }

        public void EnterPianoTilesMode()
        {
            throw new NotImplementedException();
        }
    }
}