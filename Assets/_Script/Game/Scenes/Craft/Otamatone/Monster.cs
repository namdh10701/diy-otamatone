/*using Game.Craft;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum State
    {
        Appearing, Idle, Angry, Other, OtamaneReceiving, PreparingToPlay, Playing
    }



    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;

    [SerializeField] public GameObject LeftHandIK;

    private readonly string RecieveOtamane = "Idle8";
    private readonly string OnSelectAnimationName = "Idle7";
    private readonly string IdleAnimationName = "Idle1";
    private readonly string LosePatienceIdleAnimName = "Idle2";
    private readonly string[] OnItemButtonSelectAnimName = { "Idle3", "Idle4", "Idle5" };
    private readonly string OnSequenceButtonSelectAnimName = "Idle6";
    private readonly string PreparingPlayAnimName = "RepairPlay";
    private readonly string PlayAnimName = "Play";



    private float elapsedTime = 0;

    public State CurrentState;

    private void Awake()
    {
        //PlayAnim(IdleAnimationName);
        _skeletonAnimation.clearStateOnDisable = true;
        Debug.Log("awake");
    }
    private void OnEnable()
    {
        Otamatone.OnSequenceButtonSelect.AddListener(OnSequenceButtonSelect);
        Otamatone.OnItemmButtonSelect.AddListener(OnItemButtonSelect);
        Otamatone.OnNextButtonClick.AddListener(OnNextButtonClick);
        _skeletonAnimation.AnimationState.Complete += OnAnimationCompleted;
    }

    private void OnDisable()
    {
        Otamatone.OnSequenceButtonSelect.RemoveListener(OnSequenceButtonSelect);
        Otamatone.OnItemmButtonSelect.RemoveListener(OnItemButtonSelect);
        Otamatone.OnNextButtonClick.RemoveListener(OnNextButtonClick);
        _skeletonAnimation.AnimationState.Complete -= OnAnimationCompleted;
    }

    private void OnNextButtonClick()
    {

    }

    public void SelectSkin(int skinId)
    {
        _skeletonAnimation.Skeleton.SetSkin(_skinIdNameMap[skinId]);
        _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
        PlayAnim(OnSelectAnimationName);
    }

    private void OnAnimationCompleted(TrackEntry trackEntry)
    {
        if (!trackEntry.Loop)
        {
            *//*            if (trackEntry.Animation.Name == "Idle7" || trackEntry.Animation.Name == "Idle3")
                        {
                            _skeletonAnimation.ClearState();
                        }*//*
            PlayAnim(IdleAnimationName);
        }
    }

    private void OnItemButtonSelect()
    {
        elapsedTime = 0;

        if (CraftSequenceManager.Instance.CurrentSeqeuence.PartID != CraftStateSequence.PartID.Monster)
        {
            string animName = OnItemButtonSelectAnimName[UnityEngine.Random.Range(0, OnItemButtonSelectAnimName.Length)];
            PlayAnim(animName);
        }
    }

    private void OnSequenceButtonSelect()
    {
        PlayAnim(OnSequenceButtonSelectAnimName);
    }

    public void PlayInstrument()
    {
        PlayAnim(PlayAnimName);
    }

    public void PreparePlay()
    {
        PlayAnim(PreparingPlayAnimName);
    }
    public void ReieveOmatamane()
    {
        PlayAnim(RecieveOtamane);
    }

    private void Update()
    {
        if (CurrentState == State.Idle)
        {
            elapsedTime += Time.deltaTime;
        }
        if (elapsedTime > 4)
        {
            PlayAnim(LosePatienceIdleAnimName);
        }
    }

    public void ResetMonster()
    {
        elapsedTime = 0;
        PlayAnim(OnSelectAnimationName);
        _skeletonAnimation.Update(0);
    }

    public void PlayAnim(string animName)
    {

        switch (animName)
        {
            case "Idle1":
                elapsedTime = 0;
                CurrentState = State.Idle;
                _skeletonAnimation.ClearState();
                _skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimationName, true);
                _skeletonAnimation.Update(0);
                break;
            case "Idle2":

                if (CurrentState != State.Angry)
                {
                    CurrentState = State.Angry;
                    _skeletonAnimation.AnimationState.SetAnimation(0, LosePatienceIdleAnimName, true);
                }
                break;
            case "Idle3":
            case "Idle4":
            case "Idle5":

                elapsedTime = 0;
                if (CurrentState != State.Other)
                {
                    CurrentState = State.Other;
                    _skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
                }
                break;
            case "Idle6":
                elapsedTime = 0;
                if (CurrentState != State.Other)
                {
                    CurrentState = State.Other;
                    _skeletonAnimation.AnimationState.SetAnimation(0, OnSequenceButtonSelectAnimName, false);
                }
                break;
            case "Idle7":
                Debug.Log("appear");
                elapsedTime = 0;
                CurrentState = State.Appearing;
                _skeletonAnimation.AnimationState.SetAnimation(0, OnSelectAnimationName, false);
                break;
            case "Idle8":
                CurrentState = State.OtamaneReceiving;
                _skeletonAnimation.AnimationState.SetAnimation(0, "Idle8", true);
                break;
            case "RepairPlay":
                CurrentState = State.PreparingToPlay;
                _skeletonAnimation.AnimationState.SetAnimation(0, "RepairPlay", true);
                break;
            case "Play":
                CurrentState = State.Playing;
                _skeletonAnimation.AnimationState.SetAnimation(0, "Play", true);
                break;
        }
    }
}*/



