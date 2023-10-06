using Game.Audio;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CurtainSound : MonoBehaviour
{
    public SkeletonGraphic Curtain;
    private void Awake()
    {
        Curtain = GetComponent<SkeletonGraphic>();
    }
    private void OnEnable()
    {
        Curtain.AnimationState.Start += AnimStarted;
    }
    private void OnDestroy()
    {
        Curtain.AnimationState.Start -= AnimStarted;
    }

    private void AnimStarted(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "Open" || trackEntry.Animation.Name == "Close")
        {
            AudioManager.Instance.PlaySound(SoundID.Swoosh);
        }
    }
}
