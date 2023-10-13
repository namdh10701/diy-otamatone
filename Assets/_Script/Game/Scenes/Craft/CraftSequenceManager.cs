using UnityEngine;
using Core.Singleton;
using static Game.Craft.CraftStateSequence;
using System;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using Game.Audio;
using Monetization.Ads.UI;
using Monetization.Ads;
using Services.FirebaseService.Analytics;
using Game.RemoteVariable;
using Services.FirebaseService.Remote;
using Game.Datas;
using UnityEngine.SceneManagement;

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
        public static bool IsNextClicked = false;

        private CraftStateSequence _craftStateSequence;
        private Sequence[] _sequences = new Sequence[6];
        [SerializeField] private Booth _booth;
        public NextButton NextButton;
        public GameObject ConfirmButton;
        public CraftCompleteView CraftCompleteView;
        public bool IsCompleted;
        public Otamatone Otamatone;
        [SerializeField] private SequenceButton[] _sequenceButtons;

        public UpdateNoteVisual UpdateNoteVisual;

        public Sequence CurrentSeqeuence => _craftStateSequence.CurrentState;

        public Booth Booth => _booth;

        public void EndFreeInterTime()
        {
            AdsController.Instance._isFreeAdsTimeEnded = true;
        }
        private void Start()
        {
            RemoteVariable.RemoteVariable remoteVariable = Game.RemoteVariable.RemoteVariable.Convert(RemoteVariableManager.Instance.MyRemoteVariables);
            Invoke("EndFreeInterTime", remoteVariable.FreeInterTime);
            AdsHandler adsHandler = FindFirstObjectByType<AdsHandler>();
            adsHandler.LoadBannerAndNativeAd();

            AdsController.Instance.ShowNativeAd(_nativeAdPanel);
            AudioManager.Instance.PlayMusic(SoundID.Menu_BGM, true);

            versionName = Application.version;
            versionName = versionName.Replace(".", "");
            MicCheck();
            _sequences = new Sequence[] {
                new Sequence(PartID.Monster),
                new Sequence(PartID.Head),
                new Sequence(PartID.Body),
                new Sequence(PartID.Eye),
                new Sequence(PartID.Mouth),
                new Sequence(PartID.Background)
            };

            _craftStateSequence = new CraftStateSequence(_sequences);
            _sequenceButtons[0].SetData(_sequences[0]);
            _sequenceButtons[1].SetData(_sequences[1]);
            _sequenceButtons[2].SetData(_sequences[2]);
            _sequenceButtons[3].SetData(_sequences[3]);
            _sequenceButtons[4].SetData(_sequences[4]);
            _sequenceButtons[5].SetData(_sequences[5]);
            _craftStateSequence.CurrentState.Enter();

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

            if (CurrentSeqeuence.IsCompleted && !CurrentSeqeuence.ReEnter)
            {
                GameDataManager.Instance.UpdateNote(1);
                GameDataManager.Instance.SaveDatas2();
                UpdateNoteVisual.Play();
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
            foreach (Sequence s in _sequences)
            {
                if (s.PartID == partID)
                {
                    if (s.IsCompleted)
                    {
                        _craftStateSequence.SetCurrentSequence(s);
                        OnActionTaken.Invoke();
                    }
                }
            }

            if (_craftStateSequence.IsLastState || IsCompleted)
            {
                NextButton.gameObject.SetActive(false);
                ConfirmButton.gameObject.SetActive(true);
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
                ConfirmButton.gameObject.SetActive(true);
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
            // TODO : Note here
            if (CurrentSeqeuence.IsCompleted && !CurrentSeqeuence.ReEnter)
            {
                  GameDataManager.Instance.UpdateNote(1);
                  GameDataManager.Instance.SaveDatas2();
                  UpdateNoteVisual.Play();
            }
            CurrentSeqeuence.Exit();
            StopAllCoroutines();
            AudioManager.Instance.PlaySound(SoundID.Win);
            AudioManager.Instance.StopMusic();
            FirebaseAnalytics.LogEvent("Done_" + versionName + "_" + playedTimeCount);
            OnDoneClicked.Invoke();
            _booth.HideContents();
            Otamatone.FinishCraft();

        }

        public void ResetCraft()
        {
            MicCheck();
            OtamatoneCompleted = false;
            IsCompleted = false;
            NextButton.gameObject.SetActive(true);
            ConfirmButton.SetActive(false);
            Otamatone.ResetOtamatone();
            foreach (Sequence sequence in _sequences)
            {
                sequence.ResetSequence();
            }
            _craftStateSequence = new CraftStateSequence(_sequences);
            _sequenceButtons[0].SetData(_sequences[0]);
            _sequenceButtons[1].SetData(_sequences[1]);
            _sequenceButtons[2].SetData(_sequences[2]);
            _sequenceButtons[3].SetData(_sequences[3]);
            _sequenceButtons[4].SetData(_sequences[4]);
            _sequenceButtons[5].SetData(_sequences[5]);
            _craftStateSequence.CurrentState.Enter();
            _booth.ShowContent();
            Otamatone.ResetOtamatone();

        }

        public void ShowFinishPanel()
        {
            CraftCompleteView.Show();
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

        public void EnterHome()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}