using Game.Audio;
using Game.Craft;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{

    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;
    public UnityEvent OnBop = new UnityEvent();

    [SerializeField] public GameObject LeftHandIK;

    private int defaultTrack = 0;
    private bool preparingToPlay = false;
    [SerializeField] public Transform target;


    private List<string> anims = new List<string>
    {
        "Idle1","Idle3", "Idle4","Idle5"
    };


    public enum TimerState
    {
        RUN, STOP
    }
    private TimerState _timerState;
    private float _timeToIdle2 = 0;
    private const float Rest_Time_To_Idle2 = 3;
    private bool _isIdle2 = false;
    private void Awake()
    {
        _skeletonAnimation.clearStateOnDisable = true;
    }

    private void OnEnable()
    {
        CraftSequenceManager.OnActionTaken.AddListener(OnActionTaken);
        CraftSequenceManager.OnHeadPicked.AddListener(OnHeadPicked);
        CraftSequenceManager.OnOtamatoneCompeleted.AddListener(OnMouthPicked);
        CraftSequenceManager.OnDoneClicked.AddListener(OnDoneClicked);
        _skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
        _skeletonAnimation.AnimationState.Event += OnAnimationEvent;
        _skeletonAnimation.AnimationState.Start += OnAnimationStarted;

    }


    private void OnDisable()
    {
        CraftSequenceManager.OnActionTaken.RemoveListener(OnActionTaken);
        CraftSequenceManager.OnHeadPicked.RemoveListener(OnHeadPicked);
        CraftSequenceManager.OnOtamatoneCompeleted.RemoveListener(OnMouthPicked);
        CraftSequenceManager.OnDoneClicked.RemoveListener(OnDoneClicked);
        _skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
        _skeletonAnimation.AnimationState.Event -= OnAnimationEvent;
        _skeletonAnimation.AnimationState.Start -= OnAnimationStarted;
        OnBop.RemoveAllListeners();
    }
    private void OnAnimationStarted(TrackEntry trackEntry)
    {
        if (!preparingToPlay && trackEntry.Animation.Name != "Idle7")
        {
            AudioManager.Instance.PlaySound(SoundID.Monster_Voice);
        }
    }

    private void OnAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Bop")
        {
            AudioManager.Instance.PlaySound(SoundID.Otomatune_Event);
            OnBop.Invoke();
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundID.Knock);
        }
    }
    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (!trackEntry.Loop && !preparingToPlay && !_isIdle2)
        {
            List<string> temp = new List<string>();
            temp.AddRange(anims);
            temp.Remove(_skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name);
            PlayAnim(temp[UnityEngine.Random.Range(0, temp.Count)]);

        }
    }

    private void OnActionTaken()
    {
        if (_isIdle2)
        {
            PlayAnim(anims[UnityEngine.Random.Range(0, anims.Count)]);
        }
        _timeToIdle2 = 0;
        _isIdle2 = false;
        /*  List<string> temp = new List<string>();
          temp.AddRange(anims);
          temp.Remove(_skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name);*/
        /*        if (_skeletonAnimation.AnimationState.GetCurrent(defaultTrack) == null)
                {
                    PlayAnim(anims[UnityEngine.Random.Range(0, anims.Count)]);
                }*/
    }

    private void OnMouthPicked()
    {
        PlayAnim("Idle8");
    }

    private void OnHeadPicked()
    {
        PlayAnim("Idle6");
    }

    private void OnDoneClicked()
    {
        _timeToIdle2 = 0;
        _timerState = TimerState.STOP;
        PlayAnim("Idle8", true);
    }

    private void LateUpdate()
    {
        if (_timerState == TimerState.RUN)
        {
            _timeToIdle2 += Time.deltaTime;
        }
        if (_timeToIdle2 >= Rest_Time_To_Idle2 && !_isIdle2)
        {
            _isIdle2 = true;
            PlayAnim("Idle2", true);
        }

    }

    private void Update()
    {

        Debug.DrawRay(LeftHandIK.transform.position, LeftHandIK.transform.right, Color.red);
    }
    public void ResetMonster()
    {
        preparingToPlay = false;
        _timeToIdle2 = 0;
        PlayAnim("Idle7");
        _timerState = TimerState.RUN;
        _skeletonAnimation.Update(0);
    }

    public void PlayAnim(string animName, bool isLoop = false)
    {
        if (_skeletonAnimation.AnimationState.GetCurrent(defaultTrack) != null)
        {
            if (_skeletonAnimation.AnimationState.GetCurrent(defaultTrack).Animation.Name == animName)
            {
                return;
            }
        }

        if (animName == "Idle1")
        {
            _skeletonAnimation.ClearState();
        }
        _skeletonAnimation.AnimationState.SetAnimation(defaultTrack, animName, isLoop);
    }

    public void PreparingToPlay()
    {
        preparingToPlay = true;
        _skeletonAnimation.AnimationState.SetAnimation(defaultTrack, "PrepairPlay", true);
    }

    public void Play()
    {
        preparingToPlay = true;
        _skeletonAnimation.AnimationState.SetAnimation(defaultTrack, "Play", true);
    }
}