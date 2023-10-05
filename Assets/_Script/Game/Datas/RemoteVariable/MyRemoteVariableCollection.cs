using Services.FirebaseService.Remote;
using UnityEngine;

namespace Game.RemoteVariable
{
    [System.Serializable]
    public class MyRemoteVariableCollection : RemoteVariableCollection
    {
        public RemoteBool IsInterOn;
        public RemoteBool IsRewardOn;

        public RemoteDouble InterInterInterval;
        public RemoteDouble InterRewardInterval;
        public RemoteDouble OpenOpenInterval;
        public RemoteDouble OpenInterInterval;

        public RemoteBool BeforeWinPanelShowInter;
        public RemoteBool BgTapShowInter;
        public RemoteBool BodyTapShowInter;
        public RemoteBool EyeTapShowInter;
        public RemoteBool HeadTapShowInter;
        public RemoteBool MonsterTapShowInter;
        public RemoteBool MouthTapShowInter;
        public RemoteBool NextBtnTapShowInter;
        public RemoteBool SequenceBtnTapShowInter;
        public MyRemoteVariableCollection()
        {
            IsInterOn = new RemoteBool("IsInterOn", true);
            IsRewardOn = new RemoteBool("IsRewardOn", true);

            InterInterInterval = new RemoteDouble("InterInterInterval", 20);
            InterRewardInterval = new RemoteDouble("InterRewardInterval", 5);
            OpenOpenInterval = new RemoteDouble("OpenOpenInterval", 3);
            OpenInterInterval = new RemoteDouble("OpenInterInterval", 3);

            BeforeWinPanelShowInter = new RemoteBool("BeforeWinPanelShowInter", true);
            BgTapShowInter = new RemoteBool("BgTapShowInter", true);
            BodyTapShowInter = new RemoteBool("BodyTapShowInter", true);
            EyeTapShowInter = new RemoteBool("EyeTapShowInter", true);
            HeadTapShowInter = new RemoteBool("HeadTapShowInter", true);
            MonsterTapShowInter = new RemoteBool("MonsterTapShowInter", true);
            MouthTapShowInter = new RemoteBool("MouthTapShowInter", true);
            NextBtnTapShowInter = new RemoteBool("NextBtnTapShowInter", true);
            SequenceBtnTapShowInter = new RemoteBool("SequenceBtnTapShowInter", true);
        }

        public override void AddToFetchQueue()
        {
            AddVariable(IsInterOn);
            AddVariable(IsRewardOn);

            AddVariable(InterInterInterval);
            AddVariable(InterRewardInterval);
            AddVariable(OpenOpenInterval);
            AddVariable(OpenInterInterval);

            AddVariable(BeforeWinPanelShowInter);
            AddVariable(BgTapShowInter);
            AddVariable(BodyTapShowInter);
            AddVariable(EyeTapShowInter);
            AddVariable(HeadTapShowInter);
            AddVariable(MonsterTapShowInter);
            AddVariable(MouthTapShowInter);
            AddVariable(NextBtnTapShowInter);
            AddVariable(SequenceBtnTapShowInter);
        }
    }
}