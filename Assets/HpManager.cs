using Core.Singleton;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HpManager : Singleton<HpManager>
{
    public Transform P1Heart;
    public TextMeshProUGUI P1HealthCount;
    public UnityEvent OnZeroHp = new UnityEvent();

    int CurrentP1Heath;
    Tween tween;
    private void Start()
    {
        PVPManager.Instance.OnNoteMissed.AddListener(player => OnNoteMissed(player));
    }

    public void OnNoteMissed(PVPManager.Player player)
    {
        if (CurrentP1Heath == 0)
        {
            return;
        }
        if (player == PVPManager.Player.P1)
        {
            CurrentP1Heath--;
        }
        if (tween != null)
        {
            tween.Complete();
        }
        tween = P1Heart.DOPunchScale(Vector3.one, .1f, 5, 1);
        P1HealthCount.text = CurrentP1Heath.ToString();

        if (CurrentP1Heath == 0)
        {
            OnZeroHp.Invoke();
        }
    }

    public void ResetHp()
    {
        CurrentP1Heath = 3;
        P1HealthCount.text = CurrentP1Heath.ToString();
    }


}
