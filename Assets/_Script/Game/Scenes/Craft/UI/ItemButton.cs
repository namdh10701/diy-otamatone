using Core.UI;
using DG.Tweening;
using Game.Audio;
using Game.Datas;
using Game.RemoteVariable;
using Game.UI;
using Monetization.Ads;
using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Game.Craft.CraftStateSequence;

namespace Game.Craft
{
    public class ItemButton : MonoBehaviour
    {
        public ItemButtonBackground ItemButtonBackground;
        public bool IsDisplaying;
        public bool IsActivated;
        [SerializeField] private GameObject _selectorHighlight;
        private int _index;
        [SerializeField] private Image _background;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private BoothContent _boothContent;

        [SerializeField] private GameObject _rewardLock;
        [SerializeField] private GameObject _coinLock;
        [SerializeField] private TextMeshProUGUI _coinLockText;
        private Locker _locker;
        public bool Selected { get; private set; }

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
        }

        private void OnDisable()
        {
            tween.Kill();
        }
        public void SetData(Sprite sprite, int index, Locker locker)
        {
            if (tween != null)
            {
                tween.Kill();
            }
            IsDisplaying = true;

            transform.DOScale(0, 0);
            _locker = locker;
            _index = index;
            _image.sprite = sprite;
            _background.sprite = ItemButtonBackground.Deselected;
            _button.onClick.RemoveAllListeners();
            _coinLock.gameObject.SetActive(false);
            _rewardLock.gameObject.SetActive(false);
            if (locker == null)
            {
                _button.onClick.AddListener(Select);
                return;
            }
            else
            {
                if (locker.Unlocked)
                {
                    _button.onClick.AddListener(Select);
                    return;
                }
                else
                {
                    if (locker.Type == Locker.LockType.Reward)
                    {
                        _rewardLock.gameObject.SetActive(true);
                        _button.onClick.AddListener(() => UnlockByReward());
                    }
                    else
                    {
                        _coinLock.gameObject.SetActive(true);
                        _coinLockText.text = locker.GoldToUnlock.ToString();
                        _button.onClick.AddListener(() => UnlockByCoin(locker.GoldToUnlock));
                    }
                }
            }
        }

        private void UnlockByCoin(int coinToUnlock)
        {
            if (GameDataManager.Instance.GameDatas.Coin >= coinToUnlock)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(Select);
                _coinLock.gameObject.SetActive(false);
                _locker.Unlocked = true;
                ProcessSelect();
                AudioManager.Instance.PlaySound(SoundID.Coin);
                GameDataManager.Instance.UpdateGold(-coinToUnlock);
                GameDataManager.Instance.SaveDatas();
            }
            else
            {
                ViewManager.Instance.GetView<NotificationPanel>().SetData("You don't have enough coins");
                ViewManager.Instance.Show<NotificationPanel>();
            }
        }

        public void UnlockByReward()
        {
           // Debug.Log("Unlock");
            AdsController.Instance.ShowReward(
                watched =>
                {
                    if (watched)
                    {
                        _rewardLock.gameObject.SetActive(false);
                        _button.onClick.RemoveAllListeners();
                        _button.onClick.AddListener(Select);
                        _locker.Unlocked = true;
                        ProcessSelect();
                        AudioManager.Instance.PlaySound(SoundID.Unlock_Item);
                        if (CraftSequenceManager.Instance.CurrentSeqeuence.PartID == PartID.Monster)
                        {
                            if (_index == 2)
                            {
                                CraftSequenceManager.Instance.IntroduceCat();
                            }
                        }
                        GameDataManager.Instance.SaveDatas();
                    }
                }
                );

        }

        Tween _punchTween;
        Tween tween;

        public void ProcessSelect()
        {
            Vibration.Vibrate(5);
            CraftSequenceManager.Instance.OnItemSelected(_index);
            _background.sprite = ItemButtonBackground.Selected;
            _selectorHighlight.gameObject.SetActive(true);
            _boothContent.OnItemSelected(_index);
            if (_punchTween != null)
            {
                _punchTween.Complete();
            }
            if (!IsDisplaying)
            {
                _punchTween = transform.DOPunchScale(new Vector3(.1f, .1f, 0), .3f);
            }
            if (!Selected)
            {
                AudioManager.Instance.PlaySound(SoundID.Tap_Item);
            }
            Selected = true;
        }
        public void Select()
        {
            if (CraftSequenceManager.Instance.CurrentSeqeuence.IsShowInter)
            {
                Debug.Log("Hể1");
                AdsController.Instance.ShowInter(
                    () =>
                    {
                        ProcessSelect();
                    }
                    );
            }
            else
            {
                Debug.Log("Hể2");

                CraftSequenceManager.Instance.OnItemSelected(_index);
                _background.sprite = ItemButtonBackground.Selected;
                _selectorHighlight.gameObject.SetActive(true);
                _boothContent.OnItemSelected(_index);
                if (_punchTween != null)
                {
                    _punchTween.Complete();
                }
                if (!IsDisplaying)
                {
                    _punchTween = transform.DOPunchScale(new Vector3(.1f, .1f, 0), .3f);
                }
                if (!Selected)
                {
                    AudioManager.Instance.PlaySound(SoundID.Tap_Item);
                }
                Selected = true;
            }

        }
        public void DeSelect()
        {
            Selected = false;
            _background.sprite = ItemButtonBackground.Deselected;
            _selectorHighlight.SetActive(false);
        }

        public void DeActive()
        {
            IsActivated = false;
            gameObject.SetActive(false);
            _image.color = new Color(0, 0, 0, 0);
            _button.enabled = false;
            _selectorHighlight.SetActive(false);
        }
        public void Active()
        {
            IsActivated = true;
            gameObject.SetActive(true);
            _button.enabled = true;
            _image.color = new Color(1, 1, 1, 1);
            _selectorHighlight.SetActive(false);

        }
        public void Display()
        {
            IsDisplaying = true;
            tween = transform.DOScale(1, .35f).SetEase(Ease.OutBack).OnComplete(
                () =>
                {
                    FullyDisplay();
                }
                );
        }

        public void FullyDisplay()
        {
            IsDisplaying = false;
            tween = null;
            transform.localScale = Vector3.one;
        }

        public void SelectWithoutTriggerAnim()
        {
            CraftSequenceManager.Instance.OnItemSelected(_index);
            _background.sprite = ItemButtonBackground.Selected;
            _selectorHighlight.gameObject.SetActive(true);
            _boothContent.OnItemSelected(_index);
            if (_punchTween != null)
            {
                _punchTween.Complete();
            }
            if (!IsDisplaying)
            {
                _punchTween = transform.DOPunchScale(new Vector3(.1f, .1f, 0), .3f);
            }
            Selected = true;
        }
    }
}