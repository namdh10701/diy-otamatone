using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CompleteOtamatoneUI : MonoBehaviour
{
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

    public static int _idHead = -1;
    public static int _idBody = -1;
    public static int _idMouth = -1;
    public static int _idEye = -1;

    public SkeletonGraphic _head;
    public SkeletonGraphic _eye;
    public SkeletonGraphic _body;
    public SkeletonGraphic _mouth;

    public SkeletonGraphic _monster;
    public void Start()
    {
        _monster = GetComponentInParent<SkeletonGraphic>();
        _monster.AnimationState.Event += OnAnimationEvent;

        if (_idHead == -1)
        {
            _idHead = Random.Range(0, 16);
        }
        _head.Clear();
        _head.Skeleton.SetSkin("Head/Head_" + idHead_Mouth[_idHead]);
        _head.AnimationState.SetAnimation(0, "Idle", false);

        _head.Skeleton.SetSlotsToSetupPose();
        if (_idEye == -1)
        {
            _idEye = Random.Range(0, 19);
        }

        string temp = "";
        if (idEye[_idEye] == "Ninja")
        {
            temp = "ninja";
        }
        else
        {
            temp = idEye[_idEye];
        }
        _eye.Clear();
        _eye.Skeleton.SetSkin("Eye/Eye_" + temp);
        _eye.AnimationState.SetAnimation(0, "Idle", false);
        _eye.Skeleton.SetSlotsToSetupPose();


        if (_idBody == -1)
        {
            _idBody = Random.Range(0, 10);
        }

        _body.Clear();
        _body.Skeleton.SetSkin("Body/" + idBody[_idBody]);
        _body.AnimationState.SetAnimation(0, "Idle", false);
        _body.Skeleton.SetSlotsToSetupPose();
        if (_idMouth == -1)
        {
            _idMouth = Random.Range(0, 14);
        }
        _mouth.Clear();
        _mouth.Skeleton.SetSkin("Mouth/Mouth_" + idMouth[_idMouth]);
        _mouth.AnimationState.SetAnimation(0, "Idle", false);

        _mouth.Skeleton.SetSlotsToSetupPose();
    }

    private void OnAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Bop")
        {

            RunPlay();
        }
    }

    public void RunPlay()
    {
        _head.AnimationState.SetAnimation(0, "Play", false);
        _body.AnimationState.SetAnimation(0, "Play", false);
        _eye.AnimationState.SetAnimation(0, "Play", false);
        _mouth.AnimationState.SetAnimation(0, "Play", false);
    }
}
