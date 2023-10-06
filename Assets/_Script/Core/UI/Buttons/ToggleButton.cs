using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using JetBrains.Annotations;

namespace Core.UI
{
    public class ToggleButton : MonoBehaviour
    {
        public enum ToggleState
        {
            ON, OFF
        }
        private ToggleState _currentState;
        [SerializeField] private float _toggleDuration;
        [SerializeField] private Image _circle;
        [SerializeField] private Image _onImage;
        [SerializeField] private Image _offImage;
        [SerializeField] private Image _background;

        private float _onImageOriginalPosX;
        private float _offImageOriginalPosX;

        private bool _isTransitioning = false;

        public UnityEvent _onAction;
        public UnityEvent _offAction;

        [SerializeField] private Button _button;
        private void Awake()
        {
            _onImageOriginalPosX = _onImage.transform.localPosition.x;
            _offImageOriginalPosX = _offImage.transform.localPosition.x;
            _button.onClick.AddListener(    
                () => Toggle()
            );
        }

        public void Init(ToggleState state)
        {
            _currentState = state;
            SetDefaultPos(state);
        }

        private void SetDefaultPos(ToggleState state)
        {
            if (state == ToggleState.ON)
            {
                _circle.transform.DOLocalMoveX(_onImage.transform.localPosition.x, 0);
            }
            else
            {
                _circle.transform.DOLocalMoveX(_offImage.transform.localPosition.x, 0);
            }
        }

        public void Init(UnityEvent onAction, UnityEvent offAction)
        {
            _onAction = onAction;
            _offAction = offAction;
        }

        public void Toggle(ToggleState state)
        {
            if (_isTransitioning)
            {
                return;
            }
            _isTransitioning = true;
            _currentState = state;

            if (state == ToggleState.ON)
            {
                _onAction?.Invoke();
                HandleToggleOnAnim();
            }
            else
            {
                _offAction?.Invoke();
                HandleToggleOffAnim();
            }

            void HandleToggleOffAnim()
            {
                _circle.transform.DOLocalMoveX(_offImageOriginalPosX, _toggleDuration / 2)
                    .OnComplete(
                    () =>
                    {
                        _isTransitioning = false;
                    }
                    );

              
            }

            void HandleToggleOnAnim()
            {
                _circle.transform.DOLocalMoveX(_onImageOriginalPosX, _toggleDuration / 2)
                     .OnComplete(
                     () =>
                     {
                         _isTransitioning = false;
                     }
                     );
            }
        }
        public void Toggle()
        {
            Toggle(_currentState == ToggleState.ON ? ToggleState.OFF : ToggleState.ON);
        }
    }
}
