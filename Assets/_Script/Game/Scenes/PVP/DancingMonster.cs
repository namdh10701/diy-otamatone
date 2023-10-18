using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;
using static TileRunner;

public class DancingMonster : MonoBehaviour
{
    private Player _player;
    private SkeletonAnimation _skeletonAnimation;
    string[] anims = { "Dance1", "Dance2", "Dance3", "Dance4" };
    public bool IsIdle = false;
    public bool IsRandomDance;
    public float elapsedTime = 0;
    string idleDance = "Idle_Dance";
    string prepareplay = "PrepairPlay";
    private void Awake()
    {
        if (name == "Jumbo Josh")
            idleDance = "Idle_Dancer";
        if (name == "BanBan")
        {
            prepareplay = "RepairPlay";
        }
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
    private void Start()
    {
        SelfIntroduce();
    }

    private void SelfIntroduce()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, prepareplay, true);
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        throw new NotImplementedException();
    }

    public void Init(UnityEvent<Player> onNoteMissed, UnityEvent<Player> onNoteHit, UnityEvent onStart, UnityEvent onEnd, Player side)
    {
        _player = side;
        onNoteHit.AddListener(
            (player) => OnNoteHit(player)
        );

        onNoteMissed.AddListener(
            (player) => OnNoteMissed(player)
        );

        onStart.AddListener(() => OnGameStart());
        onEnd.AddListener(() => OnGameEnd());
    }

    private void OnNoteMissed(Player player)
    {
        Debug.Log(player);
        if (player == _player)
        {
            IdleDance();
        }
    }

    private void OnNoteHit(Player player)
    {
        Debug.Log(player);
        if (player == _player)
        {
            RandomDance();
        }
    }
    private void OnGameStart()
    {
        Debug.Log("OngameStart");
        IdleDance();
    }

    public void OnGameEnd()
    {

        Debug.Log("OngameStart3");
    }

    public void IdleDance()
    {
        if (IsIdle)
            return;
        IsIdle = true;
        IsRandomDance = false;
        _skeletonAnimation.AnimationState.SetAnimation(0, idleDance, true);
    }

    public void RandomDance()
    {
        if (IsRandomDance)
        {
            if (elapsedTime < 2)
            {
                return;
            }
        }
        IsRandomDance = true;
        elapsedTime = 0;
        _skeletonAnimation.AnimationState.SetAnimation(0, anims[UnityEngine.Random.Range(0, 4)], true);
    }


    private void Update()
    {
        if (IsRandomDance)
            elapsedTime += Time.deltaTime;
    }

    public void OnReset()
    {
        SelfIntroduce();
    }
}
