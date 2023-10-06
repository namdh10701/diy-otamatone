using Core.UI;
using Game.Audio;
using UnityEngine;

[RequireComponent(typeof(AnimatedButton))]
public class SoundButton : MonoBehaviour
{
    [SerializeField] private AnimatedButton _animatedButton;
    [SerializeField] private SoundID _soundId;

    private void Start()
    {
        _animatedButton.AddOnClickEnvent(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySound(_soundId);
    }

    private void OnDestroy()
    {
        _animatedButton.RemoveOnClickEvent(PlaySound);
    }
}