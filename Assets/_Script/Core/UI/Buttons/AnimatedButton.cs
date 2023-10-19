using DG.Tweening;
using Game.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{

    public class AnimatedButton : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {

        private Button _button;
        private Transform _transform;
        [SerializeField] private SoundID _soundID = SoundID.Button_Click;
        [SerializeField] private UnityEvent _onClickEvent;
        [SerializeField] private bool _isSpamable;
        [SerializeField] private float _clickCooldownDuration = .5f; // Adjust the duration as needed

        private bool _isCooldown;
        private bool _clickedDown = false;
        private bool _isDragging;
        private Vector3 _pointerDownPos;
        public float originalScale;
        Tween tween;
        protected override void Awake()
        {
            _clickCooldownDuration = .2f;
            _button = GetComponent<Button>();
            _transform = GetComponent<Transform>();
            originalScale = _transform.localScale.x;
        }
        public void AddOnClickEnvent(UnityAction onClick)
        {
            _onClickEvent.AddListener(onClick);
        }
        public void RemoveOnClickEvent(UnityAction onClick)
        {
            _onClickEvent.RemoveListener(onClick);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_button.interactable && _button.enabled && !_isCooldown)
            {
                _pointerDownPos = eventData.position;
                _clickedDown = true;
                _isDragging = false;
                tween = _transform.DOScale(originalScale * .9f, .1f).SetUpdate(true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_button.interactable && _button.enabled && !_isCooldown && _clickedDown)
            {
                tween = _transform.DOScale(originalScale, .1f).SetEase(Ease.OutBack).SetUpdate(true);
                if (!_isDragging)
                {
                    _onClickEvent?.Invoke();
                    AudioManager.Instance.PlaySound(_soundID);
                }
                _clickedDown = false;

                if (!_isSpamable)
                {
                    _isCooldown = true;
                    Invoke("CompleteCooldown", _clickCooldownDuration);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            tween.Kill();
        }
        private void CompleteCooldown()
        {
            _isCooldown = false;
        }
        public void OnPointerMove(PointerEventData eventData)
        {
            if (_button.enabled && _clickedDown)
            {
                if (Vector2.Distance(_pointerDownPos, eventData.position) > EventSystem.current.pixelDragThreshold)
                {
                    _isDragging = true;
                }
            }
        }
    }
}
