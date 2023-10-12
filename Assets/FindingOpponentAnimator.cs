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
    public GameObject levelPrefab;
    public Image bg;
    IEnumerator Start()
    {
        Instantiate(levelPrefab, playground);
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
        TileRunner.Instance.StartTheGame();
    }
}
