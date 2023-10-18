using Game.Craft;
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
    public GameObject otamatone;
    private void Awake()
    {
        otamatone = transform.Find("OtamatoneIK").gameObject;
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

    public void Init(UnityEvent<Player> onNoteMissed, UnityEvent<Player> onNoteHit, UnityEvent onStart, UnityEvent onEnd, UnityEvent onLastNote, Player side)
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
        onLastNote.AddListener(() => OnLastNote());
    }

    private void OnLastNote()
    {
        otamatone.gameObject.SetActive(false);
        _skeletonAnimation.AnimationState.SetAnimation(0, "Idle8", true);
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
        IdleDance();
    }

    public void OnGameEnd()
    {

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
        Invoke("CheckIdleDance", 2);
    }

    void CheckIdleDance()
    {
        if (elapsedTime > 2)
        {
            IdleDance();
        }
    }

    private void Update()
    {
        if (IsRandomDance)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 3)
            {
                IdleDance();
            }
        }
        
    }

    public void OnReset()
    {
        otamatone.gameObject.SetActive(true);
        SelfIntroduce();
    }
}
