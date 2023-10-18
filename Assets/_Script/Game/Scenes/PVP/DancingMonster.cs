using Game.Craft;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;
using static TileRunner;

public class DancingMonster : MonoBehaviour
{
    public enum State
    {
        Introducing, Idle, Dance, Final
    }
    private Player _player;
    private State _currentState;
    private SkeletonAnimation _skeletonAnimation;
    string[] anims = { "Dance1", "Dance2", "Dance3", "Dance4" };
    public float idleTime = 0;
    public float danceTime = 0;
    string idleDance = "Idle_Dance";
    string prepareplay = "PrepairPlay";
    public GameObject otamatone;
    public float tapBuffer = 2;
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
        if (player == _player)
        {
            if (tapBuffer > 0)
                return;
            IdleDance();
        }
    }

    private void OnNoteHit(Player player)
    {
        if (player == _player)
        {
            tapBuffer = 2;
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
        if (_currentState == State.Idle)
        {
            return;
        }
        _currentState = State.Idle;
        _skeletonAnimation.AnimationState.SetAnimation(0, idleDance, true);
    }

    public void RandomDance()
    {
        if (_currentState == State.Dance)
        {
            if (danceTime < 2)
            {
                return;
            }
        }
        _currentState = State.Dance;
        danceTime = 0;
        _skeletonAnimation.AnimationState.SetAnimation(0, anims[UnityEngine.Random.Range(0, 4)], true);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Idle:
                idleTime += Time.deltaTime;
                break;
            case State.Dance:
                danceTime += Time.deltaTime;
                break;
        }
        if (_currentState != State.Final && _currentState != State.Introducing)
        {
            tapBuffer -= Time.deltaTime;
            if (tapBuffer < -2 && _currentState == State.Dance)
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
