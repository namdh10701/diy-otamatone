using UnityEngine;
using Game.Audio;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FindingOpponentAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform playground;
    public Image bg;
    IEnumerator Start()
    {
        PVPManager.Instance.InitLevel();
        yield return new WaitForSecondsRealtime(3);
        animator.SetTrigger("Disappear");
    }
    public void FadeBg()
    {
        bg.DOFade(0, .5f);
    }
    public void OnCountDown()
    {
        AudioManager.Instance.PlaySound(SoundID.Beep);
    }

    public void OnBegin()
    {
        PVPManager.Instance.StartGame();
    }
}
