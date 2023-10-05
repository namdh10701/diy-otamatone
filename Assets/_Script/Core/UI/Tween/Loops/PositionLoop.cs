using DG.Tweening;
using UnityEngine;
namespace Core.UI
{
    public class PositionLoop : TweenLoop
    {
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
        private RectTransform _rectTransform;
        protected override void SetupLoop()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = _startPosition;
            _loop.Append(_rectTransform.DOAnchorPos3D(_endPosition, _loopDuration / 2).SetEase(_endEase));
            _loop.Append(_rectTransform.DOAnchorPos3D(_startPosition, _loopDuration / 2).SetEase(_startEase));
        }
    }
}