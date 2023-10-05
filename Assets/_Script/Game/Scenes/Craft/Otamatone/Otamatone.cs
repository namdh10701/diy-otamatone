
using DG.Tweening;
using Game.Audio;
using Monetization.Ads;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Game.Craft
{
    public class Otamatone : MonoBehaviour
    {
        public SkeletonGraphic Curtain;
        public static Dictionary<int, string> idHead_Mouth = new Dictionary<int, string>()
        {
            {0,"ManekiNeko" },
            {1,"Ninja"},
            {2,"Kabuki" },
            {3,"Huggy" },
            {4,"Kissy" },
            {5,"Poppy" },
            {6,"Mommy" },
            {7,"Pug" },
            {8,"BoxiBoo" },
            {9,"Green" },
            {10,"Purple" },
            {11,"Orange" },
            {12,"Star" },
            {13,"Minecraft" },
            {14,"Twoface" },
            {15,"Miku" }
        };

        public static Dictionary<int, string> idMouth = new Dictionary<int, string>()
        {
            {0,"ManekiNeko" },
            {1,"Ninja"},
            {2,"Kabuki" },
            {3,"Huggy" },
            {4,"Poppy" },
            {5,"Mommy" },
            {6,"Pug" },
            {7,"BoxiBoo" },
            {8,"Green" },
            {9,"Purple" },
            {10,"Orange" },
            {11,"Minecraft" },
            {12,"Twoface" },
            {13,"Miku" }
        };

        public static Dictionary<int, string> idEye = new Dictionary<int, string>()
        {
            {0,"ManekiNeko" },
            {1,"Ninja"},
            {2,"Kabuki" },
            {3,"Huggy" },
            {4,"Kissy" },
            {5,"Poppy" },
            {6,"Mommy" },
            {7,"Pug" },
            {8,"BoxiBoo" },
            {9,"Green" },
            {10,"Purple" },
            {11,"Orange" },
            {12,"Star" },
            {13,"Minecraft" },
            {14,"Twoface" },
            {15,"Miku"},
            {16,"1"},
            {17,"2"},
            {18,"3"}
        };

        public static Dictionary<int, string> idBody = new Dictionary<int, string>()
        {
            {0,"Body_1" },
            {1,"Body_2"},
            {2,"Body_3" },
            {3,"Body_4" },
            {4,"Body_5_1" },
            {5,"Body_5_2" },
            {6,"Body_6" },
            {7,"Body_7" },
            {8,"Body_8" },
            {9,"Body_9" }
        };

        [SerializeField] private Image bg1;
        [SerializeField] private SkeletonGraphic bg2Anim;

        [SerializeField] private ParticleSystem _musicNotes;
        private int _idHead = -1;
        private int _idBody = -1;
        private int _idMouth = -1;
        private int _idEye = -1;
        private int _idBackground = -1;
        private int _idMonster = -1;


        private Coroutine blinkCoroutine;
        private Tween blinkTween;
        [SerializeField] private Monster _monster;
        [SerializeField] private SortingGroup _sortingGroup;

        private Transform _originalParent;
        private Vector3 _originalScale;
        private Vector3 _originalPos;
        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            _sortingGroup.sortingLayerName = "UI";
            _originalParent = transform.parent;
            _originalScale = transform.localScale;
            _originalPos = transform.localPosition;
            _body.gameObject.SetActive(false);

            _eye.gameObject.SetActive(false);
            _mouth.gameObject.SetActive(false);
            _head.gameObject.SetActive(false);
        }
        //private SkeletonAnimation _monster;
        [SerializeField] private SkeletonAnimation _body;
        [SerializeField] private SkeletonAnimation _eye;
        [SerializeField] private SkeletonAnimation _mouth;
        //[SerializeField] private SkeletonAnimation _head;
        [SerializeField] private SkeletonAnimation _head;

        public OtamatoneParts OtamatoneParts;

        public void OnHeadSelect(int headId)
        {
            if (_idHead == -1)
            {
                _head.gameObject.SetActive(true);
            }
            _idHead = headId;
            _head.ClearState();
            _head.Skeleton.SetSkin("Head/Head_" + idHead_Mouth[_idHead]);
            _head.AnimationState.SetAnimation(0, "Appear_Head", false);
            _head.AnimationState.Complete += AnimationState_Complete;
            _head.Skeleton.SetSlotsToSetupPose();
            _head.AnimationState.Apply(_head.Skeleton);
        }


        public void OnBodySelect(int bodyId)
        {
            if (_idBody == -1)
            {
                _body.gameObject.SetActive(true);
            }
            _idBody = bodyId;

            _body.ClearState();
            _body.Skeleton.SetSkin("Body/" + idBody[_idBody]);
            _body.AnimationState.SetAnimation(0, "Appear_Body", false);
            _body.Skeleton.SetSlotsToSetupPose();
            _body.AnimationState.Apply(_head.Skeleton);
        }
        public void OnMouthSelect(int mouthId)
        {
            if (_idMouth == -1)
            {
                _mouth.gameObject.SetActive(true);
            }
            _idMouth = mouthId;

            _mouth.Skeleton.SetSkin("Mouth/Mouth_" + idMouth[_idMouth]);
            _mouth.AnimationState.SetAnimation(0, "Appear_Mouth", false);
            _mouth.AnimationState.Complete += AnimationState_Complete;
            _mouth.Skeleton.SetSlotsToSetupPose();
            _mouth.AnimationState.Apply(_head.Skeleton);
        }
        public void OnEyeSelect(int eyeId)
        {
            if (_idEye == -1)
            {
                _eye.gameObject.SetActive(true);
            }
            _idEye = eyeId;
            string temp = "";
            if (idEye[_idEye] == "Ninja")
            {
                temp = "ninja";
            }
            else
            {
                temp = idEye[_idEye];
            }
            _eye.ClearState();
            _eye.Skeleton.SetSkin("Eye/Eye_" + temp);
            _eye.AnimationState.SetAnimation(0, "Appear_Eye", false);
            _eye.AnimationState.Complete += AnimationState_Complete;
            _eye.Skeleton.SetSlotsToSetupPose();
            _eye.AnimationState.Apply(_head.Skeleton);
        }

        private void AnimationState_Complete(Spine.TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == "Appear_Eye")
            {
                _eye.AnimationState.SetAnimation(0, "Idle", true);
                _eye.AnimationState.GetCurrent(0).TrackTime = _head.AnimationState.GetCurrent(0).TrackTime;
            }
            if (trackEntry.Animation.Name == "Appear_Mouth")
            {
                _mouth.AnimationState.SetAnimation(0, "Idle", true);
                _mouth.AnimationState.GetCurrent(0).TrackTime = _eye.AnimationState.GetCurrent(0).TrackTime;
            }
            if (trackEntry.Animation.Name == "Appear_Head")
            {
                _head.AnimationState.SetAnimation(0, "Idle", true);
                if (_idEye != -1)
                {
                    _head.AnimationState.GetCurrent(0).TrackTime = _eye.AnimationState.GetCurrent(0).TrackTime;
                }
            }
        }
        public void OnBackgroundSelect(int backgroundId)
        {
            _idBackground = backgroundId;
            bg1.sprite = OtamatoneParts.Parts[4].Sprites[backgroundId];
        }
        public void OnMonsterSelect(int monsterId)
        {
            _idMonster = monsterId;
            MonsterManager.Instance.SelectMonster(monsterId);
            _monster = MonsterManager.Instance.Monster;
            _monster.ResetMonster();
            _monster.OnBop.AddListener(RunPlay);
        }

        public void ResetOtamatone()
        {
            BeingPlayed = false;
            _sortingGroup.sortingLayerName = "UI";
            _musicNotes.Clear();
            _musicNotes.Stop();
            MonsterManager.Instance.HideAllMonster();
            bg1.DOFade(1, 0);
            bg2Anim.DOFade(1, 0);
            bg2Anim.gameObject.SetActive(false);
            _idHead = -1;
            _idBody = -1;
            _idMouth = -1;
            _idEye = -1;
            _idBackground = -1;
            _idMonster = -1;
            _body.gameObject.SetActive(false);
            _eye.gameObject.SetActive(false);
            _mouth.gameObject.SetActive(false);
            _head.gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            transform.parent = _originalParent;
            transform.localPosition = _originalPos;
            transform.localScale = _originalScale;
        }

        public void FinishCraft()
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            //sequence.Append(transform.DOMoveY(transform.position.y + .6f, 1f).SetEase(Ease.Linear));
            sequence.Append(transform.DOScale(0, .4f).SetEase(Ease.InBack).OnComplete(
                () =>
                {
                    Curtain.AnimationState.SetAnimation(0, "Open", false);
                    Curtain.AnimationState.AddAnimation(0, "Close", false, 1.25f);
                    Curtain.AnimationState.AddAnimation(0, "Open_Idle", false, 0);
                }
                ));

            sequence.AppendInterval(.6f).OnComplete(
                () =>
                {
                    Vibration.Vibrate(20);
                }
                );
            sequence.Append(transform.DOMove(_monster.LeftHandIK.transform.position, 0));
            sequence.Append(transform.DORotate(new Vector3(0, 0, 49), 0));
            sequence.Append(transform.DOScale(new Vector3(-.85f, .85f, .85f), .0f).SetEase(Ease.OutBack));
            sequence.AppendInterval(.6f);
            sequence.Append(transform.DOMove(_monster.LeftHandIK.transform.position, 0).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _sortingGroup.sortingLayerName = "Default";

                    transform.parent = _monster.LeftHandIK.transform;

                    if (_idMonster == 0)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -95);
                        transform.localPosition = new Vector3(0f, -0.08f, 0);
                    }
                    else if (_idMonster == 1)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -100);
                        transform.localPosition = new Vector3(-0f, 0.25f, 0);
                    }
                    else if (_idMonster == 2)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -100);
                        transform.localPosition = new Vector3(0.15f, .1f, 0);
                    }
                    else if (_idMonster == 3)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -93);
                        transform.localPosition = new Vector3(-0.2f, -.1f, 0);
                    }
                    else if (_idMonster == 4)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -97.852f);
                        transform.localPosition = new Vector3(0, -0.2f, 0);
                    }
                    else if (_idMonster == 5)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -89);
                        transform.localPosition = new Vector3(-0.2f, -.2f, 0);
                    }
                    else if (_idMonster == 6)
                    {
                        transform.localRotation = Quaternion.Euler(-0.22f, 0.21f, -97.1f);
                        transform.localPosition = new Vector3(-.15f, -0f, 0);
                    }
                    else if (_idMonster == 7)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, -89.28f);
                        transform.localPosition = new Vector3(-0.7f, -0.073f, 0);
                    }
                    AudioManager.Instance.PlayMusic(SoundID.Win_BGM, true);
                    _musicNotes.Play();
                    _monster.Play();
                })
                .OnPlay(
                () =>
                {
                    bg1.DOFade(0, 0f).OnComplete(
                        () =>
                        {
                            bg2Anim.gameObject.SetActive(true);
                            bg2Anim.DOFade(1, .2f);
                            bg2Anim.AnimationState.SetAnimation(0, "Idle", true);
                        }
                        );
                }
                )
                );
            sequence.AppendInterval(2f).OnPlay(
                () =>
                {
                }
                ).OnComplete(
                () =>
                {
                    if (CraftSequenceManager.BeforeWinPanelShowInter)
                    {
                        AdsController.Instance.ShowInter(
                            () =>
                            {
                                CraftSequenceManager.Instance.ShowFinishPanel();
                            }
                            );
                    }
                }
                );
            sequence.Play();
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.up);
        }

        public bool BeingPlayed = false;
        public void RunPlay()
        {
            BeingPlayed = true;
            _head.AnimationState.SetAnimation(0, "Play", false);
            _body.AnimationState.SetAnimation(0, "Play", false);
            _eye.AnimationState.SetAnimation(0, "Play", false);
            _mouth.AnimationState.SetAnimation(0, "Play", false);
        }


    }
}