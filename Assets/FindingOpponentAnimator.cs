using UnityEngine;
using Game.Audio;
using UnityEngine.Events;
using System.Collections;

public class FindingOpponentAnimator : MonoBehaviour
{
    Animator animator;
    public Transform playground;
    public GameObject levelPrefab;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    IEnumerator Start()
    {
        Instantiate(levelPrefab, playground);
        yield return new WaitForSecondsRealtime(3);
        animator.SetTrigger("Disappear");
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
