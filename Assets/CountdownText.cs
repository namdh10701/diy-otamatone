using Game.Audio;
using Spine;
using UnityEngine;

public class CountdownText : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public void StartCountdown()
    {
        PVPManager.Instance.ResetLevel();
        _animator.SetTrigger("StartCountdown");
    }
    public void OnCountdownEvent()
    {
        AudioManager.Instance.PlaySound(SoundID.Beep);
    }
    public void OnBeginEvent()
    {
        PVPManager.Instance.StartLevel();
    }
}
