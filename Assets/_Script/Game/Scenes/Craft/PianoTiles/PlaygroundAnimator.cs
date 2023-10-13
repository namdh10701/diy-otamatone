using UnityEngine;

public class PlaygroundAnimator : MonoBehaviour
{
    public void OnDisappeared()
    {
        PVPManager.Instance.DisplayEndgameUI();
    }
}
