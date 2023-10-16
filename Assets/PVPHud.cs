using UnityEngine;

public class PVPHud : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public void HideHud()
    {
        _animator.SetTrigger("HUD Dissappear");
    }
    public void OnDisappeared()
    {
        PVPManager.Instance.DisplayEndgameUI();
    }
}
