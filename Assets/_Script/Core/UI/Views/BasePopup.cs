using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using System.Linq;

namespace Core.UI
{
    public class BasePopup : View
    {
        [SerializeField] protected Image _panel;
        [SerializeField] protected Transform _contents;
        protected Tween openTween;
        protected Tween closeTween;
        protected Button[] _buttons;
        Action _onClose;
        public enum State
        {
            CLOSED, OPENED, CLOSING, OPENING
        }
        public State CurrentState { get; protected set; }
        protected virtual void Awake()
        {
            _panel?.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Hide();
               }
               );
            _buttons = GetComponentsInChildren<Button>().ToArray();
            CurrentState = State.CLOSED;
        }

        public void HideImmediately()
        {
            gameObject.SetActive(false);
            CurrentState = State.CLOSED;
        }

        public override void Show()
        {
            if (CurrentState == State.OPENED || CurrentState == State.OPENING)
            {
                return;
            }
            if (openTween != null)
            {
                return;
            }
            CurrentState = State.OPENING;
            gameObject.SetActive(true);
            _contents.transform.localScale = Vector3.zero;
            openTween = _contents.transform.DOScale(1, .2f).SetEase(Ease.OutBack).SetUpdate(true);
            openTween.onComplete += () =>
            {
                foreach (var button in _buttons)
                {
                    button.enabled = true;
                }
                openTween = null;
                CurrentState = State.OPENED;
            };
        }

        public void Show(Action Onclose)
        {
            _onClose = Onclose;
            if (CurrentState == State.OPENED || CurrentState == State.OPENING)
            {
                return;
            }
            if (openTween != null)
            {
                return;
            }
            CurrentState = State.OPENING;
            gameObject.SetActive(true);
            _contents.transform.localScale = Vector3.zero;
            openTween = _contents.transform.DOScale(1, .2f).SetEase(Ease.OutBack).SetUpdate(true);
            openTween.onComplete += () =>
            {
                foreach (var button in _buttons)
                {
                    button.enabled = true;
                }
                openTween = null;
                CurrentState = State.OPENED;
            };
        }

        public override void Hide()
        {
            
            if (CurrentState == State.CLOSED || CurrentState == State.CLOSING)
            {
                return;
            }
            if (closeTween != null)
            {
                return;
            }
            CurrentState = State.CLOSING;
            foreach (var button in _buttons)
            {
                button.enabled = false;
            }
            _contents.transform.localScale = Vector3.one;
            closeTween = _contents.transform.DOScale(0, .2f).SetEase(Ease.InBack).SetUpdate(true);
            closeTween.onComplete += () =>
            {
                closeTween = null;
                gameObject.SetActive(false);
                CurrentState = State.CLOSED;
                if (_onClose != null)
            {
                _onClose.Invoke();
                _onClose = null;
            }
            };
        }

        public virtual void Hide(Action onClosed)
        {
            if (closeTween != null)
            {
                return;
            }
            Hide();
            closeTween.onComplete += () =>
            {
                onClosed?.Invoke();
            };
        }

        private void OnDestroy()
        {
            openTween.Kill();
            closeTween.Kill();
        }
    }
}